using System;
using System.Drawing;
using System.Windows.Forms;

namespace FSGIS
{
    partial class FormChaSha
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblLayer;
        private ComboBox cmbLayers;
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
            this.lblLayer = new System.Windows.Forms.Label();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();

            this.SuspendLayout();
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(30, 30);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(86, 12);
            this.lblLayer.TabIndex = 0;
            this.lblLayer.Text = "选择 DEM：";
            // 
            // cmbLayers
            // 
            this.cmbLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Location = new System.Drawing.Point(30, 52);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Size = new System.Drawing.Size(350, 20);
            this.cmbLayers.TabIndex = 1;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(30, 86);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(98, 12);
            this.lblPath.TabIndex = 2;
            this.lblPath.Text = "输出路径：";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(30, 106);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(270, 21);
            this.txtOutputPath.TabIndex = 3;
            this.txtOutputPath.Text = "D:\\Sha.tif";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(308, 104);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 24);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(30, 144);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(350, 40);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "开始察砂";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // FormChaSha
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 210);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.lblLayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "风水工具 - 察砂";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}