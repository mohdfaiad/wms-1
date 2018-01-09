﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using unvell.ReoGrid;
using WMS.UI;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormReceiptShelves : Form
    {
        private FormMode formMode;
        private int receiptTicketID;
        private WMSEntities wmsEntities = new WMSEntities();
        private int userID;
        private int warehouseID;
        private int projectID;
        private string key;
        private string value;

        public FormReceiptShelves()
        {
            InitializeComponent();
        }

        public FormReceiptShelves(int projectID, int warehouseID, int userID)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.value = null;
            this.key = null;
        }

        public FormReceiptShelves(FormMode formMode, int receiptTicketID)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.receiptTicketID = receiptTicketID;
        }

        public FormReceiptShelves(int projectID, int warehouseID, int userID, string key, string value)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = key;
            this.value = value;
        }

        private void FormReceiptShelves_Load(object sender, EventArgs e)
        {
            InitComponents();
            if (key != null)
            {
                string name = (from n in ReceiptMetaData.putawayTicketKeyName where n.Key == key select n.Name).FirstOrDefault();
                this.toolStripComboBoxSelect.SelectedItem = name;
                this.toolStripComboBoxSelect.SelectedIndex = this.toolStripComboBoxSelect.Items.IndexOf(name);
                this.toolStripTextBoxSelect.Text = value;
            }
            Search(key, value);
        }

        private void InitComponents()
        {
            //初始化
            this.toolStripComboBoxSelect.Items.Add("无");
            string[] columnNames = (from kn in ReceiptMetaData.putawayTicketKeyName where kn.Visible == true select kn.Name).ToArray();
            this.toolStripComboBoxSelect.Items.AddRange(columnNames);
            this.toolStripComboBoxSelect.SelectedIndex = 0;

            //初始化表格
            var worksheet = this.reoGridControlUser.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.putawayTicketKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.putawayTicketKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.putawayTicketKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.putawayTicketKeyName.Length;
        }

        private void Search(string key, string value)
        {
            this.lableStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                PutawayTicketView[] putawayTicketView = null;
                if (key == null || value == null)        //搜索所有
                {
                    try
                    {
                        putawayTicketView = wmsEntities.Database.SqlQuery<PutawayTicketView>("SELECT * FROM PutawayTicketView WHERE WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", new SqlParameter[] { new SqlParameter("warehouseID", this.warehouseID), new SqlParameter("projectID", this.projectID) }).ToArray();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    double tmp;
                    //if (Double.TryParse(value, out tmp) == false) //不是数字则加上单引号
                    //{
                    //    value = "'" + value + "'";
                    //}
                    try
                    {
                        putawayTicketView = wmsEntities.Database.SqlQuery<PutawayTicketView>(String.Format("SELECT * FROM PutawayTicketView WHERE {0} = @key AND WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", key), new SqlParameter[] { new SqlParameter("@key", value), new SqlParameter("@warehouseID", this.warehouseID), new SqlParameter("@projectID", this.projectID) }).ToArray();
                    }
                    catch(EntityException)
                    {
                        MessageBox.Show("查询的值不合法，请输入正确的值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
                this.reoGridControlUser.Invoke(new Action(() =>
                {
                    this.lableStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlUser.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    int n = 0;
                    for (int i = 0; i < putawayTicketView.Length; i++)
                    {

                        PutawayTicketView curReceiptTicketView = putawayTicketView[i];
                        if (curReceiptTicketView.State == "作废")
                        {
                            continue;
                        }
                        object[] columns = Utilities.GetValuesByPropertieNames(curReceiptTicketView, (from kn in ReceiptMetaData.putawayTicketKeyName select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            if (columns[j] == null)
                            {
                                worksheet[n, j] = columns[j];
                            }
                            else
                            {
                                worksheet[n, j] = columns[j].ToString();
                            }
                        }
                        n++;

                    }
                }));
                if (putawayTicketView.Length == 0)
                {
                    int m = ReceiptUtilities.GetFirstColumnIndex(ReceiptMetaData.submissionTicketKeyName);

                    //this.reoGridControl1.Worksheets[0][6, 8] = "32323";
                    this.reoGridControlUser.Worksheets[0][0, m] = "无查询结果";
                }

            })).Start();

        }

        private void toolStripButtonItem_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormShelvesItem formShelvesItem = new FormShelvesItem(putawayTicketID);
                formShelvesItem.SetCallBack(new Action(() =>
                {
                    this.Search(null, null);
                }));
                formShelvesItem.Show();
            }
            catch(EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }           
        }

        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            {
                Search(null, null);
            }
            else
            {
                string condition = this.toolStripComboBoxSelect.Text;
                string key = "";
                foreach (KeyName kn in ReceiptMetaData.putawayTicketKeyName)
                {
                    if (condition == kn.Name)
                    {
                        key = kn.Key;
                        break;
                    }
                }
                string value = this.toolStripTextBoxSelect.Text;
                Search(key, value);
            }
        }

        private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            {
                this.toolStripTextBoxSelect.Text = "";
                this.toolStripTextBoxSelect.Enabled = false;
            }
            else
            {
                this.toolStripTextBoxSelect.Text = "";
                this.toolStripTextBoxSelect.Enabled = true;
            }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                //FormShelvesItem formShelvesItem = new FormShelvesItem(putawayTicketID);
                FormPutawayModify formPutawayModify = new FormPutawayModify(putawayTicketID);
                formPutawayModify.SetCallBack(new Action(() =>
                {
                    this.Search(null, null);
                }));
                formPutawayModify.Show();
            }
            catch(EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search(null, null);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                //FormShelvesItem formShelvesItem = new FormShelvesItem(putawayTicketID);
                if (MessageBox.Show("确定删除该上架单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM PutawayTicket WHERE ID = @putawayTicketID", new SqlParameter("putawayTicketID", putawayTicketID));
                    }
                    catch(EntityException)
                    {
                        MessageBox.Show("该上架单已被删除!");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Search(null, null);
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBoxSelect_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            {
                this.toolStripTextBoxSelect.Text = "";
                this.toolStripTextBoxSelect.Enabled = false;
            }
            else
            {
                this.toolStripTextBoxSelect.Text = "";
                this.toolStripTextBoxSelect.Enabled = true;
            }
        }

        private void toolStripButtonDistributeCancel_Click(object sender, EventArgs e)
        {

        }
    }
}