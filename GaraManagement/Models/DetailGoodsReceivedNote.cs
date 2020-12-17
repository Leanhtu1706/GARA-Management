using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsReceivedNote
    {
        public int IdGoodsReceivedNote { get; set; }
        public int IdMaterial { get; set; }
        [DisplayName("Số lượng nhập")]
        public int? Amount { get; set; }
        [DisplayName("Giá nhập")]
        public int? Price { get; set; }

        public virtual GoodsReceivedNote IdGoodsReceivedNoteNavigation { get; set; }
        public virtual Material IdMaterialNavigation { get; set; }
    }
}
