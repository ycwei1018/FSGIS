using System.Drawing;
using System.Windows.Forms;

namespace FSGIS
{
    partial class FormGuanShui
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblVector;
        private ComboBox cmbVectorLayers;
        private Label lblRaster;
        private ComboBox cmbRasterLayers;
        private Label lblPath;
        private TextBox txtOutputPath;
        private Button btnBrowse;
        private Button btnRun;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblVector = new System.Windows.Forms.Label();
            this.cmbVectorLayers = new System.Windows.Forms.ComboBox();
            this.lblRaster = new System.Windows.Forms.Label();
            this.cmbRasterLayers = new System.Windows.Forms.ComboBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();

            this.SuspendLayout();
            // 
            // lblVector
            // 
            this.lblVector.AutoSize = true;
            this.lblVector.Location = new System.Drawing.Point(30, 20);
            this.lblVector.Name = "lblVector";
            this.lblVector.Size = new System.Drawing.Size(120, 12);
            this.lblVector.TabIndex = 0;
            this.lblVector.Text = "选择水系矢量：";
            // 
            // cmbVectorLayers
            // 
            this.cmbVectorLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVectorLayers.FormattingEnabled = true;
            this.cmbVectorLayers.Location = new System.Drawing.Point(30, 40);
            this.cmbVectorLayers.Name = "cmbVectorLayers";
            this.cmbVectorLayers.Size = new System.Drawing.Size(350, 20);
            this.cmbVectorLayers.TabIndex = 1;
            // 
            // lblRaster
            // 
            this.lblRaster.AutoSize = true;
            this.lblRaster.Location = new System.Drawing.Point(30, 74);
            this.lblRaster.Name = "lblRaster";
            this.lblRaster.Size = new System.Drawing.Size(98, 12);
            this.lblRaster.TabIndex = 2;
            this.lblRaster.Text = "选择参考栅格：";
            // 
            // cmbRasterLayers
            // 
            this.cmbRasterLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRasterLayers.FormattingEnabled = true;
            this.cmbRasterLayers.Location = new System.Drawing.Point(30, 94);
            this.cmbRasterLayers.Name = "cmbRasterLayers";
            this.cmbRasterLayers.Size = new System.Drawing.Size(350, 20);
            this.cmbRasterLayers.TabIndex = 3;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(30, 128);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(98, 12);
            this.lblPath.TabIndex = 4;
            this.lblPath.Text = "输出水指数路径：";
            // 
            // txtOutputPath
            // 
                this.txtOutputPath.Location = new System.Drawing.Point(30, 148);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(270, 21);
            this.txtOutputPath.TabIndex = 5;
            this.txtOutputPath.Text = "D:\\Shui.tif";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(308, 146);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 24);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(30, 184);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(350, 40);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "开始观水";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // FormGuanShui
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 240);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.cmbRasterLayers);
            this.Controls.Add(this.lblRaster);
            this.Controls.Add(this.cmbVectorLayers);
            this.Controls.Add(this.lblVector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "风水工具 - 观水";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}