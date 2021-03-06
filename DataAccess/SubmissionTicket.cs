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
    
    public partial class SubmissionTicket
    {
        public SubmissionTicket()
        {
            this.SubmissionTicketItem = new HashSet<SubmissionTicketItem>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ReceiptTicketID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string No { get; set; }
        public string HasSelfInspectionReport { get; set; }
        public Nullable<int> ReceivePersonID { get; set; }
        public string Result { get; set; }
        public string State { get; set; }
        public Nullable<int> LastUpdateUserID { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public Nullable<int> SubmissionPersonID { get; set; }
        public Nullable<int> DeliverSubmissionPersonID { get; set; }
        public Nullable<System.DateTime> SubmissionDate { get; set; }
        public Nullable<System.DateTime> PaintTime { get; set; }
        public string SAPNo { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ICollection<SubmissionTicketItem> SubmissionTicketItem { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
