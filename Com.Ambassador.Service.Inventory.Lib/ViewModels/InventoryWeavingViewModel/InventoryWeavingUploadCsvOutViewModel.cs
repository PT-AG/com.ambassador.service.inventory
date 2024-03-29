﻿using Com.Ambassador.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Ambassador.Service.Inventory.Lib.ViewModels.InventoryWeavingViewModel
{
    public class InventoryWeavingUploadCsvOutViewModel : BasicViewModel
    {
        public string BonNo { get; set; }
        //public string Tanggal { get; set; }
        public string DestinationArea { get; set; }

        public string ReferenceNo { get; set; }
        public string Construction { get; set; }
        public string MaterialName { get; set; }
        public string WovenType { get; set; }
        public string Width { get; set; }
        public string Yarn1 { get; set; }
        public string Yarn2 { get; set; }
        public string YarnType1 { get; set; }
        public string YarnType2 { get; set; }
        public string YarnOrigin1 { get; set; }
        public string YarnOrigin2 { get; set; }
        public string ProductionOrderNo { get; set; }
        public string Grade { get; set; }
        public dynamic Piece { get; set; }
        public dynamic Qty { get; set; }
        public dynamic QtyPiece { get; set; }

        //public string Barcode { get; set; }
        //public DateTime ProductionOrderDate { get; set; }




    }
}
