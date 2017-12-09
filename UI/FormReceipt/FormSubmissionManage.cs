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
        public FormSubmissionManage()
        {
            InitializeComponent();
        }

        private void FormSubmissionManage_Load(object sender, EventArgs e)
        {
            InitComponents();
            Search(null, null);
        }

        private void InitComponents()
        {
            //初始化
            this.comboBoxSelect.Items.Add("无");
            string[] columnNames = (from kn in ReceiptMetaData.checkKeyName select kn.Name).ToArray();
            this.comboBoxSelect.Items.AddRange(columnNames);
            this.comboBoxSelect.SelectedIndex = 0;

            //初始化表格
            var worksheet = this.reoGridControl1.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < columnNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = columnNames[i];
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.checkKeyName[i].Visible;
            }
            worksheet.Columns = columnNames.Length;
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
                    submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>("SELECT * FROM SubmissionTicketView").ToArray();
                }
                else
                {
                    double tmp;
                    if (Double.TryParse(value, out tmp) == false) //不是数字则加上单引号
                    {
                        value = "'" + value + "'";
                    }
                    try
                    {
                        submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>(String.Format("SELECT * FROM SubmissionTicketView WHERE {0} = {1}", key, value)).ToArray();
                    }
                    catch
                    {
                        MessageBox.Show("查询的值不合法，请输入正确的值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        object[] columns = Utilities.GetValuesByPropertieNames(curSubmissionTicketView, (from kn in ReceiptMetaData.checkKeyName select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[n, j] = columns[j];
                        }
                        n++;
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
                foreach (KeyName kn in ReceiptMetaData.checkKeyName)
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
                    throw new Exception();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).Single();
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicket SET State='合格' WHERE ID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicketItem SET State='合格' WHERE SubmissionTicketID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='不合格' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Search(null, null);
        }

        private void buttonNoPass_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).Single();
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicket SET State='不合格' WHERE ID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicketItem SET State='不合格' WHERE SubmissionTicketID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='不合格' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Search(null, null);
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormSubmissionItem formSubmissionItem = new FormSubmissionItem(submissionTicketID);
                formSubmissionItem.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Search(null, null);
        }
    }
}
