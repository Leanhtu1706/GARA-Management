using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class GoodsDeliveryNote
    {
        public GoodsDeliveryNote()
        {
            DetailGoodsDeliveryNotes = new HashSet<DetailGoodsDeliveryNote>();
        }

        public int Id { get; set; }
        public DateTime? ExportDate { get; set; }
        public int? IdRepair { get; set; }
        public string Description { get; set; }

        public virtual Repair IdRepairNavigation { get; set; }
        public virtual ICollection<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
    }
}
