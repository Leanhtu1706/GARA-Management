using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsReceivedNote
    {
        public int IdGoodsReceivedNote { get; set; }
        public int IdSupplies { get; set; }
        public int? Amount { get; set; }
        public int? Price { get; set; }

        public virtual GoodsReceivedNote IdGoodsReceivedNoteNavigation { get; set; }
        public virtual Supply IdSuppliesNavigation { get; set; }
    }
}
