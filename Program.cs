using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Windows.Forms;

namespace FSGIS
{
    /// <summary>
    /// Program：应用程序入口类
    /// 
    /// 职责：
    /// - 初始化 GDAL / OGR 运行环境
    /// - 启动 WinForms 应用程序
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 应用程序主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 初始化 GDAL / OGR（驱动注册、环境变量配置等）
            GdalConfiguration.ConfigureGdal();

            // 启用 Windows 视觉样式
            Application.EnableVisualStyles();

            // 使用 GDI 文本渲染（兼容旧 WinForms 控件）
            Application.SetCompatibleTextRenderingDefault(false);

            // 启动主窗体
            Application.Run(new Form1());
        }
    }
}
