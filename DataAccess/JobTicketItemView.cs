//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobTicketItemView
    {
        public int ID { get; set; }
        public int JobTicketID { get; set; }
        public Nullable<int> StockInfoID { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> HappenTime { get; set; }
        public string SupplierName { get; set; }
        public string JobTicketJobTicketNo { get; set; }
        public Nullable<int> ReceiptTicketItemReceiptTicketID { get; set; }
        public string ComponentName { get; set; }
        public string SupplierNumber { get; set; }
        public string JobPersonName { get; set; }
        public string ConfirmPersonName { get; set; }
        public Nullable<int> PersonID { get; set; }
        public Nullable<decimal> ScheduledAmount { get; set; }
        public Nullable<decimal> RealAmount { get; set; }
        public string Unit { get; set; }
        public Nullable<int> JobPersonID { get; set; }
        public Nullable<int> ConfirmPersonID { get; set; }
        public Nullable<decimal> ScheduledPutOutAmount { get; set; }
        public Nullable<decimal> UnitAmount { get; set; }
        public string SupplyNumber { get; set; }
        public string SupplyNo { get; set; }
        public Nullable<int> ShipmentTicketItemID { get; set; }
        public Nullable<System.DateTime> StockInfoInventoryDate { get; set; }
        public Nullable<System.DateTime> StockInfoManufactureDate { get; set; }
        public Nullable<System.DateTime> StockInfoExpiryDate { get; set; }
        public string StockInfoReceiptTicketNo { get; set; }
        public Nullable<int> SupplyID { get; set; }
        public Nullable<int> ComponentID { get; set; }
    }
}
