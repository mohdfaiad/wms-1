﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.UI.FormReceipt;
using unvell.ReoGrid;
using System.Threading;
using System.Data.SqlClient;
using WMS.DataAccess;


namespace WMS.UI
{
    public partial class FormSubmissionManage : Form
    {
        private int projectID;
        private int warehouseID;
        private int userID;

        private string key;
        private string value;

        public FormSubmissionManage()
        {
            InitializeComponent();
        }

        public FormSubmissionManage(int projectID, int warehouseID, int userID)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = null;
            this.value = null;
        }

        public FormSubmissionManage(int projectID, int warehouseID, int userID, string key, string value)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = key;
            this.value = value;
        }

        private void FormSubmissionManage_Load(object sender, EventArgs e)
        {
            InitComponents();
            if (key != null)
            {
                string name = (from n in ReceiptMetaData.submissionTicketKeyName where n.Key == key select n.Name).FirstOrDefault();
                this.comboBoxSelect.SelectedItem = name;
                this.comboBoxSelect.SelectedIndex = this.comboBoxSelect.Items.IndexOf(name);
                
            }
            this.textBoxSelect.Text = value;
            Search(key, value);
        }

        private void InitComponents()
        {
            //初始化
            this.comboBoxSelect.Items.Add("无");
            string[] columnNames = (from kn in ReceiptMetaData.submissionTicketKeyName where kn.Visible == true select kn.Name).ToArray();
            this.comboBoxSelect.Items.AddRange(columnNames);
            this.comboBoxSelect.SelectedIndex = 0;

            //初始化表格
            var worksheet = this.reoGridControl1.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.submissionTicketKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.submissionTicketKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.submissionTicketKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.submissionTicketKeyName.Length;
        }

        private void Search(string key, string value)
        {
            this.toolStripStatusLabel2.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                SubmissionTicketView[] submissionTicketView = null;
                if (key == null || value == null)        //搜索所有
                {
                    try
                    {
                        submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>("SELECT * FROM SubmissionTicketView WHERE WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", new SqlParameter[] { new SqlParameter("warehouseID", this.warehouseID), new SqlParameter("projectID", this.projectID) }).ToArray();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //double tmp;
                    //if (Double.TryParse(value, out tmp) == false) //不是数字则加上单引号
                    //{
                    //    value = "'" + value + "'";
                    //}
                    try
                    {
                        submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>(String.Format("SELECT * FROM SubmissionTicketView WHERE {0} = @key AND ReceiptTicketWarehouse = @warehouseID AND ReceiptTicketProjectID = @projectID ORDER BY ID DESC", key), new SqlParameter[] { new SqlParameter("@key", value), new SqlParameter("@warehouseID", this.warehouseID), new SqlParameter("@projectID", this.projectID) }).ToArray();
                    }
                    catch (EntityException)
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

                this.reoGridControl1.Invoke(new Action(() =>
                {
                    this.toolStripStatusLabel2.Text = "搜索完成";
                    var worksheet = this.reoGridControl1.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    int n = 0;
                    for (int i = 0; i < submissionTicketView.Length; i++)
                    {
                        if (submissionTicketView[i].State == "作废")
                        {
                            continue;
                        }
                        SubmissionTicketView curSubmissionTicketView = submissionTicketView[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curSubmissionTicketView, (from kn in ReceiptMetaData.submissionTicketKeyName select kn.Key).ToArray());
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
                    if (submissionTicketView.Length == 0)
                    {
                        int m = ReceiptUtilities.GetFirstColumnIndex(ReceiptMetaData.submissionTicketKeyName);

                        //this.reoGridControl1.Worksheets[0][6, 8] = "32323";
                        this.reoGridControl1.Worksheets[0][0, m] = "无查询结果";
                    }
                }));

            })).Start();

        }

        private void reoGridControlUser_Click(object sender, EventArgs e)
        {

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (comboBoxSelect.SelectedIndex == 0)
            {
                Search(null, null);
            }
            else
            {
                string condition = this.comboBoxSelect.Text;
                string key = "";
                foreach (KeyName kn in ReceiptMetaData.submissionTicketKeyName)
                {
                    if (condition == kn.Name)
                    {
                        key = kn.Key;
                        break;
                    }
                }
                string value = this.textBoxSelect.Text;
                Search(key, value);
            }
        }

        private void buttonPass_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("找不到该送检单");
                    return;
                }
                SubmissionTicketItem[] submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.SubmissionTicketID == submissionTicketID select sti).ToArray();
                foreach (SubmissionTicketItem sti in submissionTicketItem)
                {
                    sti.State = "合格";
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                    if (receiptTicketItem != null)
                    {
                        receiptTicketItem.State = "过检";
                    }
                }
                submissionTicket.State = "合格";
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    if (receiptTicket != null)
                    {
                        int count = wmsEntities.Database.SqlQuery<int>(
                        "SELECT COUNT(*) FROM ReceiptTicketItem " +
                        "WHERE State <> '过检' AND ReceiptTicketID = @receiptTicketID",
                        new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                        if (count == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE ReceiptTicket SET State = '过检' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID));
                        }
                        else
                        {
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State = '已收货' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分过检' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                        }
                        if (MessageBox.Show("是否同时收货?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (SubmissionTicketItem sti in submissionTicketItem)
                            {
                            //sti.State = "合格";
                                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                                if (receiptTicketItem != null)
                                {
                                    receiptTicketItem.State = "已收货";
                                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    //StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    if (stockInfo != null)
                                    {/*TODO
                                        if (stockInfo.SubmissionAreaAmount != null)
                                        {
                                            int amount = (int)stockInfo.SubmissionAreaAmount;
                                            stockInfo.ReceiptAreaAmount += amount;
                                            stockInfo.SubmissionAreaAmount = 0;
                                        }*/
                                    }
                                }
                            }
                            wmsEntities.SaveChanges();
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State <> '已收货' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '已收货' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                            else
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分收货' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        this.Search(null, null);
                    }));
                }).Start();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }

            /*
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).Single();
                if (submissionTicket.State == "合格")
                {
                    MessageBox.Show("该送检单状态已置为合格");
                }
                else
                {
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicket SET State='合格' WHERE ID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicketItem SET State='合格' WHERE SubmissionTicketID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='过检' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='过检' WHERE ReceiptTicketID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));

                    if (MessageBox.Show("是否同时收货？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket != null)
                        {
                            if (receiptTicket.State != "已收货")
                            {
                                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='已收货' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='已收货' WHERE ReceiptTicketID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                            }
                            
                        }
                    }
                }
            }
            catch(EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search(null, null);
            */
        }

        private void buttonNoPass_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];

            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("找不到该送检单");
                    return;
                }
                SubmissionTicketItem[] submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.SubmissionTicketID == submissionTicketID select sti).ToArray();
                foreach (SubmissionTicketItem sti in submissionTicketItem)
                {
                    sti.State = "不合格";
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                    if (receiptTicketItem != null)
                    {
                        receiptTicketItem.State = "未过检";
                    }
                }
                submissionTicket.State = "不合格";
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    if (receiptTicket != null)
                    {
                        int count = wmsEntities.Database.SqlQuery<int>(
                        "SELECT COUNT(*) FROM ReceiptTicketItem " +
                        "WHERE State <> '未过检' AND ReceiptTicketID = @receiptTicketID",
                        new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                        if (count == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE ReceiptTicket SET State = '未过检' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID));
                        }
                        else
                        {
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State = '已收货' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分过检' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                        }
                        if (MessageBox.Show("是否同时拒收?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            
                            foreach (SubmissionTicketItem sti in submissionTicketItem)
                            {
                                //sti.State = "合格";
                                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                                if (receiptTicketItem != null)
                                {
                                    receiptTicketItem.State = "拒收";
                                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    //StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    if (stockInfo != null)
                                    {/*TODO
                                        if (stockInfo.SubmissionAreaAmount != null)
                                        {
                                            int amount = (int)stockInfo.SubmissionAreaAmount;
                                            stockInfo.ReceiptAreaAmount += amount;
                                            stockInfo.SubmissionAreaAmount = 0;
                                        }*/
                                    }
                                }

                            }
                            wmsEntities.SaveChanges();
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State <> '拒收' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '拒收' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                            else
                            {
                                int count3 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State = '已收货' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                                if (count3 != 0)
                                {
                                    wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分收货' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                                }
                                /*
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分收货' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                                    */
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        this.Search(null, null);
                    }));
                }).Start();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }


            /*
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).Single();
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicket SET State='不合格' WHERE ID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicketItem SET State='不合格' WHERE SubmissionTicketID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                if (MessageBox.Show("是否同时拒收?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='拒收' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='拒收' WHERE ReceiptTicketID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                }
            }
            catch (EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search(null, null);
            */
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormSubmissionItem formSubmissionItem = new FormSubmissionItem(submissionTicketID);
                formSubmissionItem.SetCallBack(new Action(() =>
                {
                    this.Search(null, null);
                }));
                formSubmissionItem.Show();
            }
            catch (EntityCommandExecutionException)
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

        private void buttonItems_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormAddSubmissionTicket formAddSubmissionTicket = new FormAddSubmissionTicket(submissionTicketID, this.userID, FormMode.ALTER);
                formAddSubmissionTicket.SetCallBack(new Action(() =>
                {
                    this.Search(null, null);
                }));
                formAddSubmissionTicket.Show();
                /*
                FormReceiptArrivalCheck formReceiptArrivalCheck = new FormReceiptArrivalCheck(submissionTicketID, this.userID);
                formReceiptArrivalCheck.SetFinishedAction(()=> {
                    Search(null, null);
                });
                formReceiptArrivalCheck.Show();
                */
            }
            catch (EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search(null, null);
        }

        private void comboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxSelect.SelectedIndex == 0)
            {
                this.textBoxSelect.Text = "";
                this.textBoxSelect.Enabled = false;
            }
            else
            {
                this.textBoxSelect.Text = "";
                this.textBoxSelect.Enabled = true;
            }
        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

            
            if (MessageBox.Show("确认删除，并取消送检？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Search(null, null);
                return;
            }
            var worksheet = this.reoGridControl1.Worksheets[0];
            WMSEntities wmsEntities = new WMSEntities();
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("此送检单已被删除");
                    this.Search(null, null);
                    return;
                }
                else
                {
                    new Thread(() =>
                    {
                        try
                        {
                            SubmissionTicketItem[] submissionTicketItems = (from sti in wmsEntities.SubmissionTicketItem where sti.SubmissionTicketID == submissionTicketID select sti).ToArray();

                            foreach (SubmissionTicketItem sti in submissionTicketItems)
                            {
                                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                                if (receiptTicketItem != null)
                                {
                                    if (receiptTicketItem.ReceiptTicket.State == "送检中" || receiptTicketItem.ReceiptTicket.State == "过检" || receiptTicketItem.ReceiptTicket.State == "未过检")
                                    {
                                        receiptTicketItem.State = "待送检";
                                        StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                        if (stockInfo != null)
                                        {
                                            stockInfo.ReceiptAreaAmount += stockInfo.SubmissionAmount;
                                            stockInfo.SubmissionAmount -= stockInfo.SubmissionAmount;
                                        }
                                        receiptTicketItem.State = "待送检";
                                    }
                                }
                                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();

                                if (receiptTicket.State == "送检中" || receiptTicket.State == "过检" || receiptTicket.State == "未过检")
                                {
                                    if (receiptTicket != null)
                                    {
                                        receiptTicket.State = "待送检";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该送检单对应收货单已收货或上架，无法改变收货单状态，但删除送检单成功。");
                                }
                                wmsEntities.Database.ExecuteSqlCommand("DELETE FROM SubmissionTicket WHERE ID = @submissionTicketID", new SqlParameter("submissionTicketID", submissionTicket.ID));
                                wmsEntities.SaveChanges();
                                this.Search(null, null);
                                //wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State = '待送检' WHERE ID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", sti.ReceiptTicketItemID));
                            }
                            
                        }
                        catch
                        {
                            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            return;
                        }
                    }).Start();
                }
            }
            catch (EntityCommandExecutionException)
            {
                MessageBox.Show("请选择收货单");
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            Search(null, null);
        }
    }
}