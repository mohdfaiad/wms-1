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
    
    public partial class StockInfoCheckTicketItemView
    {
        public int ID { get; set; }
        public Nullable<int> StockInfoID { get; set; }
        public Nullable<decimal> ExcpetedOverflowAreaAmount { get; set; }
        public Nullable<decimal> ExpectedShipmentAreaAmount { get; set; }
        public Nullable<decimal> RealOverflowAreaAmount { get; set; }
        public Nullable<decimal> RealShipmentAreaAmount { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> StockInfoCheckTicketID { get; set; }
        public string ComponentNo { get; set; }
        public string ComponentName { get; set; }
        public string ComponentNumber { get; set; }
        public string SupplierNumber { get; set; }
        public Nullable<decimal> ExpectedRejectAreaAmount { get; set; }
        public Nullable<decimal> RealRejectAreaAmount { get; set; }
        public Nullable<decimal> ExpectedReceiptAreaAmount { get; set; }
        public Nullable<decimal> RealReceiptAreaAmount { get; set; }
        public Nullable<decimal> ExpectedSubmissionAmount { get; set; }
        public Nullable<decimal> RealSubmissionAmount { get; set; }
        public Nullable<int> PersonID { get; set; }
    }
}
