using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSGIS
{
    /// <summary>
    /// 地图控件：
    /// - 负责地图绘制
    /// - 管理缩放、平移
    /// - 进行屏幕坐标与地理坐标转换
    /// - 提供栅格值悬浮查询
    /// </summary>
    public class MapControl : Control
    {
        #region 公共属性与字段

        /// <summary>
        /// 是否显示栅格值悬浮窗
        /// </summary>
        public bool ShowRasterValue { get; set; } = false;

        /// <summary>
        /// 当前鼠标位置对应的栅格查询结果缓存
        /// </summary>
        private List<string> _rasterValues = new List<string>();

        /// <summary>
        /// 地图图层集合（绘制顺序由 List 顺序决定）
        /// </summary>
        public List<MapLayer> Layers = new List<MapLayer>();

        /// <summary>
        /// 缩放比例：
        /// 屏幕像素 / 地理单位
        /// </summary>
        public float Zoom = 1.0f;

        /// <summary>
        /// 当前视图中心点的地理坐标
        /// </summary>
        public PointF CenterGeo = new PointF(0, 0);

        #endregion

        #region 鼠标交互状态

        private bool _isDragging = false;
        private Point _lastMousePos;

        /// <summary>
        /// 地图鼠标移动事件（对外暴露地理坐标）
        /// </summary>
        public event EventHandler<PointF> OnMapMouseMove;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MapControl()
        {
            DoubleBuffered = true;
            BackColor = Color.WhiteSmoke;

            // 注册鼠标滚轮缩放
            MouseWheel += MapControl_MouseWheel;
        }

        // =====================================================
        // 1. 全图显示（Zoom To Extent）
        // =====================================================
        public void ZoomToExtent()
        {
            RectangleF extent = GetFullExtent();
            if (extent.IsEmpty) return;

            // 地理范围中心作为视图中心
            CenterGeo = new PointF(
                extent.X + extent.Width / 2.0f,
                extent.Y + extent.Height / 2.0f
            );

            // 计算缩放比例（留 5% 边距）
            float scaleX = Width / extent.Width;
            float scaleY = Height / extent.Height;
            Zoom = Math.Min(scaleX, scaleY) * 0.95f;

            Invalidate();
        }

        // =====================================================
        // 2. 鼠标滚轮缩放（以鼠标为中心）
        // =====================================================
        private void MapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            // 当前鼠标对应的地理坐标
            PointF mouseGeo = ScreenToGeo(e.Location);

            // 调整缩放比例
            if (e.Delta > 0)
                Zoom *= 1.2f;
            else
                Zoom /= 1.2f;

            // 缩放后重新计算鼠标位置的屏幕坐标
            PointF newMouseScreen = GeoToScreen(mouseGeo);

            // 屏幕偏移量
            float dx = newMouseScreen.X - e.X;
            float dy = newMouseScreen.Y - e.Y;

            // 将屏幕偏移量反算为地理偏移量，修正中心点
            CenterGeo.X += dx / Zoom;
            CenterGeo.Y -= dy / Zoom; // 注意 Y 轴方向相反

            Invalidate();
        }

        // =====================================================
        // 3. 绘制（核心坐标变换）
        // =====================================================
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.WhiteSmoke);
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            #region 核心坐标变换矩阵

            // 1. 移动原点到屏幕中心
            e.Graphics.TranslateTransform(Width / 2.0f, Height / 2.0f);

            // 2. 翻转 Y 轴，使 Y 向上为正
            e.Graphics.ScaleTransform(1, -1);

            // 3. 应用缩放
            e.Graphics.ScaleTransform(Zoom, Zoom);

            // 4. 将地理中心点移动到原点
            e.Graphics.TranslateTransform(-CenterGeo.X, -CenterGeo.Y);

            #endregion

            // 绘制所有图层
            foreach (var layer in Layers)
            {
                layer.Draw(e.Graphics, this);
            }

            // =================================================
            // 绘制屏幕 UI（恢复默认坐标系）
            // =================================================
            e.Graphics.ResetTransform();
            using (Font f = new Font("Arial", 10))
            {
                e.Graphics.DrawString(
                    $"Zoom: {Zoom:F1}\nCenter: {CenterGeo.X:F2}, {CenterGeo.Y:F2}",
                    f,
                    Brushes.Black,
                    5,
                    5
                );
            }

            // =================================================
            // 栅格值悬浮窗
            // =================================================
            if (ShowRasterValue && _rasterValues.Count > 0)
            {
                Point mousePt = PointToClient(Cursor.Position);

                int padding = 5;
                int lineHeight = 20;
                int boxWidth = 200;
                int boxHeight = _rasterValues.Count * lineHeight + padding * 2;

                int startX = mousePt.X + 15;
                int startY = mousePt.Y;

                using (Font fontVal = new Font("Microsoft YaHei", 9))
                {
                    int maxTextW = 0;
                    foreach (var str in _rasterValues)
                    {
                        var size = e.Graphics.MeasureString(str, fontVal);
                        if (size.Width > maxTextW)
                            maxTextW = (int)size.Width;
                    }

                    boxWidth = maxTextW + padding * 2;

                    Rectangle rect = new Rectangle(startX, startY, boxWidth, boxHeight);

                    using (Brush bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                    {
                        e.Graphics.FillRectangle(bgBrush, rect);
                    }
                    e.Graphics.DrawRectangle(Pens.White, rect);

                    for (int i = 0; i < _rasterValues.Count; i++)
                    {
                        e.Graphics.DrawString(
                            _rasterValues[i],
                            fontVal,
                            Brushes.White,
                            startX + padding,
                            startY + padding + i * lineHeight
                        );
                    }
                }
            }
        }

        // =====================================================
        // 坐标转换工具
        // =====================================================
        public PointF ScreenToGeo(Point screenPt)
        {
            float cx = Width / 2.0f;
            float cy = Height / 2.0f;

            float dx = screenPt.X - cx;
            float dy = screenPt.Y - cy;

            float geoX = CenterGeo.X + dx / Zoom;
            float geoY = CenterGeo.Y - dy / Zoom;

            return new PointF(geoX, geoY);
        }

        public PointF GeoToScreen(PointF geoPt)
        {
            float cx = Width / 2.0f;
            float cy = Height / 2.0f;

            float x = (geoPt.X - CenterGeo.X) * Zoom + cx;
            float y = cy - (geoPt.Y - CenterGeo.Y) * Zoom;

            return new PointF(x, y);
        }

        // =====================================================
        // 地图范围计算
        // =====================================================
        public RectangleF GetFullExtent()
        {
            if (Layers.Count == 0) return RectangleF.Empty;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            bool hasLayer = false;

            foreach (var layer in Layers)
            {
                if (layer.Extent.IsEmpty) continue;

                hasLayer = true;
                minX = Math.Min(minX, layer.Extent.Left);
                minY = Math.Min(minY, layer.Extent.Top);
                maxX = Math.Max(maxX, layer.Extent.Right);
                maxY = Math.Max(maxY, layer.Extent.Bottom);
            }

            if (!hasLayer) return RectangleF.Empty;

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        // =====================================================
        // 鼠标拖拽平移
        // =====================================================
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _lastMousePos = e.Location;
                Cursor = Cursors.Hand;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                float dx = e.X - _lastMousePos.X;
                float dy = e.Y - _lastMousePos.Y;

                CenterGeo.X -= dx / Zoom;
                CenterGeo.Y += dy / Zoom;

                _lastMousePos = e.Location;
                Invalidate();
            }

            // 对外抛出地理坐标
            PointF geo = ScreenToGeo(e.Location);
            OnMapMouseMove?.Invoke(this, geo);

            // 栅格值查询
            if (ShowRasterValue)
            {
                _rasterValues.Clear();
                foreach (var layer in Layers)
                {
                    string val = layer.GetValueAtGeoPoint(geo);
                    if (!string.IsNullOrEmpty(val))
                        _rasterValues.Add(val);
                }
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }
    }
}
