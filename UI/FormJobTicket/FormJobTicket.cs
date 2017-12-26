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
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormJobTicket : Form
    {
        WMSEntities wmsEntities = new WMSEntities();
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;

        private Action<string> toPutOutStorageTicketCallback = null;

        public void SetToPutOutStorageTicketCallback(Action<string> callback)
        {
            this.toPutOutStorageTicketCallback = callback;
        }

        public FormJobTicket(int userID,int projectID,int warehouseID)
        {
            InitializeComponent();
            InitComponents();
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void FormJobTicket_Load(object sender, EventArgs e)
        {
            this.Search();
        }

        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in JobTicketViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.comboBoxSearchCondition.Items.Add("无");
            this.comboBoxSearchCondition.Items.AddRange(visibleColumnNames);
            this.comboBoxSearchCondition.SelectedIndex = 0;


            //初始化表格
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;

            for (int i = 0; i < JobTicketViewMetaData.KeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = JobTicketViewMetaData.KeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = JobTicketViewMetaData.KeyNames[i].Visible;
            }
            worksheet.Columns = JobTicketViewMetaData.KeyNames.Length; //限制表的长度
        }

        public void SetSearchCondition(string key,string value)
        {
            string name = (from kn in JobTicketViewMetaData.KeyNames
                           where kn.Key == key
                           select kn.Name).FirstOrDefault();
            if(name == null)
            {
                return;
            }
            for (int i = 0; i < this.comboBoxSearchCondition.Items.Count; i++) 
            {
                var item = comboBoxSearchCondition.Items[i];
                if (item.ToString() == name)
                {
                    this.comboBoxSearchCondition.SelectedIndex = i;
                }
            }
            this.textBoxSearchValue.Text = value;
        }

        private void Search()
        {
            string key = null;
            string value = null;

            if (this.comboBoxSearchCondition.SelectedIndex != 0)
            {
                key = (from kn in JobTicketViewMetaData.KeyNames
                       where kn.Name == this.comboBoxSearchCondition.SelectedItem.ToString()
                       select kn.Key).First();
                value = this.textBoxSearchValue.Text;
            }

            this.labelStatus.Text = "正在搜索中...";
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet[0, 0] = "加载中...";
            new Thread(new ThreadStart(() =>
            {
                JobTicketView[] jobTicketViews = null;
                string sql = "SELECT * FROM JobTicketView WHERE 1=1 ";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (this.projectID != -1)
                {
                    sql += "AND ProjectID = @projectID ";
                    parameters.Add(new SqlParameter("projectID", this.projectID));
                }
                if (warehouseID != -1)
                {
                    sql += "AND WarehouseID = @warehouseID ";
                    parameters.Add(new SqlParameter("warehouseID", this.warehouseID));
                }
                if (key != null && value != null) //查询条件不为null则增加查询条件
                {
                    sql += "AND " + key + " = @value ";
                    parameters.Add(new SqlParameter("value", value));
                }
                sql += " ORDER BY ID DESC"; //倒序排序
                try
                {
                    jobTicketViews = wmsEntities.Database.SqlQuery<JobTicketView>(sql, parameters.ToArray()).ToArray();
                }
                catch (EntityCommandExecutionException)
                {
                    MessageBox.Show("查询失败，请检查输入条件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("查询失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.reoGridControlMain.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    if (jobTicketViews.Length == 0)
                    {
                        worksheet[0, 1] = "没有查询到符合条件的记录";
                    }
                    for (int i = 0; i < jobTicketViews.Length; i++)
                    {
                        var curJobTicketViews = jobTicketViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curJobTicketViews, (from kn in JobTicketViewMetaData.KeyNames select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                }));
            })).Start();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FormJobTicketItem formJobTicketItem = new FormJobTicketItem(ids[0]);
            formJobTicketItem.SetJobTicketStateChangedCallback(new Action(() =>
            {
                this.Invoke(new Action(this.Search));
            }));
            formJobTicketItem.Show();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length == 0)
            {
                MessageBox.Show("请选择要删除的项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(MessageBox.Show("确定要删除选中项吗？","提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)!= DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除";
            new Thread(new ThreadStart(()=>
            {
                try
                {
                    foreach (int id in ids)
                    {
                        this.wmsEntities.Database.ExecuteSqlCommand(string.Format("DELETE FROM JobTicket WHERE ID = {0}", id));
                    }
                    this.wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(this.Search));
            })).Start();
        }

        private void buttonGeneratePutOutStorageTicket_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int jobTicketID = ids[0];

            FormPutOutStorageTicketNew form = new FormPutOutStorageTicketNew(jobTicketID, this.userID, this.projectID, this.warehouseID);
            form.SetToPutOutStorageTicketCallback(this.toPutOutStorageTicketCallback);
            form.Show();
            /*
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行操作","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = ids[0];

            new Thread(new ThreadStart(()=>
            {
                try
                {
                    JobTicket jobTicket = (from j in this.wmsEntities.JobTicket where j.ID == id select j).FirstOrDefault();
                    if (jobTicket == null)
                    {
                        MessageBox.Show("作业单不存在，请刷新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ShipmentTicket shipmentTicket = (from s in wmsEntities.ShipmentTicket where s.ID == jobTicket.ShipmentTicketID select s).FirstOrDefault();
                    if (shipmentTicket == null)
                    {
                        MessageBox.Show("对应发货单信息不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    shipmentTicket.State = ShipmentTicketViewMetaData.STRING_STATE_DELIVERING;
                    PutOutStorageTicket putOutStorageTicket = new PutOutStorageTicket();
                    wmsEntities.PutOutStorageTicket.Add(putOutStorageTicket);
                    putOutStorageTicket.ProjectID = this.projectID;
                    putOutStorageTicket.WarehouseID = this.warehouseID;
                    putOutStorageTicket.CreateUserID = this.userID;
                    putOutStorageTicket.CreateTime = DateTime.Now;
                    putOutStorageTicket.LastUpdateUserID = this.userID;
                    putOutStorageTicket.LastUpdateTime = DateTime.Now;
                    putOutStorageTicket.JobTicketID = id;
                    putOutStorageTicket.No = "";

                    foreach (JobTicketItem jobTicketItem in jobTicket.JobTicketItem)
                    {
                        PutOutStorageTicketItem putOutStorageTicketItem = new PutOutStorageTicketItem();
                        putOutStorageTicket.PutOutStorageTicketItem.Add(putOutStorageTicketItem);
                        putOutStorageTicketItem.StockInfoID = jobTicketItem.StockInfoID;
                    }

                    wmsEntities.SaveChanges();
                    putOutStorageTicket.No = Utilities.GenerateNo("C", putOutStorageTicket.ID);
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("生成出库单失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(this.Search));
                MessageBox.Show("操作成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })).Start();*/
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FormJobTicketModify formJobTicketModify = new FormJobTicketModify(this.userID,ids[0]);
            formJobTicketModify.SetModifyFinishedCallback(new Action(()=>
            {
                this.Search();
            }));
            formJobTicketModify.Show();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void comboBoxSearchCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.comboBoxSearchCondition.SelectedIndex == 0)
            {
                this.textBoxSearchValue.Text = "";
                this.textBoxSearchValue.Enabled = false;
            }
            else
            {
                this.textBoxSearchValue.Enabled = true;
            }
        }

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.Search();
            }
        }

        private void buttonToPutOutStorageTicket_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = ids[0];
            new Thread(() =>
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    JobTicket jobTicket = (from s in wmsEntities.JobTicket
                                                     where s.ID == id
                                                     select s).FirstOrDefault();
                    if (jobTicket == null)
                    {
                        MessageBox.Show("作业单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.toPutOutStorageTicketCallback(jobTicket.JobTicketNo);
                }
                catch (Exception)
                {
                    MessageBox.Show("查询失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }).Start();

        }
    }
}
