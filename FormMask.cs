using System;
using System.Text;
using System.Windows.Forms;
using OSGeo.GDAL;
using System.Runtime.InteropServices; // 用于调用 Windows API

namespace FSGIS
{
    public partial class FormMask : Form
    {
        private MapControl _map;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)] string path,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
            int shortPathLength);

        public FormMask(MapControl map)
        {
            InitializeComponent();
            _map = map;
            LoadLayers();
        }

        private void LoadLayers()
        {
            foreach (var layer in _map.Layers)
            {
                if (layer is RasterLayer) cmbRaster.Items.Add(layer);
                else if (layer is VectorLayer) cmbVector.Items.Add(layer);
            }
            cmbRaster.DisplayMember = "LayerName";
            cmbVector.DisplayMember = "LayerName";
            if (cmbRaster.Items.Count > 0) cmbRaster.SelectedIndex = 0;
            if (cmbVector.Items.Count > 0) cmbVector.SelectedIndex = 0;
        }

        // 浏览按钮事件
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "GeoTIFF|*.tif";
            sfd.FileName = "Mask_Result.tif"; // 默认文件名
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtOut.Text = sfd.FileName;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cmbRaster.SelectedItem == null || cmbVector.SelectedItem == null) return;

            var rLayer = cmbRaster.SelectedItem as RasterLayer;
            var vLayer = cmbVector.SelectedItem as VectorLayer;

            try
            {
                // 获取短路径 (Short Path) 以避开中文问题
                string inputPath = ToShortPath(rLayer.FilePath);
                string cutlinePath = ToShortPath(vLayer.FilePath);
                string outputPath = txtOut.Text; // 输出路径如果是新的，可能无法获取短路径，暂时保持原样，GDAL Create通常对中文支持稍好

                // 强制 GDAL 配置
                Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
                Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");

                string[] options = new string[]
                {
                    "-cutline", cutlinePath,
                    "-crop_to_cutline",
                    "-dstnodata", "-9999"
                };

                // 打开数据
                Dataset dsIn = Gdal.Open(inputPath, Access.GA_ReadOnly);
                if (dsIn == null) throw new Exception("无法打开输入栅格，请检查路径。");

                Dataset[] srcDS = new Dataset[] { dsIn };  

                // 执行 Warp
                Gdal.Warp(
                    outputPath, 
                    srcDS, 
                    new GDALWarpAppOptions(options),
                    null,
                    null);

                dsIn.Dispose();

                MessageBox.Show("掩膜裁剪完成！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
            }
        }

        // 辅助方法：转换为短路径
        private string ToShortPath(string longPath)
        {
            StringBuilder shortPath = new StringBuilder(255);
            GetShortPathName(longPath, shortPath, shortPath.Capacity);
            string result = shortPath.ToString();
            // 如果转换失败（比如文件不存在），返回原路径
            return string.IsNullOrEmpty(result) ? longPath : result;
        }
    }
}