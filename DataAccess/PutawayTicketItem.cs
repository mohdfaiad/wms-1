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
    
    public partial class PutawayTicketItem
    {
        public int ID { get; set; }
        public int PutawayTicketID { get; set; }
        public string DisplacementPositionNo { get; set; }
        public string TargetStorageLocation { get; set; }
        public string BoardNo { get; set; }
        public Nullable<decimal> ScheduledMoveCount { get; set; }
        public Nullable<decimal> DistrabuteCount { get; set; }
        public Nullable<decimal> MoveCount { get; set; }
        public string State { get; set; }
        public string OperatePerson { get; set; }
        public string OperateTime { get; set; }
        public int ReceiptTicketItemID { get; set; }
    
        public virtual PutawayTicket PutawayTicket { get; set; }
        public virtual ReceiptTicketItem ReceiptTicketItem { get; set; }
    }
}
