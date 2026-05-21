using System;
using System.Drawing;

namespace FSGIS
{
    /// <summary>
    /// MapLayer：地图图层抽象基类
    /// 
    /// 设计目的：
    /// - 作为 RasterLayer / VectorLayer 的统一父类
    /// - 定义图层的基础属性（名称、路径、范围）
    /// - 约束绘制与查询接口
    /// </summary>
    public abstract class MapLayer : IDisposable
    {
        /// <summary>
        /// 图层显示名称
        /// </summary>
        public string LayerName { get; set; }

        /// <summary>
        /// 数据文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 图层空间范围（地图坐标）
        /// 由子类在构造时计算并赋值
        /// </summary>
        public RectangleF Extent { get; protected set; }

        /// <summary>
        /// 构造函数：初始化图层名称与路径
        /// </summary>
        protected MapLayer(string name, string path)
        {
            LayerName = name;
            FilePath = path;

            // 默认范围为空，具体由派生类计算
            Extent = RectangleF.Empty;
        }

        // =====================================================
        // 绘制接口（所有图层必须实现）
        // =====================================================
        /// <summary>
        /// 绘制图层内容
        /// </summary>
        /// <param name="g">GDI+ 绘图对象</param>
        /// <param name="mapControl">当前地图控件（提供变换与缩放信息）</param>
        public abstract void Draw(Graphics g, MapControl mapControl);

        // =====================================================
        // 像元 / 属性查询接口（可选实现）
        // =====================================================
        /// <summary>
        /// 根据地图坐标查询图层数值或属性
        /// - RasterLayer：返回像元值
        /// - VectorLayer：可扩展为要素属性查询
        /// </summary>
        /// <param name="geoPoint">地图坐标点</param>
        /// <returns>查询结果字符串，默认返回 null</returns>
        public virtual string GetValueAtGeoPoint(PointF geoPoint)
        {
            return null;
        }

        // =====================================================
        // 资源释放
        // =====================================================
        /// <summary>
        /// 释放图层相关资源
        /// 由子类按需重写
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
