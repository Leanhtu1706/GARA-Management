using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("Ngày xuất")]
        public DateTime? ExportDate { get; set; }
        public int? IdRepair { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Thời gian cập nhật")]
        public DateTime? UpdateAt { get; set; }

        public virtual Repair IdRepairNavigation { get; set; }
        public virtual ICollection<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
    }
}
