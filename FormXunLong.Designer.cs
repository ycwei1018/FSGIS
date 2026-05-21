using System.Drawing;
using System.Windows.Forms;

namespace FSGIS
{
    partial class FormXunLong
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblLayer;
        private ComboBox cmbLayers;
        private Label lblPath;
        private TextBox txtOutPrefix;
        private Button btnBrowse;
        private Label lblPython;
        private TextBox txtPythonExe;
        private Button btnBrowsePython;
        private Button btnRun;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblLayer = new System.Windows.Forms.Label();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtOutPrefix = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblPython = new System.Windows.Forms.Label();
            this.txtPythonExe = new System.Windows.Forms.TextBox();
            this.btnBrowsePython = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();

            this.SuspendLayout();
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(20, 18);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(65, 12);
            this.lblLayer.TabIndex = 0;
            this.lblLayer.Text = "选择DEM图层：";
            // 
            // cmbLayers
            // 
            this.cmbLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Location = new System.Drawing.Point(20, 40);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Size = new System.Drawing.Size(320, 20);
            this.cmbLayers.TabIndex = 1;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(20, 75);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(101, 12);
            this.lblPath.TabIndex = 2;
            this.lblPath.Text = "输出路径：";
            // 
            // txtOutPrefix
            // 
            this.txtOutPrefix.Location = new System.Drawing.Point(20, 95);
            this.txtOutPrefix.Name = "txtOutPrefix";
            this.txtOutPrefix.Size = new System.Drawing.Size(240, 21);
            this.txtOutPrefix.TabIndex = 3;
            this.txtOutPrefix.Text = "D:\\Long";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(270, 94);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(70, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblPython
            // 
            this.lblPython.AutoSize = true;
            this.lblPython.Location = new System.Drawing.Point(20, 130);
            this.lblPython.Name = "lblPython";
            this.lblPython.Size = new System.Drawing.Size(113, 12);
            this.lblPython.TabIndex = 5;
            this.lblPython.Text = "Python 可执行文件：";
            // 
            // txtPythonExe
            // 
            this.txtPythonExe.Location = new System.Drawing.Point(20, 150);
            this.txtPythonExe.Name = "txtPythonExe";
            this.txtPythonExe.Size = new System.Drawing.Size(240, 21);
            this.txtPythonExe.TabIndex = 6;
            this.txtPythonExe.Text = "D:\\Python\\Python312\\python.exe";
            // 
            // btnBrowsePython
            // 
            this.btnBrowsePython.Location = new System.Drawing.Point(270, 149);
            this.btnBrowsePython.Name = "btnBrowsePython";
            this.btnBrowsePython.Size = new System.Drawing.Size(70, 23);
            this.btnBrowsePython.TabIndex = 7;
            this.btnBrowsePython.Text = "浏览...";
            this.btnBrowsePython.UseVisualStyleBackColor = true;
            this.btnBrowsePython.Click += new System.EventHandler(this.btnBrowsePython_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(20, 190);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(320, 36);
            this.btnRun.TabIndex = 8;
            this.btnRun.Text = "开始寻龙";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // FormXunLong
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 240);
            this.Controls.Add(this.lblLayer);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.txtOutPrefix);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblPython);
            this.Controls.Add(this.txtPythonExe);
            this.Controls.Add(this.btnBrowsePython);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormXunLong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "风水工具 - 寻龙";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}