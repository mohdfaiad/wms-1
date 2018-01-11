﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WMS.UI.FormReceipt;
using WMS.UI.FormBase;
using WMS.DataAccess;

namespace WMS.UI
{



    public partial class FormSupplierRemind : Form
    {


        private int supplierid;
        private WMSEntities wmsEntities = new WMSEntities();
        
        private DateTime contract_enddate;
        //private TimeSpan days;
        private int days;

        public FormSupplierRemind(int supplierid)
        {
            InitializeComponent();

            this.supplierid = supplierid;
        }





        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormSupplierRemind_Load(object sender, EventArgs e)
        {


            this.textBoxContractRemind.Text = " 无合同过期提醒";
            this.textBoxContractRemind.Font = new Font("宋体", 12, FontStyle.Bold);
            this.textBox3.Text = "无库存预警";
            this.textBox3 .Font = new Font("宋体", 12, FontStyle.Bold);
            contractrenmind();

            componentremind();
            



        }








        private void contractrenmind()


        {
            Supplier Supplier = new Supplier();

            Supplier = (from u in this.wmsEntities.Supplier
                        where u.ID == supplierid
                        select u).Single();





            if (Convert.ToString(Supplier.EndingTime) != string.Empty)
            {
                this.contract_enddate = Convert.ToDateTime(Supplier.EndingTime);
                days = (DateTime.Now - contract_enddate).Days;

                if ((-days) < 10)
                {

                    this.textBoxContractRemind.Text= "您的合同还有" + (-days) + "天就到期了";
                    

                }




                if (Supplier.EndingTime < DateTime.Now)
                {
                    this.textBoxContractRemind.Text = " 您的合同已经到截止日期";
                    this.textBoxContractRemind.Font = new Font("宋体", 12, FontStyle.Bold);

                }



            }


        }





        private void componentremind()
        {

            int[] warringdays = { 3, 5, 10 };
            int reminedays;
            
            var ShipmentAreaAmount = (from u in wmsEntities.StockInfoView
                       where u.ReceiptTicketSupplierID ==
                       this.supplierid
                       select u.ShipmentAreaAmount ).ToArray();
            var ComponentName = (from u in wmsEntities.StockInfoView
                           where u.ReceiptTicketSupplierID == supplierid
                           select u.ComponentName).ToArray();
            int[] singlecaramount = new int[ComponentName.Length];
            int[] dailyproduction = new int[ComponentName.Length];

            for (int i=0; i<ComponentName .Length;i++ )

            {
                




                var compon = (from u in wmsEntities.ComponentView
                              where u.Name == ComponentName[i]
                              select u).Single();

                singlecaramount [i] = Convert .ToInt32 ( compon.SingleCarUsageAmount);
                dailyproduction[i] = Convert.ToInt32(compon.DailyProduction);
                if(ShipmentAreaAmount[i] == 0||singlecaramount [i]==0||dailyproduction[0] == 0)
                {
                    continue;
                }

                reminedays = Convert.ToInt32(ShipmentAreaAmount[i]) / (singlecaramount[i] * dailyproduction[i]);



                if(reminedays <10)
                {
                    this.textBox3.AppendText ( "零件" + ComponentName[i] + " 大约仅可以用" + reminedays + "天");
                }
                    




            }





        }







            private void button1_Click(object sender, EventArgs e)
            {
                this.Close();
            }
        
    }
}