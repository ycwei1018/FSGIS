using System;
using System.IO;
using System.Windows.Forms;
using OSGeo.GDAL;
using OSGeo.OGR;

namespace FSGIS
{
    public partial class FormGuanShui : Form
    {
        private MapControl _map;

        public FormGuanShui(MapControl map)
        {
            InitializeComponent();
            _map = map;
            LoadLayers();
        }

        /// <summary>
        /// 加载当前地图中的矢量 / 栅格图层
        /// </summary>
        private void LoadLayers()
        {
            cmbVectorLayers.Items.Clear();
            cmbRasterLayers.Items.Clear();

            foreach (var layer in _map.Layers)
            {
                if (layer is VectorLayer)
                    cmbVectorLayers.Items.Add(layer);
                else if (layer is RasterLayer)
                    cmbRasterLayers.Items.Add(layer);
            }

            cmbVectorLayers.DisplayMember = "LayerName";
            cmbRasterLayers.DisplayMember = "LayerName";

            if (cmbVectorLayers.Items.Count > 0)
                cmbVectorLayers.SelectedIndex = 0;

            if (cmbRasterLayers.Items.Count > 0)
                cmbRasterLayers.SelectedIndex = 0;
        }

        /// <summary>
        /// 选择输出 GeoTIFF 路径
        /// </summary>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog
            {
                Filter = "GeoTIFF|*.tif",
                FileName = "Shui.tif"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = sfd.FileName;
                }
            }
        }

        /// <summary>
        /// 执行观水分析主流程
        /// </summary>
        private void btnRun_Click(object sender, EventArgs e)
        {
            // ---------- 参数检查 ----------
            if (cmbVectorLayers.SelectedItem == null ||
                cmbRasterLayers.SelectedItem == null)
            {
                MessageBox.Show("请选择水系矢量和参考栅格图层。");
                return;
            }

            var vecLayer = cmbVectorLayers.SelectedItem as VectorLayer;
            var refLayer = cmbRasterLayers.SelectedItem as RasterLayer;

            if (vecLayer == null ||
                refLayer == null ||
                !File.Exists(vecLayer.FilePath) ||
                !File.Exists(refLayer.FilePath))
            {
                MessageBox.Show("选中的图层文件不存在。");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOutputPath.Text))
            {
                MessageBox.Show("请选择输出路径。");
                return;
            }

            try
            {
                // ---------- GDAL 环境配置 ----------
                Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
                Gdal.SetConfigOption("SHAPE_ENCODING", "");

                // ---------- 打开参考栅格 ----------
                Dataset dsRef = Gdal.Open(refLayer.FilePath, Access.GA_ReadOnly);
                int width = dsRef.RasterXSize;
                int height = dsRef.RasterYSize;

                double[] gt = new double[6];
                dsRef.GetGeoTransform(gt);

                string proj = dsRef.GetProjection();

                // ---------- 创建水系掩膜栅格 ----------
                var memDriver = Gdal.GetDriverByName("MEM");
                var dsMask = memDriver.Create(
                    "",
                    width,
                    height,
                    1,
                    DataType.GDT_Byte,
                    null
                );

                dsMask.SetGeoTransform(gt);
                dsMask.SetProjection(proj);

                var maskBand = dsMask.GetRasterBand(1);
                maskBand.Fill(0, 0);

                // ---------- 打开水系矢量 ----------
                var vecDS = Ogr.Open(vecLayer.FilePath, 0);
                var ogrLayer = vecDS.GetLayerByIndex(0);

                int[] bandList = { 1 };
                double[] burnValues = { 1.0 };

                // 栅格化水系（ALL_TOUCHED）
                Gdal.RasterizeLayer(
                    dsMask,
                    1,
                    bandList,
                    ogrLayer,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    1,
                    burnValues,
                    new string[] { "ALL_TOUCHED=TRUE" },
                    null,
                    null
                );

                // ---------- 距离栅格 ----------
                var dsDist = memDriver.Create(
                    "",
                    width,
                    height,
                    1,
                    DataType.GDT_Float32,
                    null
                );

                dsDist.SetGeoTransform(gt);
                dsDist.SetProjection(proj);

                var distBand = dsDist.GetRasterBand(1);

                string[] proxOptions =
                {
                    "VALUES=1",
                    "DIST_UNITS=PIXEL"
                };

                Gdal.ComputeProximity(
                    maskBand,
                    distBand,
                    proxOptions,
                    null,
                    null
                );

                // ---------- 读取距离数据 ----------
                float[] distData = new float[width * height];
                distBand.ReadRaster(
                    0, 0,
                    width, height,
                    distData,
                    width, height,
                    0, 0
                );

                // ---------- 读取参考栅格 ----------
                float[] refData = new float[width * height];
                var refBand = dsRef.GetRasterBand(1);

                refBand.ReadRaster(
                    0, 0,
                    width, height,
                    refData,
                    width, height,
                    0, 0
                );

                double noDataVal;
                int hasVal;
                refBand.GetNoDataValue(out noDataVal, out hasVal);

                // ---------- 观水评价 ----------
                float[] outData = new float[width * height];
                double pixelSize = 30.0;

                for (int i = 0; i < distData.Length; i++)
                {
                    if (hasVal == 1 &&
                        Math.Abs(refData[i] - noDataVal) < 1e-5)
                    {
                        outData[i] = -9999;
                        continue;
                    }

                    float distMeters = distData[i] * (float)pixelSize;

                    if (distMeters < 50)
                        outData[i] = 0.25f;
                    else if (distMeters < 200)
                        outData[i] = 0.85f;
                    else if (distMeters < 500)
                        outData[i] = 1.0f;
                    else if (distMeters < 1000)
                        outData[i] = 0.65f;
                    else if (distMeters < 2000)
                        outData[i] = 0.50f;
                    else
                        outData[i] = 0.15f;
                }

                // ---------- 输出 GeoTIFF ----------
                var tifDriver = Gdal.GetDriverByName("GTiff");
                var dsOut = tifDriver.Create(
                    txtOutputPath.Text,
                    width,
                    height,
                    1,
                    DataType.GDT_Float32,
                    null
                );

                dsOut.SetGeoTransform(gt);
                dsOut.SetProjection(proj);

                var outBand = dsOut.GetRasterBand(1);
                outBand.SetNoDataValue(-9999);

                outBand.WriteRaster(
                    0, 0,
                    width, height,
                    outData,
                    width, height,
                    0, 0
                );

                dsOut.FlushCache();

                // ---------- 资源释放 ----------
                dsOut.Dispose();
                dsDist.Dispose();
                dsMask.Dispose();
                vecDS.Dispose();
                dsRef.Dispose();

                MessageBox.Show("观水完成！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }
    }
}
