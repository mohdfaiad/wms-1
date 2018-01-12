﻿namespace WMS.UI
{
    partial class FormBaseSupply
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer supplys = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (supplys != null))
            {
                supplys.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBaseSupply));
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.reoGridControlSupply = new unvell.ReoGrid.ReoGridControl();
            this.panelPager = new System.Windows.Forms.Panel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelSelect = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxSelect = new System.Windows.Forms.ToolStripComboBox();
            this.textBoxSearchValue = new System.Windows.Forms.ToolStripTextBox();
            this.buttonSearch = new System.Windows.Forms.ToolStripButton();
            this.buttonHistorySearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSupplySingleBoxTranPackingInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSupplyOuterPackingSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSupplyShipmentInfo = new System.Windows.Forms.ToolStripButton();
            this.buttonImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 39);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1352, 677);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 569);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1585, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(69, 22);
            this.labelStatus.Text = "零件信息";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.reoGridControlSupply, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelPager, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1585, 541);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // reoGridControlSupply
            // 
            this.reoGridControlSupply.BackColor = System.Drawing.Color.White;
            this.reoGridControlSupply.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlSupply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlSupply.LeadHeaderContextMenuStrip = null;
            this.reoGridControlSupply.Location = new System.Drawing.Point(3, 2);
            this.reoGridControlSupply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reoGridControlSupply.Name = "reoGridControlSupply";
            this.reoGridControlSupply.RowHeaderContextMenuStrip = null;
            this.reoGridControlSupply.Script = null;
            this.reoGridControlSupply.SheetTabContextMenuStrip = null;
            this.reoGridControlSupply.SheetTabNewButtonVisible = true;
            this.reoGridControlSupply.SheetTabVisible = true;
            this.reoGridControlSupply.SheetTabWidth = 80;
            this.reoGridControlSupply.ShowScrollEndSpacing = true;
            this.reoGridControlSupply.Size = new System.Drawing.Size(1579, 499);
            this.reoGridControlSupply.TabIndex = 4;
            this.reoGridControlSupply.Text = "reoGridControl1";
            // 
            // panelPager
            // 
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(4, 507);
            this.panelPager.Margin = new System.Windows.Forms.Padding(4);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(1577, 30);
            this.panelPager.TabIndex = 5;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelSelect,
            this.toolStripComboBoxSelect,
            this.textBoxSearchValue,
            this.buttonSearch,
            this.buttonHistorySearch,
            this.toolStripSeparator2,
            this.toolStripButtonAdd,
            this.toolStripButtonAlter,
            this.toolStripButtonDelete,
            this.toolStripSeparator3,
            this.toolStripButtonSupplySingleBoxTranPackingInfo,
            this.toolStripButtonSupplyOuterPackingSize,
            this.toolStripButtonSupplyShipmentInfo,
            this.toolStripSeparator1,
            this.buttonImport});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(1585, 28);
            this.toolStripTop.TabIndex = 2;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripLabelSelect
            // 
            this.toolStripLabelSelect.Name = "toolStripLabelSelect";
            this.toolStripLabelSelect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripLabelSelect.Size = new System.Drawing.Size(84, 25);
            this.toolStripLabelSelect.Text = "查询条件：";
            // 
            // toolStripComboBoxSelect
            // 
            this.toolStripComboBoxSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxSelect.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.toolStripComboBoxSelect.Name = "toolStripComboBoxSelect";
            this.toolStripComboBoxSelect.Size = new System.Drawing.Size(151, 28);
            this.toolStripComboBoxSelect.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxSelect_SelectedIndexChanged);
            // 
            // textBoxSearchValue
            // 
            this.textBoxSearchValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearchValue.Enabled = false;
            this.textBoxSearchValue.Name = "textBoxSearchValue";
            this.textBoxSearchValue.Size = new System.Drawing.Size(201, 28);
            this.textBoxSearchValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSearchValue_KeyPress);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearch.Image")));
            this.buttonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(63, 25);
            this.buttonSearch.Text = "查询";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonHistorySearch
            // 
            this.buttonHistorySearch.Image = ((System.Drawing.Image)(resources.GetObject("buttonHistorySearch.Image")));
            this.buttonHistorySearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonHistorySearch.Name = "buttonHistorySearch";
            this.buttonHistorySearch.Size = new System.Drawing.Size(123, 25);
            this.buttonHistorySearch.Text = "查看历史信息";
            this.buttonHistorySearch.Click += new System.EventHandler(this.buttonHistorySearch_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::WMS.UI.Properties.Resources.add;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(63, 25);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = global::WMS.UI.Properties.Resources.cancle;
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(63, 25);
            this.toolStripButtonAlter.Text = "修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = global::WMS.UI.Properties.Resources.delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(63, 25);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonSupplySingleBoxTranPackingInfo
            // 
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButtonSupplySingleBoxTranPackingInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Name = "toolStripButtonSupplySingleBoxTranPackingInfo";
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Size = new System.Drawing.Size(189, 25);
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Text = "查看/修改单箱包装信息";
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Click += new System.EventHandler(this.toolStripButtonSupplySingleBoxTranPackingInfo_Click);
            // 
            // toolStripButtonSupplyOuterPackingSize
            // 
            this.toolStripButtonSupplyOuterPackingSize.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButtonSupplyOuterPackingSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSupplyOuterPackingSize.Name = "toolStripButtonSupplyOuterPackingSize";
            this.toolStripButtonSupplyOuterPackingSize.Size = new System.Drawing.Size(204, 25);
            this.toolStripButtonSupplyOuterPackingSize.Text = "查看/修改零件外包装信息";
            this.toolStripButtonSupplyOuterPackingSize.Click += new System.EventHandler(this.toolStripButtonSupplyOuterPackingSize_Click);
            // 
            // toolStripButtonSupplyShipmentInfo
            // 
            this.toolStripButtonSupplyShipmentInfo.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButtonSupplyShipmentInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSupplyShipmentInfo.Name = "toolStripButtonSupplyShipmentInfo";
            this.toolStripButtonSupplyShipmentInfo.Size = new System.Drawing.Size(189, 25);
            this.toolStripButtonSupplyShipmentInfo.Text = "查看/修改出货包装信息";
            this.toolStripButtonSupplyShipmentInfo.Click += new System.EventHandler(this.toolStripButtonSupplyShipmentInfo_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Image = global::WMS.UI.Properties.Resources.add;
            this.buttonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(93, 25);
            this.buttonImport.Text = "批量导入";
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // FormBaseSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1585, 594);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.toolStripTop);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormBaseSupply";
            this.Text = "零件信息";
            this.Load += new System.EventHandler(this.FormBaseSupply_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSelect;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSelect;
        private System.Windows.Forms.ToolStripTextBox textBoxSearchValue;
        private System.Windows.Forms.ToolStripButton buttonSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private unvell.ReoGrid.ReoGridControl reoGridControlSupply;
        private System.Windows.Forms.Panel panelPager;
        private System.Windows.Forms.ToolStripButton toolStripButtonSupplySingleBoxTranPackingInfo;
        private System.Windows.Forms.ToolStripButton toolStripButtonSupplyOuterPackingSize;
        private System.Windows.Forms.ToolStripButton toolStripButtonSupplyShipmentInfo;
        private System.Windows.Forms.ToolStripButton buttonHistorySearch;
        private System.Windows.Forms.ToolStripButton buttonImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}