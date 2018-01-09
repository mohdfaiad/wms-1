﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class StockInfoCheckTicksModifyMetaDate
    {
        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件",Editable=false,Save=false,EditPlaceHolder ="点击选择零件"},
            new KeyName(){Key="SupplierName",Name="供应商",Editable=false,Save=false},
            new KeyName(){Key="ExpectedRejectAreaAmount",Name="不良品区数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealRejectAreaAmount",Name="实际不良品区数量",Editable=true  ,Save=true  },
            new KeyName(){Key="ExpectedReceiptAreaAmount",Name="收货区数量",Editable=false  ,Save=true  },
            new KeyName(){Key="RealReceiptAreaAmount",Name="实际收货区数量",Editable=true  ,Save=true  },
            new KeyName(){Key="ExpectedSubmissionAmount",Name="送检数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealSubmissionAmount",Name="实际送检数量",Editable=true  ,Save=true  },





            new KeyName(){Key="ExcpetedOverflowAreaAmount",Name="溢库区数量",Editable=false ,Save=true  },
           

            new KeyName(){Key="RealOverflowAreaAmount",Name="实际溢库区数量",Editable=true ,Save=true }
            ,
             new KeyName(){Key="ExpectedShipmentAreaAmount",Name="发货区数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealShipmentAreaAmount",Name="实际发货区数量",Editable=true ,Save=true }
            ,


        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
