using System;
using System.Drawing;
using System.Drawing.Imaging;
using OSGeo.GDAL;

namespace FSGIS
{
    /// <summary>
    /// RasterLayer：栅格图层
    /// 功能：
    /// - 打开 GDAL 栅格数据
    /// - 计算空间范围（Extent）
    /// - 对单波段数据进行灰度线性拉伸
    /// - 支持绘制与像元值查询
    /// </summary>
    public class RasterLayer : MapLayer
    {
        private Dataset _ds;     // GDAL 数据集
        private Band _band;      // 默认使用第 1 波段

        /// <summary>
        /// 栅格预览图（拉伸后的 Bitmap）
        /// </summary>
        public Bitmap Preview { get; private set; }

        // -----------------------------
        // 地理范围（地图坐标）
        // -----------------------------
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;

        // -----------------------------
        // 拉伸参数
        // -----------------------------
        private double _minVal;
        private double _maxVal;
        private double _noData;
        private bool _hasNoData;

        /// <summary>
        /// 构造函数：打开栅格并生成预览图
        /// </summary>
        public RasterLayer(string name, string path) : base(name, path)
        {
            try
            {
                // 注册 GDAL 驱动
                Gdal.AllRegister();

                // 打开栅格数据
                _ds = Gdal.Open(path, Access.GA_ReadOnly);
                if (_ds == null)
                    throw new Exception("Cannot open raster");

                // 获取第一个波段
                _band = _ds.GetRasterBand(1);

                // =============================
                // GeoTransform → 空间范围
                // =============================
                double[] gt = new double[6];
                _ds.GetGeoTransform(gt);

                int w = _ds.RasterXSize;
                int h = _ds.RasterYSize;

                double x1 = gt[0];
                double y1 = gt[3];
                double x2 = gt[0] + w * gt[1] + h * gt[2];
                double y2 = gt[3] + w * gt[4] + h * gt[5];

                MinX = (float)Math.Min(x1, x2);
                MaxX = (float)Math.Max(x1, x2);
                MinY = (float)Math.Min(y1, y2);
                MaxY = (float)Math.Max(y1, y2);

                // 设置图层范围
                this.Extent = new RectangleF(
                    MinX,
                    MinY,
                    MaxX - MinX,
                    MaxY - MinY
                );

                // =============================
                // NoData 读取
                // =============================
                int hasND;
                _band.GetNoDataValue(out _noData, out hasND);
                _hasNoData = hasND == 1;

                // =============================
                // 波段最小 / 最大值
                // =============================
                double[] minmax = new double[2];
                _band.ComputeRasterMinMax(minmax, 0);
                _minVal = minmax[0];
                _maxVal = minmax[1];

                // =============================
                // 构建拉伸后的位图
                // =============================
                Preview = BuildStretchBitmap();
            }
            catch
            {
                // 出现异常时给一个默认状态，避免程序崩溃
                Preview = null;
                this.Extent = new RectangleF(0, 0, 10, 10);
            }
        }

        // =====================================================
        // 灰度线性拉伸 → Bitmap
        // =====================================================
        private Bitmap BuildStretchBitmap()
        {
            int w = _ds.RasterXSize;
            int h = _ds.RasterYSize;

            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            // 一次性读取整幅栅格
            double[] buffer = new double[w * h];
            _band.ReadRaster(0, 0, w, h, buffer, w, h, 0, 0);

            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb
            );

            double range = _maxVal - _minVal;
            if (range <= 0) range = 1;

            int stride = data.Stride;
            byte[] pixelData = new byte[stride * h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    double val = buffer[y * w + x];
                    int offset = y * stride + x * 4;

                    // NoData → 完全透明
                    if (_hasNoData && Math.Abs(val - _noData) < 1e-8)
                    {
                        pixelData[offset + 0] = 0; // B
                        pixelData[offset + 1] = 0; // G
                        pixelData[offset + 2] = 0; // R
                        pixelData[offset + 3] = 0; // A
                        continue;
                    }

                    // 线性拉伸到 [0,1]
                    double t = (val - _minVal) / range;
                    t = Math.Max(0.0, Math.Min(1.0, t));

                    byte g = (byte)(t * 255);

                    pixelData[offset + 0] = g;   // B
                    pixelData[offset + 1] = g;   // G
                    pixelData[offset + 2] = g;   // R
                    pixelData[offset + 3] = 255; // A
                }
            }

            // 写回 Bitmap
            System.Runtime.InteropServices.Marshal.Copy(
                pixelData, 0, data.Scan0, pixelData.Length
            );

            bmp.UnlockBits(data);
            return bmp;
        }

        // =====================================================
        // 绘制栅格
        // =====================================================
        public override void Draw(Graphics g, MapControl mapControl)
        {
            if (Preview == null)
                return;

            // 通过负高度实现 Y 轴翻转（GIS 坐标系）
            g.DrawImage(
                Preview,
                MinX,
                MaxY,
                MaxX - MinX,
                MinY - MaxY
            );
        }

        // =====================================================
        // 像元值查询（地图坐标 → 行列号）
        // =====================================================
        public override string GetValueAtGeoPoint(PointF geoPoint)
        {
            // 超出范围直接返回
            if (geoPoint.X < MinX || geoPoint.X > MaxX ||
                geoPoint.Y < MinY || geoPoint.Y > MaxY)
                return null;

            double[] gt = new double[6];
            _ds.GetGeoTransform(gt);

            int col = (int)((geoPoint.X - gt[0]) / gt[1]);
            int row = (int)((geoPoint.Y - gt[3]) / gt[5]);

            if (col < 0 || row < 0 ||
                col >= _ds.RasterXSize ||
                row >= _ds.RasterYSize)
                return null;

            double[] val = new double[1];
            _band.ReadRaster(col, row, 1, 1, val, 1, 1, 0, 0);

            if (_hasNoData && Math.Abs(val[0] - _noData) < 1e-8)
                return $"{LayerName}: NoData";

            return $"{LayerName}: {val[0]:F2}";
        }

        // =====================================================
        // 资源释放
        // =====================================================
        public override void Dispose()
        {
            if (Preview != null) Preview.Dispose();
            if (_band != null) _band.Dispose();
            if (_ds != null) _ds.Dispose();

            base.Dispose();
        }
    }
}
