namespace FSGIS
{
    partial class FormMask
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblRaster = new System.Windows.Forms.Label();
            this.cmbRaster = new System.Windows.Forms.ComboBox();
            this.lblVector = new System.Windows.Forms.Label();
            this.cmbVector = new System.Windows.Forms.ComboBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button(); 
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRaster
            // 
            this.lblRaster.AutoSize = true;
            this.lblRaster.Location = new System.Drawing.Point(20, 20);
            this.lblRaster.Name = "lblRaster";
            this.lblRaster.Size = new System.Drawing.Size(65, 12);
            this.lblRaster.TabIndex = 0;
            this.lblRaster.Text = "选择栅格：";
            // 
            // cmbRaster
            // 
            this.cmbRaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRaster.FormattingEnabled = true;
            this.cmbRaster.Location = new System.Drawing.Point(20, 40);
            this.cmbRaster.Name = "cmbRaster";
            this.cmbRaster.Size = new System.Drawing.Size(300, 20);
            this.cmbRaster.TabIndex = 1;
            // 
            // lblVector
            // 
            this.lblVector.AutoSize = true;
            this.lblVector.Location = new System.Drawing.Point(20, 80);
            this.lblVector.Name = "lblVector";
            this.lblVector.Size = new System.Drawing.Size(89, 12);
            this.lblVector.TabIndex = 2;
            this.lblVector.Text = "掩膜图层(面)：";
            // 
            // cmbVector
            // 
            this.cmbVector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVector.FormattingEnabled = true;
            this.cmbVector.Location = new System.Drawing.Point(20, 100);
            this.cmbVector.Name = "cmbVector";
            this.cmbVector.Size = new System.Drawing.Size(300, 20);
            this.cmbVector.TabIndex = 3;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(20, 140);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(65, 12);
            this.lblPath.TabIndex = 4;
            this.lblPath.Text = "输出路径：";
            // 
            // txtOut
            // 
            this.txtOut.Location = new System.Drawing.Point(20, 160);
            this.txtOut.Name = "txtOut";
            this.txtOut.Size = new System.Drawing.Size(220, 21);
            this.txtOut.TabIndex = 5;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(250, 158);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(70, 23);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(20, 200);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(300, 40);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "开始裁剪";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // FormMask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 260);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.cmbVector);
            this.Controls.Add(this.lblVector);
            this.Controls.Add(this.cmbRaster);
            this.Controls.Add(this.lblRaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMask";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "掩膜";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRaster;
        private System.Windows.Forms.ComboBox cmbRaster;
        private System.Windows.Forms.Label lblVector;
        private System.Windows.Forms.ComboBox cmbVector;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnRun;
    }
}