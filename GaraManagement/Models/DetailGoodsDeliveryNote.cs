﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsDeliveryNote
    {
        public int IdGoodsDeliveryNote { get; set; }
        public int IdMaterial { get; set; }
        [DisplayName("Số lượng xuất")]
        public int? Amount { get; set; }

        public virtual GoodsDeliveryNote IdGoodsDeliveryNoteNavigation { get; set; }
        public virtual Material IdMaterialNavigation { get; set; }
    }
}
