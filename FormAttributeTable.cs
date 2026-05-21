using System;
using System.Data;
using System.Windows.Forms;
using OSGeo.OGR;
using OSGeo.GDAL;

namespace FSGIS
{
    /// <summary>
    /// 矢量图层属性表窗口
    /// 用于读取并显示 Shapefile 的属性信息
    /// </summary>
    public partial class FormAttributeTable : Form
    {
        /// <summary>
        /// 当前显示属性表的矢量图层
        /// </summary>
        private VectorLayer _layer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="layer">要显示属性表的 VectorLayer</param>
        public FormAttributeTable(VectorLayer layer)
        {
            InitializeComponent();

            _layer = layer;
            this.Text = "属性表 - " + _layer.LayerName;

            // 绑定窗体加载事件
            this.Load += FormAttributeTable_Load;
        }

        /// <summary>
        /// 窗体加载时触发
        /// </summary>
        private void FormAttributeTable_Load(object sender, EventArgs e)
        {
            LoadAttributes();
        }

        /// <summary>
        /// 读取矢量图层属性并加载到 DataGridView
        /// </summary>
        private void LoadAttributes()
        {
            // ==========================================
            //  GDAL 自动处理编码
            // ==========================================
            Gdal.SetConfigOption("SHAPE_ENCODING", null);

            // 打开矢量数据源（只读）
            DataSource ds = Ogr.Open(_layer.FilePath, 0);
            if (ds == null) return;

            // 默认读取第 0 个图层
            Layer ogrLayer = ds.GetLayerByIndex(0);

            // 创建内存表，用于绑定到 DataGridView
            DataTable dt = new DataTable();

            FeatureDefn defn = ogrLayer.GetLayerDefn();
            int fieldCount = defn.GetFieldCount();

            // ==========================================
            // 1. 创建表结构（字段）
            // ==========================================

            // FID 作为第一列
            dt.Columns.Add("FID", typeof(int));

            // 创建属性字段列
            for (int i = 0; i < fieldCount; i++)
            {
                FieldDefn fieldDefn = defn.GetFieldDefn(i);

                // 统一使用 string 类型，兼容所有字段类型
                dt.Columns.Add(fieldDefn.GetName(), typeof(string));
            }

            // ==========================================
            // 2. 读取要素数据
            // ==========================================
            Feature feat;
            ogrLayer.ResetReading();

            while ((feat = ogrLayer.GetNextFeature()) != null)
            {
                DataRow row = dt.NewRow();

                // 写入 FID
                row["FID"] = feat.GetFID();

                // 写入属性字段
                for (int i = 0; i < fieldCount; i++)
                {
                    string fieldName = defn.GetFieldDefn(i).GetName();

                    // GetFieldAsString 会自动根据 SHAPE_ENCODING 转码
                    row[fieldName] = feat.GetFieldAsString(i);
                }

                dt.Rows.Add(row);

                // 释放要素对象
                feat.Dispose();
            }

            // ==========================================
            // 3. 绑定到 DataGridView
            // ==========================================
            dgvAttributes.DataSource = dt;

            // 释放数据源
            ds.Dispose();
        }
    }
}
