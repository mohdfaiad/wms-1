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
    
    public partial class ReceiptTicketItemView
    {
        public int ReceiptTicketItemReceiptTicketID { get; set; }
        public int ReceiptTicketItemComponentID { get; set; }
        public string ReceiptTicketItemPackageName { get; set; }
        public Nullable<decimal> ReceiptTicketItemExpectedPackageAmount { get; set; }
        public Nullable<decimal> ReceiptTicketItemExpectedAmount { get; set; }
        public Nullable<decimal> ReceiptTicketItemReceiviptAmount { get; set; }
        public Nullable<decimal> ReceiptTicketItemWrongComponentAmount { get; set; }
        public Nullable<decimal> ReceiptTicketItemShortageAmount { get; set; }
        public Nullable<decimal> ReceiptTicketItemDisqualifiedAmount { get; set; }
        public string ReceiptTicketItemManufactureNo { get; set; }
        public Nullable<System.DateTime> ReceiptTicketItemInventoryDate { get; set; }
        public Nullable<System.DateTime> ReceiptTicketItemManufactureDate { get; set; }
        public Nullable<System.DateTime> ReceiptTicketItemExpiryDate { get; set; }
        public string ReceiptTicketItemRealRightProperty { get; set; }
        public string ReceiptTicketItemBoxNo { get; set; }
        public Nullable<int> ComponentProjectID { get; set; }
        public Nullable<int> ComponentWarehouseID { get; set; }
        public Nullable<int> ComponentSupplierID { get; set; }
        public string ComponentContainerNo { get; set; }
        public string ComponentFactroy { get; set; }
        public string ComponentWorkPosition { get; set; }
        public string ComponentNo { get; set; }
        public string ComponentName { get; set; }
        public string ComponentSupplierType { get; set; }
        public string ComponentType { get; set; }
        public string ComponentSize { get; set; }
        public string ComponentCategory { get; set; }
        public string ComponentGroupPrincipal { get; set; }
        public Nullable<decimal> ComponentSingleCarUsageAmount { get; set; }
        public Nullable<decimal> ComponentCharge1 { get; set; }
        public Nullable<decimal> ComponentCharge2 { get; set; }
        public Nullable<decimal> ComponentInventoryRequirement1Day { get; set; }
        public Nullable<decimal> ComponentInventoryRequirement3Day { get; set; }
        public Nullable<decimal> ComponentInventoryRequirement5Day { get; set; }
        public Nullable<decimal> ComponentInventoryRequirement10Day { get; set; }
        public Nullable<System.DateTime> ReceiptTicketVoucherYear { get; set; }
        public string ReceiptTicketReletedVoucherNo { get; set; }
        public string ReceiptTicketReletedVoucherLineNo { get; set; }
        public Nullable<System.DateTime> ReceiptTicketReletedVoucherYear { get; set; }
        public string ReceiptTicketHeadingText { get; set; }
        public Nullable<System.DateTime> ReceiptTicketPostCountDate { get; set; }
        public string ReceiptTicketInwardDeliverTicketNo { get; set; }
        public string ReceiptTicketInwardDeliverLineNo { get; set; }
        public string ReceiptTicketOutwardDeliverTicketNo { get; set; }
        public string ReceiptTicketOutwardDeliverLineNo { get; set; }
        public string ReceiptTicketPurchaseTicketNo { get; set; }
        public string ReceiptTicketPurchaseTicketLineNo { get; set; }
        public Nullable<System.DateTime> ReceiptTicketOrderDate { get; set; }
        public string ReceiptTicketReceiptStorageLocation { get; set; }
        public string ReceiptTicketBoardNo { get; set; }
        public string ReceiptTicketReceiptPackage { get; set; }
        public Nullable<decimal> ReceiptTicketExpectedAmount { get; set; }
        public Nullable<decimal> ReceiptTicketReceiptCount { get; set; }
        public string ReceiptTicketMoveType { get; set; }
        public string ReceiptTicketSource { get; set; }
        public string ReceiptTicketAssignmentPerson { get; set; }
        public Nullable<int> ReceiptTicketPostedCount { get; set; }
        public string ReceiptTicketBoxNo { get; set; }
        public Nullable<int> ReceiptTicketCreateUserID { get; set; }
        public Nullable<System.DateTime> ReceiptTicketCreateTime { get; set; }
        public Nullable<int> ReceiptTicketLastUpdateUserID { get; set; }
        public Nullable<System.DateTime> ReceiptTicketLastUpdateTime { get; set; }
        public string ProjectName { get; set; }
        public string WarehouseName { get; set; }
        public string CreateUserUsername { get; set; }
        public string CreateUserPassword { get; set; }
        public Nullable<int> CreateUserAuthority { get; set; }
        public string CreateUserAuthorityName { get; set; }
        public Nullable<int> CreateUserSupplierID { get; set; }
        public string LastUpdateUserUsername { get; set; }
        public string LastUpdateUserPassword { get; set; }
        public Nullable<int> LastUpdateUserAuthority { get; set; }
        public string LastUpdateUserAuthorityName { get; set; }
        public Nullable<int> LastUpdateUserSupplierID { get; set; }
    }
}
