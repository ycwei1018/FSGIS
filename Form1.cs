using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSGIS
{
    /// <summary>
    /// 主窗体：FSGIS 基础版本
    /// 负责界面初始化、图层管理、工具调用与地图交互
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 地图控件（核心绘制与交互对象）
        /// </summary>
        private MapControl map;

        /// <summary>
        /// 构造函数：初始化界面与地图环境
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            #region MapControl 初始化

            map = new MapControl
            {
                Dock = DockStyle.Fill
            };
            panelMap.Controls.Add(map);

            #endregion

            #region 地图鼠标移动事件（显示坐标）

            // 监听地图鼠标移动事件，实时更新右下角坐标显示
            map.OnMapMouseMove += (s, point) =>
            {
                lblCoordinate.Text = $"Lon: {point.X:F6},  Lat: {point.Y:F6}";
            };

            #endregion

            #region TreeView 右键选中支持

            // 右键点击 TreeView 时自动选中节点
            treeLayers.MouseDown += treeLayers_MouseDown;

            #endregion

            #region 普通工具箱初始化

            listTools.Items.Add("全图显示"); // Zoom to Extent
            listTools.Items.Add("放大");       // Zoom In
            listTools.Items.Add("缩小");       // Zoom Out

            #endregion

            #region 默认视图设置（北京）

            // 北京中心点（WGS84）
            map.CenterGeo = new PointF(116.4074f, 39.9042f);
            map.Zoom = 200.0f;

            #endregion
        }

        // =====================================================
        // TreeView：右键即选中节点
        // =====================================================
        private void treeLayers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeLayers.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    treeLayers.SelectedNode = node;
                }
            }
        }

        // =====================================================
        // 图层右键菜单：加载属性表
        // =====================================================
        private void itemAttributeTable_Click(object sender, EventArgs e)
        {
            TreeNode node = treeLayers.SelectedNode;
            if (node == null) return;

            // 仅矢量图层支持属性表
            if (node.Tag is VectorLayer vectorLayer)
            {
                FormAttributeTable tableForm = new FormAttributeTable(vectorLayer);
                tableForm.Show();
            }
            else
            {
                MessageBox.Show("只有矢量图层包含属性表。");
            }
        }

        // =====================================================
        // 栅格值显示开关
        // =====================================================
        private void chkShowRasterValue_CheckedChanged(object sender, EventArgs e)
        {
            map.ShowRasterValue = chkShowRasterValue.Checked;
            map.Invalidate();
        }

        // =====================================================
        // 普通工具箱操作
        // =====================================================
        private void listTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            string toolName = listTools.SelectedItem?.ToString();

            switch (toolName)
            {
                case "全图显示":
                    map.ZoomToExtent();
                    break;

                case "放大":
                    map.Zoom *= 1.5f;
                    map.Invalidate();
                    break;

                case "缩小":
                    map.Zoom *= 0.67f;
                    map.Invalidate();
                    break;
            }
        }

        // =====================================================
        // 风水工具箱入口
        // =====================================================
        private void liXiangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map.Layers.Count == 0)
            {
                MessageBox.Show("请先加载 DEM 栅格图层。");
                return;
            }

            FormLiXiang tool = new FormLiXiang(map);
            tool.ShowDialog();
        }

        private void guanShuiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map.Layers.Count < 2)
            {
                MessageBox.Show("请确保至少加载了一个水系矢量图层和一个参考栅格图层。");
                return;
            }

            FormGuanShui tool = new FormGuanShui(map);
            tool.ShowDialog();
        }

        private void chaShaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map.Layers.Count == 0)
            {
                MessageBox.Show("请先加载 DEM 数据。");
                return;
            }

            FormChaSha tool = new FormChaSha(map);
            tool.ShowDialog();
        }

        private void dianXueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map.Layers.Count == 0)
            {
                MessageBox.Show("请先加载 DEM 数据。");
                return;
            }

            FormDianXue tool = new FormDianXue(map);
            tool.ShowDialog();
        }

        private void xunLongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map.Layers.Count == 0)
            {
                MessageBox.Show("请先加载 DEM 数据。");
                return;
            }

            FormXunLong tool = new FormXunLong(map);
            tool.ShowDialog();
        }

        private void maskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map.Layers.Count < 2)
            {
                MessageBox.Show("请确保至少加载了一个栅格图层和一个矢量面图层。");
                return;
            }

            FormMask tool = new FormMask(map);
            tool.ShowDialog();
        }

        // =====================================================
        // 添加图层
        // =====================================================
        private void addLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "所有支持的图层|*.tif;*.png;*.jpg;*.shp|栅格|*.tif;*.png;*.jpg|矢量 Shapefile|*.shp"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string path = dlg.FileName;
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            MapLayer layer;

            if (path.ToLower().EndsWith(".shp"))
                layer = new VectorLayer(name, path);
            else
                layer = new RasterLayer(name, path);

            map.Layers.Add(layer);
            map.Invalidate();

            listLayers.Items.Add(name);

            TreeNode node = new TreeNode(name)
            {
                Tag = layer
            };
            treeLayers.Nodes.Add(node);
        }

        // =====================================================
        // TreeView 拖拽排序（同步地图绘制顺序）
        // =====================================================
        private void treeLayers_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeLayers_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
        }

        private void treeLayers_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            Point pt = treeLayers.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = treeLayers.GetNodeAt(pt);

            if (draggedNode == null || targetNode == null || draggedNode == targetNode)
                return;

            draggedNode.Remove();
            treeLayers.Nodes.Insert(targetNode.Index, draggedNode);

            map.Layers.Clear();

            // TreeView 从下往上 → 地图从底到顶
            for (int i = treeLayers.Nodes.Count - 1; i >= 0; i--)
            {
                if (treeLayers.Nodes[i].Tag is MapLayer layer)
                {
                    map.Layers.Add(layer);
                }
            }

            map.Invalidate();
        }

        // =====================================================
        // 移除图层
        // =====================================================
        private void removeLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeLayers.SelectedNode == null)
            {
                MessageBox.Show("请先选择要移除的图层。", "提示");
                return;
            }

            if (treeLayers.SelectedNode.Tag is MapLayer layerToRemove)
            {
                map.Layers.Remove(layerToRemove);
                treeLayers.Nodes.Remove(treeLayers.SelectedNode);
                map.Invalidate();

                for (int i = listLayers.Items.Count - 1; i >= 0; i--)
                {
                    if (listLayers.Items[i].ToString() == layerToRemove.LayerName)
                    {
                        listLayers.Items.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("选中的不是有效图层。", "错误");
            }
        }
    }
}
