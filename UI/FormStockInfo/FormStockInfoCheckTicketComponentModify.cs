﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormStockInfoCheckTicketComponentModify : Form
    {
        private FormMode mode = FormMode.ALTER;
        private int stockInfoCheckID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private int userID = -1;
        private int stockinfoid = -1;
        private int personid = -1;
        private Func<int> StockIDGetter = null;
        private WMS.DataAccess.WMSEntities wmsEntities = new WMS.DataAccess.WMSEntities();
        private Action modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private Action<int> SetSelectFinishedCallback = null;
        private   FormSelectStockInfo FormSelectStockInfo = null;
        private PagerWidget<WMS.DataAccess.StockInfoCheckTicketItemView > pagerWidget = null;


        //private Action checkFinishedCallback = null;




        public FormStockInfoCheckTicketComponentModify(int projectID, int warehouseID,int userID ,int personid ,int stockInfoCheckID=-1)
        {
            InitializeComponent();
            this.stockInfoCheckID = stockInfoCheckID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            //this.personid = personid;
            

        }

        private void FormStockCheckModify_Load(object sender, EventArgs e)
        {

            
            if (this.mode == FormMode.ALTER && this.stockInfoCheckID == -1)
            {
                throw new Exception("未设置源库存信息");
            }
            if(this.mode==FormMode.ADD||this.mode==FormMode.ALTER)
            {
                this.reoGridControlMain .Visible = false;
                this.buttonAdd.Visible = false;
                 
                this.buttonDelete.Visible = false;
                
                this.Size = new Size(500, 300);
                this.MinimizeBox = false;
                this.MaximizeBox = false;
               

            }
            if (this.mode == FormMode.ADD)
                {
                this.labelStatus.Text = "添加盘点单";

                 }
            if (this.mode == FormMode.ALTER)
            {
                this.labelStatus.Text = "修改盘点单";

            }
            if (this.mode==FormMode.CHECK)
            {
                
                this.labelStatus.Text = "盘点单条目";
                


            }
            
            Utilities.CreateEditPanel(this.tableLayoutPanel2, StockInfoCheckTicksModifyMetaDate.KeyNames);
            









            TextBox textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
            textBoxComponentName.BackColor = Color.White;








            TextBox textBoxPersonName =(TextBox )this.Controls.Find("textBoxPersonName", true)[0];
            textBoxPersonName.BackColor = Color.White;
            //this.StockIDGetter = Utilities.BindTextBoxSelect<FormSelectStockInfo, WMS.DataAccess.StockInfoView>(this, "textBoxComponentName", "ComponentName");
            this.Controls.Find("textBoxComponentName", true)[0].Click += textBoxComponentName_Click;
            this.Controls.Find("textBoxPersonName", true)[0].Click += textBoxPersonName_Click;



            this.Controls.Find("textBoxExcpetedOverflowAreaAmount", true)[0].TextChanged += textBoxExcpetedOverflowAreaAmount_TextChanged;
            this.Controls.Find("textBoxRealOverflowAreaAmount", true)[0].TextChanged += textBoxExcpetedOverflowAreaAmount_TextChanged;
            this.Controls.Find("textBoxExpectedShipmentAreaAmount", true)[0].TextChanged += textBoxExcpetedOverflowAreaAmount_TextChanged;
            this.Controls.Find("textBoxRealShipmentAreaAmount", true)[0].TextChanged += textBoxExcpetedOverflowAreaAmount_TextChanged;






            this.reoGridControlMain.Worksheets[0].SelectionRangeChanged += worksheet_SelectionRangeChanged;
            this.InitComponents();
            
            this.pagerWidget.ClearCondition();
            additionTextboxdiffrence();
            this.pagerWidget.AddCondition("StockInfoCheckTicketID", Convert .ToString ( this.stockInfoCheckID ));
            this.pagerWidget.Search();
            
            
             
        }










        private void additionTextboxdiffrence()
        {
            Label label = new Label();
            label.Text = "盘点差异值";
            label.Dock = DockStyle.Fill;
            label.Font = new Font("微软雅黑", 10);
            label.AutoSize = true;
            tableLayoutPanel2.Controls.Add(label);


            TextBox textBox = new TextBox();

            //textBox.Text = "自动填写";
            textBox.Font = new Font("微软雅黑", 10);
            textBox.Dock = DockStyle.Fill;
            textBox.Name = "textBoxDifference";

            //textBox.ForeColor = Color.DarkGray ;
            textBox.ReadOnly = true;
            tableLayoutPanel2.Controls.Add(textBox);
        }




        private void textBoxExcpetedOverflowAreaAmount_TextChanged(object sender, EventArgs e)
        {
            //string a = "";
            //foreach (Control control in this.tableLayoutPanel2.Controls)
            //{
            //    if (control is TextBox)
            //    {
            //        TextBox textBox = control as TextBox;
            //        a = a + textBox.Name;
            //        a = a + "   ";

            //    }
            //}







            TextBox textBoxExcpetedOverflowAreaAmount = (TextBox)this.Controls.Find("textBoxExcpetedOverflowAreaAmount", true)[0];
            TextBox textBoxRealOverflowAreaAmount = (TextBox)this.Controls.Find("textBoxRealOverflowAreaAmount", true)[0];
            TextBox textBoxExpectedShipmentAreaAmount = (TextBox)this.Controls.Find("textBoxExpectedShipmentAreaAmount", true)[0];
            TextBox textBoxRealShipmentAreaAmount = (TextBox)this.Controls.Find("textBoxRealShipmentAreaAmount", true)[0];

            TextBox textBoxtDifference = (TextBox)this.Controls.Find("textBoxDifference", true)[0];

            if (textBoxExcpetedOverflowAreaAmount.Text != "" && textBoxRealOverflowAreaAmount.Text != "" && textBoxExpectedShipmentAreaAmount.Text !="" && textBoxRealShipmentAreaAmount.Text != "")

                


            {
                decimal  ExcpetedOverflowAreaAmount = Convert.ToDecimal (textBoxExcpetedOverflowAreaAmount.Text);
               decimal  ExpectedShipmentAreaAmount = Convert.ToDecimal (textBoxExpectedShipmentAreaAmount.Text);
                decimal  RealOverflowAreaAmount= Convert.ToDecimal (textBoxRealOverflowAreaAmount.Text);
                decimal  RealShipmentAreaAmount = Convert.ToDecimal (textBoxRealShipmentAreaAmount.Text);


                textBoxtDifference.Text = (ExcpetedOverflowAreaAmount+ ExpectedShipmentAreaAmount-RealOverflowAreaAmount- RealShipmentAreaAmount).ToString();


            }





        }

        private void worksheet_SelectionRangeChanged(object sender, unvell.ReoGrid.Events.RangeEventArgs e)
        {
            this.RefreshTextBoxes();
        }


        private void textBoxPersonName_Click(object sender, EventArgs e)
        {
            var FormSelectPerson = new FormSelectPerson();


            FormSelectPerson.SetSelectFinishedCallback((selectedID) =>
            {

                var PersonName = (from s in wmsEntities.PersonView
                                  where s.ID == selectedID
                                  select s).FirstOrDefault();
                if (PersonName.Name == null)
                {
                    MessageBox.Show("选择人员信息失败，人员信息不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //this.supplierID = selectedID;
                //selectedID = 1;
                this.personid = selectedID;
                this.Controls.Find("textBoxPersonName", true)[0].Text = PersonName.Name;
                //this.personidc = 1;



            });
            FormSelectPerson.Show();


        }




        private void RefreshTextBoxes()
        {
            this.ClearTextBoxes();
            var worksheet = this.reoGridControlMain.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            
            if (ids.Length == 0)
            {

                this.buttonAdd.Text = "添加条目";
                this.stockinfoid = -1;
                
                //为编辑框填写默认值
                Utilities.FillTextBoxDefaultValues(this.tableLayoutPanel1, ShipmentTicketItemViewMetaData.KeyNames);

                return;
            }
            //this.buttonAdd.Text = "复制条目";
            int id = ids[0];
            WMS.DataAccess.StockInfoCheckTicketItemView StockInfoCheckTicketItem = null;
            

            try
            {
                WMS.DataAccess.WMSEntities wmsEntities = new WMS.DataAccess.WMSEntities();
                StockInfoCheckTicketItem = (from s in wmsEntities.StockInfoCheckTicketItemView
                                            where s.ID == id
                                            select s).FirstOrDefault();
            } 
            catch
            {
                MessageBox.Show("刷新数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (StockInfoCheckTicketItem == null)
            {
                MessageBox.Show("系统错误，未找到相应盘点单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (StockInfoCheckTicketItem.StockInfoID != null)
            {
                this.stockinfoid  = StockInfoCheckTicketItem.StockInfoID.Value;
            }
            else
            {
                this.stockinfoid = -1;
            }
            if(StockInfoCheckTicketItem .PersonID !=null)
            {

                this.personid = StockInfoCheckTicketItem.PersonID.Value ;

            }




            //if (StockInfoCheckTicketItem.PersonID  != null)
            //{
            //    this.personid  = StockInfoCheckTicketItem.PersonID.Value;
            //}
            //else
            //{
            //    this.personid  = -1;
            //}




            //TextBox textbox1 = (TextBox)this.Controls.Find("textBox", true)[0];

            Utilities.CopyPropertiesToTextBoxes(StockInfoCheckTicketItem, this);
            Utilities.CopyPropertiesToComboBoxes(StockInfoCheckTicketItem, this);

        }

        private void ClearTextBoxes()
        {
            foreach (Control control in this.tableLayoutPanel2.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.Text = "";
                }
            }



        }












        private void InitComponents()
        {
            //string[] visibleColumnNames = (from kn in StockInfoCheckTicksModifyMetaDate.KeyNames
            //                               where kn.Visible == true
            //                               select kn.Name).ToArray();



            ////初始化表格
            //var worksheet = this.reoGridControlMain.Worksheets[0];
            //worksheet.SelectionMode = unvell.ReoGrid.WorksheetSelectionMode.Row;
            //for (int i = 0; i < StockInfoCheckTicksModifyMetaDate.KeyNames.Length; i++)
            //{
            //    worksheet.ColumnHeaders[i].Text = StockInfoCheckTicksModifyMetaDate.KeyNames[i].Name;
            //    worksheet.ColumnHeaders[i].IsVisible = StockInfoCheckTicksModifyMetaDate.KeyNames[i].Visible;
            //}
            //worksheet.Columns = StockInfoCheckTicksModifyMetaDate.KeyNames.Length;//限制表的长度
            //worksheet.RowCount = 10;

            try
            {
                WMS.DataAccess.WMSEntities wmsEntities = new DataAccess.WMSEntities ();
                WMS.DataAccess.User user = (from u in wmsEntities.User where u.ID == this.userID select u).FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("登录失效，请重新登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //如果登录失效，不能查出任何数据。                   
                    return;
                }                
            }
            catch
            {
                MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //如果加载失败，不能查出任何数据。
                return;
            }
            //初始化分页控件
            this.pagerWidget = new PagerWidget<WMS.DataAccess.StockInfoCheckTicketItemView>(this.reoGridControlMain, StockInfoCheckTicksModifyMetaDate.KeyNames, -1, -1);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();



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


        private void textBoxComponentName_Click(object sender, EventArgs e)


        {
            if (this.FormSelectStockInfo == null)
            { this.FormSelectStockInfo = new FormSelectStockInfo();

            }
            
            
            //this.projectID,this.warehouseID,this.stockinfoid );
            this.FormSelectStockInfo.SetSelectFinishedCallback((selectedID) =>
            {
                 
                wmsEntities = new DataAccess.WMSEntities();
                WMS.DataAccess.StockInfoView stockinfoName = new DataAccess.StockInfoView();
                try
                {
                    stockinfoName = (from s in wmsEntities.StockInfoView
                                     where s.ID == selectedID
                                     select s).FirstOrDefault();

                }


                catch
                {
                    MessageBox.Show("选择库存信息失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


                }
                if (stockinfoName.ComponentName == null)
                    {
                        MessageBox.Show("选择库存信息失败，库存信息不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                
                
                //this.supplierID = selectedID;
                //selectedID = 1;
                this.stockinfoid = selectedID;
                this.Controls.Find("textBoxComponentName", true)[0].Text = stockinfoName.ComponentName;
                this.Controls.Find("textBoxSupplierName", true)[0].Text = stockinfoName.SupplierName;
                this.Controls.Find("textBoxExcpetedOverflowAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.OverflowAreaAmount);
                this.Controls.Find("textBoxExpectedShipmentAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.ShipmentAreaAmount);


                this.Controls.Find("textBoxExpectedRejectAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.RejectAreaAmount );
                this.Controls.Find("textBoxExpectedReceiptAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.ReceiptAreaAmount  );
                this.Controls.Find("textBoxExpectedSubmissionAmount", true)[0].Text = Convert.ToString(stockinfoName.SubmissionAmount );

                
               this.Controls.Find("textBoxRealRejectAreaAmount", true)[0].Text =Convert .ToString(stockinfoName.RejectAreaAmount);
                this.Controls.Find("textBoxRealReceiptAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.ReceiptAreaAmount);
                this.Controls.Find("textBoxRealSubmissionAmount", true)[0].Text=Convert .ToString (stockinfoName .SubmissionAmount) ;
                this.Controls.Find("textBoxRealOverflowAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.OverflowAreaAmount) ;
                this.Controls.Find("textBoxRealShipmentAreaAmount", true)[0].Text = Convert.ToString(stockinfoName.ShipmentAreaAmount);
                TextBox textBoxRealShipmentAreaAmount = (TextBox)this.Controls.Find("textBoxRealShipmentAreaAmount", true)[0];
                this.Controls.Find("textBoxPersonName", true)[0].Text = "";


            });
            FormSelectStockInfo.ShowDialog();






        }










        public void SetModifyFinishedCallback(Action callback)
        {
            this.modifyFinishedCallback = callback;
        }




        public void SetAddFinishedCallback(Action<int> callback)
        {
            this.addFinishedCallback = callback;
        }
     


        

        private void buttonAdd_Click(object sender, EventArgs e)
        {


            decimal  tmp = 0;
           DataAccess.StockInfoCheckTicketItem StockInfoCheckTicketItem = null;
           TextBox textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
           TextBox textBoxRealRejectAreaAmount = (TextBox)this.Controls.Find("textBoxRealRejectAreaAmount", true)[0];
           TextBox textBoxRealReceiptAreaAmount = (TextBox)this.Controls.Find("textBoxRealReceiptAreaAmount", true)[0];
           TextBox textBoxRealSubmissionAmount = (TextBox)this.Controls.Find("textBoxRealSubmissionAmount", true)[0];
           TextBox textBoxRealOverflowAreaAmount = (TextBox)this.Controls.Find("textBoxRealOverflowAreaAmount", true)[0];
           TextBox textBoxRealShipmentAreaAmount = (TextBox)this.Controls.Find("textBoxRealShipmentAreaAmount", true)[0];
           




            if (textBoxComponentName.Text == string.Empty)
            {

                MessageBox.Show("请选择零件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBoxRealRejectAreaAmount.Text !=string .Empty&&!decimal.TryParse(textBoxRealRejectAreaAmount.Text, out tmp))
            {
                MessageBox.Show("实际不良品区数量只接受数字类型");
                return;
            }
            if (textBoxRealReceiptAreaAmount.Text != string.Empty && !decimal.TryParse(textBoxRealReceiptAreaAmount.Text, out tmp))
            {
                MessageBox.Show("实际收货区数量只接受数字类型");
                return;
            }

            if (textBoxRealSubmissionAmount.Text != string.Empty && !decimal.TryParse(textBoxRealSubmissionAmount.Text, out tmp))
            {
                MessageBox.Show("实际送检数量只接受数字类型");
                return;
            }

            if (textBoxRealOverflowAreaAmount.Text != string.Empty && !decimal.TryParse(textBoxRealOverflowAreaAmount.Text, out tmp))
            {
                MessageBox.Show("实际溢库区数量只接受数字类型");
                return;
            }
            if (textBoxRealShipmentAreaAmount.Text != string.Empty && !decimal.TryParse(textBoxRealShipmentAreaAmount.Text, out tmp))
            {
                MessageBox.Show("实际发货区数量只接受数字类型");
                return;
            }

            






            StockInfoCheckTicketItem = new DataAccess.StockInfoCheckTicketItem();
            this.wmsEntities.StockInfoCheckTicketItem.Add(StockInfoCheckTicketItem);


            StockInfoCheckTicketItem.StockInfoCheckTicketID = this.stockInfoCheckID ;



            StockInfoCheckTicketItem.StockInfoID = this.stockinfoid;
            StockInfoCheckTicketItem.PersonID = this.personid;
            

            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, StockInfoCheckTicketItem, StockInfoCheckTicksModifyMetaDate.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            wmsEntities.SaveChanges();
            this.labelStatus.Text = "正在添加";
            MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            

            this.Search(StockInfoCheckTicketItem.ID);
            this.labelStatus.Text = "盘点单条目";
            




            if (this.mode == FormMode.CHECK && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(this.stockInfoCheckID);
            }
            //Utilities.CreateEditPanel(this.tableLayoutPanel2, StockInfoCheckTicksModifyMetaDate.KeyNames);
            
            //this.Controls.Find("textBoxComponentName", true)[0].Click += textBoxComponentName_Click;
            //this.Controls.Find("textBoxPersonName", true)[0].Click += textBoxPersonName_Click;







        }




        public void SetMode(FormMode mode)
        {
            this.mode = mode;
            if (mode == FormMode.ALTER)
            {
                this.Text = "修改盘点单信息";
                
               
            }
            else if (mode == FormMode.ADD)
            {
                this.Text = "添加盘点单信息";
               
               
            }
            else if (mode == FormMode.CHECK)
                this.Text = "盘点单条目";
                

        }

        private void Search(int selectID=-1)
        {
           this.pagerWidget.Search(false, selectID);
            this.labelStatus.Text = "盘点单条目";
            //var worksheet = this.reoGridControlMain.Worksheets[0];

            //worksheet[0, 1] = "加载中...";
            //new Thread(new ThreadStart(() => 
            //{
            //    WMS.DataAccess.StockInfoCheckTicketItemView[] shipmentTicketItemViews = null;
            //    try
            //    {
            //        shipmentTicketItemViews = (from s in wmsEntities.StockInfoCheckTicketItemView 
            //                                   where s.StockInfoCheckTicketID   == this.stockInfoCheckID 
            //                                   orderby s.ID descending
            //                                   select s).ToArray();
            //    }
            //    catch
            //    {
            //        MessageBox.Show("查询数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        if (this.IsDisposed == false)
            //        {
            //            this.Invoke(new Action(this.Close));
            //        }
            //        return;
            //    }

            //    this.Invoke(new Action(() =>
            //    {
            //        this.labelStatus.Text = "加载完成";
            //        worksheet.DeleteRangeData(RangePosition.EntireRange);
            //        if (shipmentTicketItemViews.Length == 0)
            //        {
            //            worksheet[0, 1] = "没有符合条件的记录";
            //        }


            //        if (shipmentTicketItemViews.Length > worksheet.RowCount)

            //        {
            //            worksheet.AppendRows(  shipmentTicketItemViews.Length- worksheet.RowCount);
            //        }

            //        for (int i = 0; i < shipmentTicketItemViews.Length; i++)
            //        {
            //            var curShipmentTicketViews = shipmentTicketItemViews[i];
            //            object[] columns = Utilities.GetValuesByPropertieNames(curShipmentTicketViews, (from kn in StockInfoCheckTicksModifyMetaDate.KeyNames select kn.Key).ToArray());
            //            for (int j = 0; j < columns.Length; j++)
            //            {
            //                worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
            //            }
            //        }







            //if (selectID != -1)
            //{
            //    Utilities.SelectLineByID(this.reoGridControlMain, selectID);
            //}
            //this.Invoke(new Action(this.RefreshTextBoxes));
            //    }));
            //})).Start();


        }

        


        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                var worksheet = this.reoGridControlMain.Worksheets[0];

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
                foreach (int id in deleteIDs)
                {
                    this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM StockInfoCheckTicketItem WHERE ID = @stockCheckID", new SqlParameter("stockCheckID", id));
                }
                this.wmsEntities.SaveChanges();
                
            })).Start();
            this.Invoke(new Action(() =>
            {
                this.pagerWidget.Search();
            //this.Search();
            this.labelStatus.Text = "盘点单条目";
            }));

            //this.labelStatus.Text = "盘点单条目";


            //WMS.DataAccess.StockInfoCheckTicket stockInfoCheck = null;
            //try
            //{
            //    WMS.DataAccess.WMSEntities wmsEntities = new WMS.DataAccess.WMSEntities();
            //    stockInfoCheck = (from s in wmsEntities.StockInfoCheckTicket
            //                      where s.ID == this.stockInfoCheckID
            //                      select s).FirstOrDefault ();


            //}
            //catch
            //{
            //    MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    this.Close();
            //    return;
            //}
            //if (stockInfoCheck == null)
            //{
            //    MessageBox.Show("要删除的项目已不存在，请确认后操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    return;
            //}

            //try
            //{
            //    stockInfoCheck.LastUpdateUserID = Convert.ToString(userID);
            //    stockInfoCheck.LastUpdateTime = DateTime.Now;
            //    wmsEntities.SaveChanges();
            //}
            //catch
            //{
            //    MessageBox.Show("更新时间操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            if (this.mode == FormMode.CHECK && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(this.stockInfoCheckID);
            }

            })).Start();
        }

       

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            new Thread(new ThreadStart(() =>
            {
                int id = ids[0];
                WMS .DataAccess .StockInfoCheckTicketItem StockInfoCheckTicketItem = null;
                wmsEntities = new WMS.DataAccess.WMSEntities();
                try
                {
                    StockInfoCheckTicketItem = (from s in this.wmsEntities.StockInfoCheckTicketItem  where s.ID == id select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("查找盘点单条目操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (StockInfoCheckTicketItem == null)
                {
                    MessageBox.Show("盘点单条目不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

           
               
                StockInfoCheckTicketItem.PersonID  = this.personid == -1 ? null : (int?)this.personid;
                StockInfoCheckTicketItem.StockInfoID = this.stockinfoid == -1 ? null : (int?)this.stockinfoid;









                if (Utilities.CopyTextBoxTextsToProperties(this, StockInfoCheckTicketItem, StockInfoCheckTicksModifyMetaDate.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    



                    this.wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("修改信息操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    this.Search(StockInfoCheckTicketItem .ID );
                    Utilities.SelectLineByID(this.reoGridControlMain, StockInfoCheckTicketItem.ID);
                }));
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })).Start();



            ////更改盘点信息

            //WMS.DataAccess.StockInfoCheckTicket stockInfoCheck = null;
            //try
            //{
            //    wmsEntities = new DataAccess.WMSEntities();
            //    stockInfoCheck = (from s in wmsEntities.StockInfoCheckTicket
            //                      where s.ID == this.stockInfoCheckID
            //                      select s).FirstOrDefault();


            //}
            //catch
            //{
            //    MessageBox.Show("加载盘点单数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    this.Close();
            //    return;
            //}
            //if (stockInfoCheck == null)
            //{
            //    MessageBox.Show("要修改的项目已不存在，请确认后操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    return;
            //}
            //stockInfoCheck.LastUpdateUserID = Convert.ToString(userID);
            //stockInfoCheck.LastUpdateTime = DateTime.Now;
            //try
            //{

            //    wmsEntities.SaveChanges();

            //}
            //catch
            //{
            //    MessageBox.Show("修改盘点单操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
























            if (this.mode == FormMode.CHECK && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(this.stockInfoCheckID);
            }














        }

        private void buttonAdd_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonAdd_MouseEnter(object sender, EventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAdd_MouseLeave(object sender, EventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonAlter_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAlter.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonAlter_MouseEnter(object sender, EventArgs e)
        {
            buttonAlter.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAlter_MouseLeave(object sender, EventArgs e)
        {
            buttonAlter.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonDelete_MouseDown(object sender, MouseEventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonDelete_MouseEnter(object sender, EventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonDelete_MouseLeave(object sender, EventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonAddAll_Click(object sender, EventArgs e)
        {
            WMS.DataAccess.StockInfo[] stockinfoall = null;
            WMS.DataAccess.StockInfoCheckTicketItem [] StockInfoCheckTicketItemsave = null;
            
            try 
            {
                wmsEntities = new DataAccess.WMSEntities();
                stockinfoall = (from kn in wmsEntities.StockInfo
                                 where kn.ProjectID ==this.projectID &&kn.WarehouseID ==this.warehouseID 
                                    select kn).ToArray();

                StockInfoCheckTicketItemsave= (from kn in wmsEntities.StockInfoCheckTicketItem 
                                           where kn.StockInfoCheckTicketID ==this.stockInfoCheckID 
                                           select kn).ToArray();





            }
            catch
            {
                MessageBox.Show("添加所有条目操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

            }

            if (stockinfoall.Length ==0)
            {
                MessageBox.Show("库存信息为空，无法添加条目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i=0;i<stockinfoall .Length;i++)
            {
                bool repet = false;
                var  StockInfoCheckTicketItem = new DataAccess.StockInfoCheckTicketItem();


                try
                {
                    this.wmsEntities.StockInfoCheckTicketItem.Add(StockInfoCheckTicketItem);
                }

                catch
                {
                    MessageBox.Show("添加所有条目操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                for(int j=0;j< StockInfoCheckTicketItemsave.Length;j++)
                {
                    if (StockInfoCheckTicketItemsave[j].StockInfoID == stockinfoall[i].ID )
                    {
                        repet = true;

                   }

                }

                if(repet ==true )
                { continue; }

                StockInfoCheckTicketItem.StockInfoID = stockinfoall[i].ID;
                //StockInfoCheckTicketItem.PersonID = this.personid;

                StockInfoCheckTicketItem.StockInfoCheckTicketID = this.stockInfoCheckID;
                StockInfoCheckTicketItem.ExcpetedOverflowAreaAmount = stockinfoall[i].OverflowAreaAmount;
                StockInfoCheckTicketItem.ExpectedReceiptAreaAmount = stockinfoall[i].ReceiptAreaAmount ;
                StockInfoCheckTicketItem.ExpectedRejectAreaAmount = stockinfoall[i].RejectAreaAmount ;
                StockInfoCheckTicketItem.ExpectedShipmentAreaAmount = stockinfoall[i].ShipmentAreaAmount;
                StockInfoCheckTicketItem.ExpectedSubmissionAmount = stockinfoall[i].SubmissionAmount;
                StockInfoCheckTicketItem.RealOverflowAreaAmount = stockinfoall[i].OverflowAreaAmount;
                StockInfoCheckTicketItem.RealReceiptAreaAmount = stockinfoall[i].ReceiptAreaAmount;
                StockInfoCheckTicketItem.RealRejectAreaAmount = stockinfoall[i].RejectAreaAmount;
                StockInfoCheckTicketItem.RealShipmentAreaAmount = stockinfoall[i].ShipmentAreaAmount;
                StockInfoCheckTicketItem.RealSubmissionAmount = stockinfoall[i].SubmissionAmount;



                try
                {
                    wmsEntities.SaveChanges();

                }
                catch
                {
                    MessageBox.Show("添加所有条目操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



              

            }

            MessageBox.Show("添加所有条目成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Search();
            if (this.mode == FormMode.CHECK && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(this.stockInfoCheckID);
            }



        }

        private void buttonAddAll_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAddAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonAddAll_MouseEnter(object sender, EventArgs e)
        {
            buttonAddAll.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAddAll_MouseLeave(object sender, EventArgs e)
        {
            buttonAddAll.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }




    } 
    
}
