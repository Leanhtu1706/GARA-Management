using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsDeliveryNote
    {
        public string IdGoodsDeliveryNote { get; set; }
        public string IdSupplies { get; set; }
        public int? Amount { get; set; }

        public virtual GoodsDeliveryNote IdGoodsDeliveryNoteNavigation { get; set; }
        public virtual Supply IdSuppliesNavigation { get; set; }
    }
}
