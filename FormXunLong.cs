using OSGeo.GDAL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSGIS
{
    public partial class FormXunLong : Form
    {
        private MapControl _map;

        public FormXunLong(MapControl map)
        {
            _map = map;
            InitializeComponent();
            LoadLayers();

            // 不覆盖 Designer 的默认值，只有当文本框为空时才尝试自动填充
            if (string.IsNullOrWhiteSpace(txtPythonExe.Text))
            {
                string found = FindPythonExecutable();
                if (!string.IsNullOrWhiteSpace(found))
                    txtPythonExe.Text = found;
            }
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

        // 在 PATH 中查找 python.exe 或常见安装目录
        private string FindPythonExecutable()
        {
            // 1) 在 PATH 中查找
            var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            var paths = pathEnv.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in paths)
            {
                try
                {
                    var candidate = Path.Combine(p.Trim(), "python.exe");
                    if (File.Exists(candidate)) return candidate;
                }
                catch { }
            }

            // 2) 常见安装目录（ProgramFiles）
            string[] progRoots = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            };

            foreach (var root in progRoots.Where(r => !string.IsNullOrEmpty(r)))
            {
                try
                {
                    // 搜索 Python* 目录的 python.exe
                    var dirs = Directory.GetDirectories(root, "Python*", SearchOption.TopDirectoryOnly);
                    foreach (var d in dirs)
                    {
                        var candidate = Path.Combine(d, "python.exe");
                        if (File.Exists(candidate)) return candidate;

                        // 有时在 'Scripts' 或 'PCBuild' 下
                        candidate = Directory.EnumerateFiles(d, "python.exe", SearchOption.AllDirectories).FirstOrDefault();
                        if (!string.IsNullOrEmpty(candidate)) return candidate;
                    }
                }
                catch { }
            }

            // 3) 无结果
            return null;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "任意文件|*.*", FileName = "Long" })  
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var dir = Path.GetDirectoryName(sfd.FileName) ?? sfd.FileName;
                    var name = Path.GetFileNameWithoutExtension(sfd.FileName);
                    txtOutPrefix.Text = Path.Combine(dir, name);
                }
            }
        }

        private void btnBrowsePython_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "python.exe|python.exe|所有文件|*.*", Title = "请选择 Python 可执行文件" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPythonExe.Text = ofd.FileName;
                }
            }
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (cmbLayers.SelectedItem == null)
            {
                MessageBox.Show("请先选择输入 DEM。");
                return;
            }

            var layer = cmbLayers.SelectedItem as RasterLayer;
            string inputDem = layer?.FilePath;
            string outPrefix = txtOutPrefix.Text?.Trim();
            string pythonExe = txtPythonExe.Text?.Trim();

            if (string.IsNullOrEmpty(inputDem) || !File.Exists(inputDem))
            {
                MessageBox.Show("选中的输入 DEM 不存在或路径无效。");
                return;
            }
            if (string.IsNullOrWhiteSpace(outPrefix))
            {
                MessageBox.Show("请指定输出路径。");
                return;
            }
            if (string.IsNullOrWhiteSpace(pythonExe) || !File.Exists(pythonExe))
            {
                MessageBox.Show("请指定有效的 Python 解释器路径（python.exe）。");
                return;
            }

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string pythonScript = Path.Combine(appDirectory, "script", "ridge.py");
            if (!File.Exists(pythonScript))
            {
                MessageBox.Show($"找不到脚本：{pythonScript}\n请确认 script\\ridge.py 已随程序一起部署。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var args = new StringBuilder();
                args.Append($"\"{pythonScript}\" --input \"{inputDem}\" --out \"{outPrefix}\"");

                var psi = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = args.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(pythonScript) ?? appDirectory
                };

                using (var proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                    if (proc.ExitCode != 0)
                    {
                        var err = proc.StandardError.ReadToEnd();
                        var outp = proc.StandardOutput.ReadToEnd();
                        MessageBox.Show($"Python 运行失败（退出码 {proc.ExitCode}）：\n{err}\n{outp}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("寻龙完成！");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("调用 Python 失败：\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
