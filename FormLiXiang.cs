using System;
using System.IO;
using System.Windows.Forms;
using OSGeo.GDAL;

namespace FSGIS
{
    public partial class FormLiXiang : Form
    {
        private MapControl _map;

        public FormLiXiang(MapControl map)
        {
            InitializeComponent();
            _map = map;
            LoadLayers();
        }

        private void LoadLayers()
        {
            cmbLayers.Items.Clear();
            foreach (var layer in _map.Layers)
            {
                if (layer is RasterLayer)
                    cmbLayers.Items.Add(layer);
            }
            cmbLayers.DisplayMember = "LayerName";
            if (cmbLayers.Items.Count > 0)
                cmbLayers.SelectedIndex = 0;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog
            {
                Filter = "GeoTIFF|*.tif",
                FileName = "Xiang.tif"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                    txtOutputPath.Text = sfd.FileName;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cmbLayers.SelectedItem == null)
            {
                MessageBox.Show("请选择一个 DEM 图层。");
                return;
            }

            var layer = cmbLayers.SelectedItem as RasterLayer;
            if (layer == null || !File.Exists(layer.FilePath))
            {
                MessageBox.Show("输入文件不存在。");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOutputPath.Text))
            {
                MessageBox.Show("请选择输出路径。");
                return;
            }

            try
            {
                Dataset dsIn = Gdal.Open(layer.FilePath, Access.GA_ReadOnly);
                int width = dsIn.RasterXSize;
                int height = dsIn.RasterYSize;

                double[] gt = new double[6];
                dsIn.GetGeoTransform(gt);

                Band inBand = dsIn.GetRasterBand(1);

                // 读取 NoData
                double inNoData;
                int hasNoData;
                inBand.GetNoDataValue(out inNoData, out hasNoData);
                bool hasND = hasNoData != 0;


                float[] demData = new float[width * height];
                inBand.ReadRaster(0, 0, width, height, demData, width, height, 0, 0);

                float[] outData = new float[width * height];

                // 默认全部赋为 NoData（防止边界或未写入）
                if (hasND)
                {
                    for (int i = 0; i < outData.Length; i++)
                        outData[i] = (float)inNoData;
                }

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        int idx = y * width + x;

                        // 核心：原 DEM 为 NoData → 输出仍为 NoData
                        if (hasND && demData[idx] == inNoData)
                        {
                            outData[idx] = (float)inNoData;
                            continue;
                        }

                        float z1 = demData[(y - 1) * width + (x - 1)];
                        float z2 = demData[(y - 1) * width + x];
                        float z3 = demData[(y - 1) * width + (x + 1)];
                        float z4 = demData[y * width + (x - 1)];
                        float z6 = demData[y * width + (x + 1)];
                        float z7 = demData[(y + 1) * width + (x - 1)];
                        float z8 = demData[(y + 1) * width + x];
                        float z9 = demData[(y + 1) * width + (x + 1)];

                        double dzdx = ((z3 + 2 * z6 + z9) - (z1 + 2 * z4 + z7)) / 8.0;
                        double dzdy = ((z7 + 2 * z8 + z9) - (z1 + 2 * z2 + z3)) / 8.0;

                        double aspect;
                        if (Math.Abs(dzdx) < 1e-5 && Math.Abs(dzdy) < 1e-5)
                        {
                            aspect = -1;
                        }
                        else
                        {
                            aspect = 57.29578 * Math.Atan2(dzdy, -dzdx);
                            if (aspect < 0) aspect = 90 - aspect;
                            else if (aspect > 90) aspect = 360.0 - aspect + 90.0;
                            else aspect = 90.0 - aspect;
                        }

                        float score;
                        if (aspect < 0)
                        {
                            score = 0;
                        }
                        else
                        {
                            if (aspect >= 337.5 || aspect < 22.5) score = 0.5f;
                            else if (aspect < 67.5) score = 0.2f;
                            else if (aspect < 112.5) score = 0.8f;
                            else if (aspect < 157.5) score = 0.9f;
                            else if (aspect < 202.5) score = 1.0f;
                            else if (aspect < 247.5) score = 0.6f;
                            else if (aspect < 292.5) score = 0.4f;
                            else score = 0.1f;
                        }

                        outData[idx] = score;
                    }
                }

                Driver drv = Gdal.GetDriverByName("GTiff");
                Dataset dsOut = drv.Create(
                    txtOutputPath.Text,
                    width,
                    height,
                    1,
                    DataType.GDT_Float32,
                    null);

                dsOut.SetGeoTransform(gt);
                dsOut.SetProjection(dsIn.GetProjection());

                Band outBand = dsOut.GetRasterBand(1);
                if (hasND)
                    outBand.SetNoDataValue(inNoData);

                outBand.WriteRaster(0, 0, width, height, outData, width, height, 0, 0);

                dsOut.FlushCache();
                dsOut.Dispose();
                dsIn.Dispose();

                MessageBox.Show("立向完成！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }
    }
}
