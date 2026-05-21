using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSGIS
{
    /// <summary>
    /// VectorLayer：矢量图层类
    /// - 支持 Point / LineString / Polygon
    /// - 加载时一次性缓存几何点坐标，提高绘制性能
    /// </summary>
    public class VectorLayer : MapLayer
    {
        private DataSource _dataSource;       // OGR 数据源
        private Layer _ogrLayer;              // 第一图层
        private List<PointF[]> _cachedGeometries = new List<PointF[]>(); // 缓存几何点数组
        private wkbGeometryType _geomType;    // 图层几何类型（点/线/面）

        /// <summary>
        /// 构造函数：打开矢量文件并缓存几何体
        /// </summary>
        public VectorLayer(string name, string path) : base(name, path)
        {
            // 为避免中文乱码，设置 shapefile 编码 (GDAL 3.x 必须)
            Gdal.SetConfigOption("SHAPE_ENCODING", "");

            // 打开矢量文件（只读模式）
            _dataSource = Ogr.Open(path, 0);
            if (_dataSource == null)
                throw new Exception("无法打开矢量文件");

            // 读取第一图层及几何类型
            _ogrLayer = _dataSource.GetLayerByIndex(0);
            _geomType = _ogrLayer.GetGeomType();

            // 获取图层范围（Envelope -> RectangleF）
            Envelope env = new Envelope();
            _ogrLayer.GetExtent(env, 1);
            this.Extent = new RectangleF(
                (float)env.MinX, (float)env.MinY,
                (float)(env.MaxX - env.MinX),
                (float)(env.MaxY - env.MinY)
            );

            // 加载全部要素并缓存几何数据
            LoadFeatures();
        }

        /// <summary>
        /// 将矢量数据的几何点坐标转换为 PointF 并缓存
        /// （简化：Polygon 仅缓存外环，不处理多重多边形/孔洞）
        /// </summary>
        private void LoadFeatures()
        {
            _ogrLayer.ResetReading();
            Feature feat;

            while ((feat = _ogrLayer.GetNextFeature()) != null)
            {
                Geometry geom = feat.GetGeometryRef();
                if (geom != null)
                {
                    // 处理 Polygon 与 LineString（均取外环或主线）
                    if (geom.GetGeometryType() == wkbGeometryType.wkbPolygon ||
                        geom.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        Geometry ring =
                            (geom.GetGeometryType() == wkbGeometryType.wkbPolygon)
                            ? geom.GetGeometryRef(0) : geom;

                        int count = ring.GetPointCount();
                        PointF[] pts = new PointF[count];

                        for (int i = 0; i < count; i++)
                        {
                            pts[i] = new PointF(
                                (float)ring.GetX(i),
                                (float)ring.GetY(i)
                            );
                        }

                        _cachedGeometries.Add(pts);
                    }
                    // 处理 Point
                    else if (geom.GetGeometryType() == wkbGeometryType.wkbPoint)
                    {
                        _cachedGeometries.Add(new PointF[]
                        {
                            new PointF((float)geom.GetX(0), (float)geom.GetY(0))
                        });
                    }
                }

                feat.Dispose();
            }
        }

        /// <summary>
        /// 绘制矢量图层
        /// - 根据 Zoom 调整线宽与点大小，让显示更稳定
        /// </summary>
        public override void Draw(Graphics g, MapControl mapControl)
        {
            // 使用屏幕固定像素宽度 (2px)，按地图比例换算为地图线宽
            float penWidth = 2.0f / mapControl.Zoom;

            using (Pen pen = new Pen(Color.Red, penWidth))
            using (Brush brush = new SolidBrush(Color.FromArgb(100, 255, 0, 0))) // 半透明填充
            {
                foreach (var pts in _cachedGeometries)
                {
                    // 绘制点
                    if (_geomType == wkbGeometryType.wkbPoint)
                    {
                        float size = 5.0f / mapControl.Zoom;
                        float x = pts[0].X - size / 2;
                        float y = pts[0].Y - size / 2;

                        g.FillEllipse(Brushes.Red, x, y, size, size);
                    }
                    // 绘制线
                    else if (_geomType == wkbGeometryType.wkbLineString)
                    {
                        if (pts.Length > 1)
                            g.DrawLines(pen, pts);
                    }
                    // 绘制多边形（先填充再描边）
                    else if (_geomType == wkbGeometryType.wkbPolygon)
                    {
                        if (pts.Length > 2)
                        {
                            g.FillPolygon(brush, pts);
                            g.DrawPolygon(pen, pts);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 释放 OGR 数据源
        /// </summary>
        public override void Dispose()
        {
            if (_dataSource != null)
                _dataSource.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// 矢量图层暂不实现点查询功能，返回 null
        /// </summary>
        public override string GetValueAtGeoPoint(PointF geoPoint)
        {
            return null;
        }
    }
}
