namespace FSGIS
{
    partial class Form1
    {
        /// <summary>
        /// 设计器组件容器
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理正在使用的资源
        /// </summary>
        /// <param name="disposing">
        /// true：释放托管资源  
        /// false：仅释放非托管资源
        /// </param>
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listTools = new System.Windows.Forms.ListBox();
            this.listLayers = new System.Windows.Forms.ListBox();
            this.treeLayers = new System.Windows.Forms.TreeView();
            this.ctxLayerMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemAttributeTable = new System.Windows.Forms.ToolStripMenuItem();
            this.chkShowRasterValue = new System.Windows.Forms.CheckBox();
            this.panelMap = new System.Windows.Forms.Panel();
            this.lblCoordinate = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fengShuiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liXiangToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guanShuiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chaShaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dianXueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xunLongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ctxLayerMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 36);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listTools);
            this.splitContainer1.Panel1.Controls.Add(this.listLayers);
            this.splitContainer1.Panel1.Controls.Add(this.treeLayers);
            this.splitContainer1.Panel1.Controls.Add(this.chkShowRasterValue);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelMap);
            this.splitContainer1.Panel2.Controls.Add(this.lblCoordinate);
            this.splitContainer1.Size = new System.Drawing.Size(1476, 806);
            this.splitContainer1.SplitterDistance = 375;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // listTools
            // 
            this.listTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTools.FormattingEnabled = true;
            this.listTools.ItemHeight = 18;
            this.listTools.Location = new System.Drawing.Point(0, 509);
            this.listTools.Margin = new System.Windows.Forms.Padding(4);
            this.listTools.Name = "listTools";
            this.listTools.Size = new System.Drawing.Size(375, 275);
            this.listTools.TabIndex = 1;
            this.listTools.SelectedIndexChanged += new System.EventHandler(this.listTools_SelectedIndexChanged);
            // 
            // listLayers
            // 
            this.listLayers.Dock = System.Windows.Forms.DockStyle.Top;
            this.listLayers.FormattingEnabled = true;
            this.listLayers.ItemHeight = 18;
            this.listLayers.Location = new System.Drawing.Point(0, 361);
            this.listLayers.Margin = new System.Windows.Forms.Padding(4);
            this.listLayers.Name = "listLayers";
            this.listLayers.Size = new System.Drawing.Size(375, 148);
            this.listLayers.TabIndex = 2;
            // 
            // treeLayers
            // 
            this.treeLayers.AllowDrop = true;
            this.treeLayers.ContextMenuStrip = this.ctxLayerMenu;
            this.treeLayers.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeLayers.Location = new System.Drawing.Point(0, 0);
            this.treeLayers.Margin = new System.Windows.Forms.Padding(4);
            this.treeLayers.Name = "treeLayers";
            this.treeLayers.Size = new System.Drawing.Size(375, 361);
            this.treeLayers.TabIndex = 0;
            this.treeLayers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeLayers_ItemDrag);
            this.treeLayers.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeLayers_DragDrop);
            this.treeLayers.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeLayers_DragEnter);
            // 
            // ctxLayerMenu
            // 
            this.ctxLayerMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctxLayerMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemAttributeTable});
            this.ctxLayerMenu.Name = "ctxLayerMenu";
            this.ctxLayerMenu.Size = new System.Drawing.Size(171, 34);
            // 
            // itemAttributeTable
            // 
            this.itemAttributeTable.Name = "itemAttributeTable";
            this.itemAttributeTable.Size = new System.Drawing.Size(170, 30);
            this.itemAttributeTable.Text = "加载属性表";
            this.itemAttributeTable.Click += new System.EventHandler(this.itemAttributeTable_Click);
            // 
            // chkShowRasterValue
            // 
            this.chkShowRasterValue.AutoSize = true;
            this.chkShowRasterValue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkShowRasterValue.Location = new System.Drawing.Point(0, 784);
            this.chkShowRasterValue.Name = "chkShowRasterValue";
            this.chkShowRasterValue.Size = new System.Drawing.Size(375, 22);
            this.chkShowRasterValue.TabIndex = 3;
            this.chkShowRasterValue.Text = "显示栅格值";
            this.chkShowRasterValue.UseVisualStyleBackColor = true;
            this.chkShowRasterValue.CheckedChanged += new System.EventHandler(this.chkShowRasterValue_CheckedChanged);
            // 
            // panelMap
            // 
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Margin = new System.Windows.Forms.Padding(4);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(1095, 776);
            this.panelMap.TabIndex = 0;
            // 
            // lblCoordinate
            // 
            this.lblCoordinate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCoordinate.Location = new System.Drawing.Point(0, 776);
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(1095, 30);
            this.lblCoordinate.TabIndex = 1;
            this.lblCoordinate.Text = "X:    Y:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.generalToolStripMenuItem,
            this.fengShuiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1476, 36);
            this.menuStrip1.TabIndex = 1;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLayerToolStripMenuItem,
            this.removeLayerToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(62, 32);
            this.fileToolStripMenuItem.Text = "文件";
            // 
            // addLayerToolStripMenuItem
            // 
            this.addLayerToolStripMenuItem.Name = "addLayerToolStripMenuItem";
            this.addLayerToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.addLayerToolStripMenuItem.Text = "添加图层";
            this.addLayerToolStripMenuItem.Click += new System.EventHandler(this.addLayerToolStripMenuItem_Click);
            // 
            // removeLayerToolStripMenuItem
            // 
            this.removeLayerToolStripMenuItem.Name = "removeLayerToolStripMenuItem";
            this.removeLayerToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.removeLayerToolStripMenuItem.Text = "移除图层";
            this.removeLayerToolStripMenuItem.Click += new System.EventHandler(this.removeLayerToolStripMenuItem_Click);
            // 
            // generalToolStripMenuItem
            // 
            this.generalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.maskToolStripMenuItem});
            this.generalToolStripMenuItem.Name = "generalToolStripMenuItem";
            this.generalToolStripMenuItem.Size = new System.Drawing.Size(116, 32);
            this.generalToolStripMenuItem.Text = "普通工具箱";
            // 
            // maskToolStripMenuItem
            // 
            this.maskToolStripMenuItem.Name = "maskToolStripMenuItem";
            this.maskToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.maskToolStripMenuItem.Text = "裁剪掩膜";
            this.maskToolStripMenuItem.Click += new System.EventHandler(this.maskToolStripMenuItem_Click);
            // 
            // fengShuiToolStripMenuItem
            // 
            this.fengShuiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.liXiangToolStripMenuItem,
            this.guanShuiToolStripMenuItem,
            this.chaShaToolStripMenuItem,
            this.dianXueToolStripMenuItem,
            this.xunLongToolStripMenuItem});
            this.fengShuiToolStripMenuItem.Name = "fengShuiToolStripMenuItem";
            this.fengShuiToolStripMenuItem.Size = new System.Drawing.Size(116, 32);
            this.fengShuiToolStripMenuItem.Text = "风水工具箱";
            // 
            // liXiangToolStripMenuItem
            // 
            this.liXiangToolStripMenuItem.Name = "liXiangToolStripMenuItem";
            this.liXiangToolStripMenuItem.Size = new System.Drawing.Size(146, 34);
            this.liXiangToolStripMenuItem.Text = "立向";
            this.liXiangToolStripMenuItem.Click += new System.EventHandler(this.liXiangToolStripMenuItem_Click);
            // 
            // guanShuiToolStripMenuItem
            // 
            this.guanShuiToolStripMenuItem.Name = "guanShuiToolStripMenuItem";
            this.guanShuiToolStripMenuItem.Size = new System.Drawing.Size(146, 34);
            this.guanShuiToolStripMenuItem.Text = "观水";
            this.guanShuiToolStripMenuItem.Click += new System.EventHandler(this.guanShuiToolStripMenuItem_Click);
            // 
            // chaShaToolStripMenuItem
            // 
            this.chaShaToolStripMenuItem.Name = "chaShaToolStripMenuItem";
            this.chaShaToolStripMenuItem.Size = new System.Drawing.Size(146, 34);
            this.chaShaToolStripMenuItem.Text = "察砂";
            this.chaShaToolStripMenuItem.Click += new System.EventHandler(this.chaShaToolStripMenuItem_Click);
            // 
            // dianXueToolStripMenuItem
            // 
            this.dianXueToolStripMenuItem.Name = "dianXueToolStripMenuItem";
            this.dianXueToolStripMenuItem.Size = new System.Drawing.Size(146, 34);
            this.dianXueToolStripMenuItem.Text = "点穴";
            this.dianXueToolStripMenuItem.Click += new System.EventHandler(this.dianXueToolStripMenuItem_Click);
            // 
            // xunLongToolStripMenuItem
            // 
            this.xunLongToolStripMenuItem.Name = "xunLongToolStripMenuItem";
            this.xunLongToolStripMenuItem.Size = new System.Drawing.Size(146, 34);
            this.xunLongToolStripMenuItem.Text = "寻龙";
            this.xunLongToolStripMenuItem.Click += new System.EventHandler(this.xunLongToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 842);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "FSGIS Basic Version";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ctxLayerMenu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeLayers;
        private System.Windows.Forms.ListBox listTools;
        private System.Windows.Forms.ListBox listLayers;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Label lblCoordinate;

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLayerToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem generalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maskToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem fengShuiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liXiangToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guanShuiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chaShaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dianXueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xunLongToolStripMenuItem;

        private System.Windows.Forms.CheckBox chkShowRasterValue;

        private System.Windows.Forms.ContextMenuStrip ctxLayerMenu;
        private System.Windows.Forms.ToolStripMenuItem itemAttributeTable;
    }
}
