using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FSGIS
{
    public partial class FormDianXue : Form
    {
        private MapControl _map;

        public FormDianXue(MapControl map)
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
            using (var sfd = new SaveFileDialog { Filter = "GeoTIFF|*.tif", FileName = "Xue.tif" })
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
                Band inBand = dsIn.GetRasterBand(1);
                inBand.ReadRaster(0, 0, width, height, demData, width, height, 0, 0);

                double noDataVal; int hasVal;
                inBand.GetNoDataValue(out noDataVal, out hasVal);

                float[] outData = new float[width * height];

                var offsetsInner = GetCircleOffsets(0, 3);
                var offsetsRing = GetCircleOffsets(3, 6);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int idx = y * width + x;
                        float centerZ = demData[idx];

                        if (hasVal == 1 && Math.Abs(centerZ - noDataVal) < 1e-5) { outData[idx] = -9999; continue; }

                        double sumInner = 0; int countInner = 0;
                        foreach (var p in offsetsInner)
                        {
                            int nx = x + p.X, ny = y + p.Y;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            {
                                float v = demData[ny * width + nx];
                                if (hasVal == 1 && Math.Abs(v - noDataVal) < 1e-5) continue;
                                sumInner += v; countInner++;
                            }
                        }

                        if (countInner == 0) { outData[idx] = 0; continue; }
                        double innerMean = sumInner / countInner;

                        double threshold = innerMean + 5.0;

                        int countHigher = 0, countRingTotal = 0;
                        foreach (var p in offsetsRing)
                        {
                            int nx = x + p.X, ny = y + p.Y;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            {
                                float v = demData[ny * width + nx];
                                if (hasVal == 1 && Math.Abs(v - noDataVal) < 1e-5) continue;
                                countRingTotal++;
                                if (v >= threshold) countHigher++;
                            }
                        }

                        outData[idx] = (countRingTotal == 0) ? 0 : (float)countHigher / countRingTotal;
                    }
                }

                var drv = Gdal.GetDriverByName("GTiff");
                Dataset dsOut = drv.Create(txtOutputPath.Text, width, height, 1, DataType.GDT_Float32, null);
                dsOut.SetGeoTransform(gt);
                dsOut.SetProjection(dsIn.GetProjection());
                var outBand = dsOut.GetRasterBand(1);
                outBand.SetNoDataValue(-9999);
                outBand.WriteRaster(0, 0, width, height, outData, width, height, 0, 0);

                dsOut.FlushCache();
                dsOut.Dispose();
                dsIn.Dispose();

                MessageBox.Show("点穴完成！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private System.Drawing.Point[] GetCircleOffsets(int minRadius, int maxRadius)
        {
            var list = new List<System.Drawing.Point>();
            for (int y = -maxRadius; y <= maxRadius; y++)
            {
                for (int x = -maxRadius; x <= maxRadius; x++)
                {
                    int distSq = x * x + y * y;
                    if (distSq > minRadius * minRadius && distSq <= maxRadius * maxRadius) list.Add(new System.Drawing.Point(x, y));
                    else if (minRadius == 0 && distSq <= maxRadius * maxRadius) list.Add(new System.Drawing.Point(x, y));
                }
            }
            return list.ToArray();
        }
    }
}