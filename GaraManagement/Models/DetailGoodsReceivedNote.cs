using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsReceivedNote
    {
        public string IdGoodsReceivedNote { get; set; }
        public string IdSupplies { get; set; }
        public int? Amount { get; set; }
        public int? Price { get; set; }

        public virtual GoodsReceivedNote IdGoodsReceivedNoteNavigation { get; set; }
        public virtual Supply IdSuppliesNavigation { get; set; }
    }
}
