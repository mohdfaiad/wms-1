﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class PutOutStorageTicketItemViewMetaData
    {
        public const string STRING_STATE_WAIT_FOR_LOAD = "待装车";
        public const string STRING_STATE_PART_LOAD = "部分装车";
        public const string STRING_STATE_ALL_LOAD = "全部装车";

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            //new KeyName(){Key="StockInfoID",Name="库存信息ID",Editable=true},
            //new KeyName(){Key="PutOutStorageTicketNo",Name="出库单号",Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件",Editable=false,Save=false},
            new KeyName(){Key="SupplierName",Name="供应商",Editable=false,Save=false},
            new KeyName(){Key="ScheduledAmount",Name="装车数量",Editable=false,Save=false},
            new KeyName(){Key="RealAmount",Name="实际装车数量"},
            new KeyName(){Key="ExceedStockAmount",Name="超库存数量",Editable=true},
            new KeyName(){Key="Unit",Name="单位",Editable=false,Save=false },
            new KeyName(){Key="State",Name="状态",Editable=false,ComboBoxItems=new ComboBoxItem[]{
                new ComboBoxItem(STRING_STATE_WAIT_FOR_LOAD),
                new ComboBoxItem(STRING_STATE_PART_LOAD),
                new ComboBoxItem(STRING_STATE_ALL_LOAD)
            } },
            new KeyName(){Key="JobPersonName",Name="实际作业人员",Save=false,EditPlaceHolder="点击选择人员"},
            new KeyName(){Key="ConfirmPersonName",Name="确认人",Save=false,EditPlaceHolder="点击选择人员"},
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
