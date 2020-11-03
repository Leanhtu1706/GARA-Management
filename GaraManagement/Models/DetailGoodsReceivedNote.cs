using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsReceivedNote
    {
        public int IdGoodsReceivedNote { get; set; }
        public int IdMaterial { get; set; }
        public int? Amount { get; set; }
        public int? Price { get; set; }

        public virtual GoodsReceivedNote IdGoodsReceivedNoteNavigation { get; set; }
        public virtual Material IdMaterialNavigation { get; set; }
    }
}
