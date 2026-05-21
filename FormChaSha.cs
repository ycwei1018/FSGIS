using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FSGIS
{
    public partial class FormChaSha : Form
    {
        private MapControl _map;

        public FormChaSha(MapControl map)
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
                if (layer is RasterLayer) cmbLayers.Items.Add(layer);
            }
            cmbLayers.DisplayMember = "LayerName";
            if (cmbLayers.Items.Count > 0) cmbLayers.SelectedIndex = 0;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog { Filter = "GeoTIFF|*.tif", FileName = "Sha.tif" })
            {
                if (sfd.ShowDialog() == DialogResult.OK) txtOutputPath.Text = sfd.FileName;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cmbLayers.SelectedItem == null) { MessageBox.Show("请选择 DEM 图层。"); return; }
            var layer = cmbLayers.SelectedItem as RasterLayer;
            if (layer == null || !File.Exists(layer.FilePath)) { MessageBox.Show("输入文件不存在。"); return; }
            if (string.IsNullOrWhiteSpace(txtOutputPath.Text)) { MessageBox.Show("请选择输出路径。"); return; }

            try
            {
                Dataset dsIn = Gdal.Open(layer.FilePath, Access.GA_ReadOnly);
                int width = dsIn.RasterXSize;
                int height = dsIn.RasterYSize;
                double[] gt = new double[6];
                dsIn.GetGeoTransform(gt);

                float[] demData = new float[width * height];
                dsIn.GetRasterBand(1).ReadRaster(0, 0, width, height, demData, width, height, 0, 0);

                double noDataVal; int hasVal;
                dsIn.GetRasterBand(1).GetNoDataValue(out noDataVal, out hasVal);

                float[] outData = new float[width * height];

                var offsets3 = GetCircleOffsets(3);
                var offsets6 = GetCircleOffsets(6);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int idx = y * width + x;
                        float currentZ = demData[idx];

                        if (hasVal == 1 && Math.Abs(currentZ - noDataVal) < 1e-5) { outData[idx] = -9999; continue; }

                        float smallMin = currentZ, smallMax = currentZ;
                        foreach (var p in offsets3)
                        {
                            int nx = x + p.X, ny = y + p.Y;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            {
                                float v = demData[ny * width + nx];
                                if (hasVal == 1 && Math.Abs(v - noDataVal) < 1e-5) continue;
                                if (v < smallMin) smallMin = v;
                                if (v > smallMax) smallMax = v;
                            }
                        }

                        float largeMax = smallMax;
                        foreach (var p in offsets6)
                        {
                            int nx = x + p.X, ny = y + p.Y;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            {
                                float v = demData[ny * width + nx];
                                if (hasVal == 1 && Math.Abs(v - noDataVal) < 1e-5) continue;
                                if (v > largeMax) largeMax = v;
                            }
                        }

                        float denominator = (float)(largeMax - smallMin + 0.1);
                        if (denominator == 0) outData[idx] = 0;
                        else
                        {
                            float numerator = (float)(smallMax - smallMin + 0.1);
                            float result = 1.0f - (numerator / denominator);
                            outData[idx] = Math.Max(0, Math.Min(1, result));
                        }
                    }
                }

                var drv = Gdal.GetDriverByName("GTiff");
                Dataset dsOut = drv.Create(txtOutputPath.Text, width, height, 1, DataType.GDT_Float32, null);
                dsOut.SetGeoTransform(gt);
                dsOut.SetProjection(dsIn.GetProjection());
                dsOut.GetRasterBand(1).SetNoDataValue(-9999);
                dsOut.GetRasterBand(1).WriteRaster(0, 0, width, height, outData, width, height, 0, 0);

                dsOut.FlushCache();
                dsOut.Dispose();
                dsIn.Dispose();

                MessageBox.Show("察砂完成！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private System.Drawing.Point[] GetCircleOffsets(int radius)
        {
            var list = new System.Collections.Generic.List<System.Drawing.Point>();
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x * x + y * y <= radius * radius) list.Add(new System.Drawing.Point(x, y));
                }
            }
            return list.ToArray();
        }
    }
}
