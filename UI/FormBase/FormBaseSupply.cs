﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormBaseSupply : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int authority;
        private int authority_self = (int)Authority.BASE_COMPONENT;
        int supplierID = -1;
        private int check_history = 0;
        int projectID = -1;
        int warehouseID = -1;
        int userID = -1;
        private Supplier supplier = null;
        private int contractst;   //合同状态
        private int contract_change = 1;
        private PagerWidget<SupplyView> pagerWidget = null;

        public FormBaseSupply(int authority, int supplierID, int projectID, int warehouseID, int userID)
        {
            InitializeSupply();
            this.authority = authority;
            this.supplierID = supplierID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
        }
        private void InitSupplys()
        {
            string[] visibleColumnNames = (from kn in SupplyViewMetaData.supplykeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.toolStripComboBoxSelect.Items.Add("无");
            this.toolStripComboBoxSelect.Items.AddRange(visibleColumnNames);
            this.toolStripComboBoxSelect.SelectedIndex = 0;

            this.pagerWidget = new PagerWidget<SupplyView>(this.reoGridControlSupply, SupplyViewMetaData.supplykeyNames, this.projectID, this.warehouseID);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();
        }

        private void FormBaseSupply_Load(object sender, EventArgs e)
        {
            if(supplierID !=0)
            { this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
                this.toolStripButtonAlter.Enabled  = false;
            }

            if ((this.authority & authority_self) != authority_self)
            {
                this.contract_change = 0;
                Supplier supplier = (from u in this.wmsEntities.Supplier
                                     where u.ID == supplierID
                                     select u).Single();
                this.supplier = supplier;
                this.contractst = Convert.ToInt32(supplier.ContractState);
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
                if (this.contractst == 0)
                {
                    this.toolStripButtonAlter.Enabled = false;
                }

                InitSupplys();

                this.pagerWidget.AddCondition("ID", Convert.ToString(supplierID));
                this.pagerWidget.AddCondition("IsHistory", "0");
                this.pagerWidget.Search();

            }
            if ((this.authority & authority_self) == authority_self)
            {
                InitSupplys();
                this.pagerWidget.AddCondition("IsHistory", "0");
                this.pagerWidget.Search();
            }
        }

        private void reoGridControlUser_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {

            if (this.buttonSearch.Text == "全部信息")
            {
                this.buttonSearch.Text = "查询";
                this.buttonSearch.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonAlter.Enabled = true;
                this.toolStripComboBoxSelect.Enabled = true;
                this.buttonImport.Enabled = true;
            }

            this.pagerWidget.ClearCondition();
            this.pagerWidget.AddCondition("IsHistory", "0");

            if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            {
                this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.textBoxSearchValue.Text);
            }
            if ((this.authority & authority_self) != authority_self)
            {
                this.pagerWidget.AddCondition("ID", Convert.ToString(supplierID));
                this.check_history = 0;
                this.pagerWidget.Search();
            }
            if ((this.authority & authority_self) == authority_self)
            {
                this.check_history = 0;
                this.pagerWidget.Search();
            }
        }


        private void buttonHistorySearch_Click(object sender, EventArgs e)
        {
            if (check_history == 1)
            {
                MessageBox.Show("已经显示历史信息了", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.toolStripButtonAdd.Enabled = false;
            this.toolStripButtonAlter.Enabled = false;
            this.toolStripComboBoxSelect.Enabled = false;
            this.buttonImport.Enabled = false;
            this.buttonSearch.DisplayStyle= ToolStripItemDisplayStyle.Text;
            this.buttonSearch.Text = "全部信息";
            


            this.pagerWidget.ClearCondition();

            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.pagerWidget.AddCondition("NewestSupplyID", Convert.ToString(componenID));
            }

            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.pagerWidget.AddCondition("IsHistory", "1");

            if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            {

                this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.textBoxSearchValue.Text);
            }
            if ((this.authority & authority_self) != authority_self)
            {
                this.pagerWidget.AddCondition("SupplierID", Convert.ToString(supplierID));
                this.check_history = 1;
                this.pagerWidget.Search();
            }
            if ((this.authority & authority_self) == authority_self)
            {
                this.check_history = 1;
                this.pagerWidget.Search();
            }




        }



        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormSupplyModify(this.projectID, this.warehouseID, this.supplierID, this.userID);
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback((addedID) =>
            {
                this.pagerWidget.Search(false,addedID);
            });
            form.Show();

        }//添加
  

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formSupplyModify = new FormSupplyModify(this.projectID, this.warehouseID, this.supplierID, this.userID, componenID);
                formSupplyModify.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false,addedID);
                });
                formSupplyModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }//修改

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            List<int> deleteIDs = new List<int>();
            for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
            {
                try
                {
                    int curID = int.Parse(worksheet[i + worksheet.SelectionRange.Row, 0].ToString());
                    deleteIDs.Add(curID);
                }
                catch
                {
                    continue;
                }
            }
            if (deleteIDs.Count == 0)
            {
                MessageBox.Show("请选择您要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除...";


                new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        foreach (int id in deleteIDs)
                        {

                            var componen_historyid = (from kn in wmsEntities.Supply
                                                      where kn.NewestSupplyID == id
                                                      select kn.ID).ToArray();
                            if (componen_historyid.Length > 0)
                            {
                                try
                                {
                                    foreach (int NewestSupplyid in componen_historyid)
                                    {
                                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Supply WHERE ID = @componentID", new SqlParameter("componentID", NewestSupplyid));


                                    }
                                    wmsEntities.SaveChanges();
                                }
                                catch
                                {
                                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                        }

                    }
                    catch
                    {
                        MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        foreach (int id in deleteIDs)
                        {
                            this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Supply WHERE ID = @componenID", new SqlParameter("componenID", id));
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.wmsEntities.SaveChanges();
                    this.Invoke(new Action(() =>
                    {
                        this.pagerWidget.Search();
                        MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                })).Start();


        }//删除

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.pagerWidget.Search();
            }
        }

        private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            {
                this.textBoxSearchValue.Text = "";
                this.textBoxSearchValue.Enabled = false;
                this.textBoxSearchValue.BackColor = Color.LightGray;
            }
            else
            {
                this.textBoxSearchValue.Enabled = true;
                this.textBoxSearchValue.BackColor = Color.White;
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonSupplySingleBoxTranPackingInfo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var form = new ComponentSingleBoxTranPackingInfoModify(this.userID,componenID);
                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonSupplyOuterPackingSize_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var form = new ComponentOuterPackingSizeModify(this.userID,componenID);
                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonSupplyShipmentInfo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var form = new ComponentShipmentInfoModify(this.userID,componenID);

                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            //创建导入窗口
            StandardImportForm<DataAccess.Supply> formImport =
                new StandardImportForm<DataAccess.Supply>
                (
                    //参数1：KeyName
                    ComponenViewMetaData.componenkeyNames,
                    (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                    {
                        return true;
                    },
                    () => //参数3：导入完成回调函数
                    {
                        this.pagerWidget.Search();
                    }
                );

            //显示导入窗口
            formImport.Show();
        }
    }
    }
