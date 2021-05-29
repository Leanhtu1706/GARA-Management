using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GaraManagement.Models
{
    public partial class GoodsReceivedNote
    {
        public GoodsReceivedNote()
        {
            DetailGoodsReceivedNotes = new HashSet<DetailGoodsReceivedNote>();
        }

        public int Id { get; set; }
        [DisplayName("Ngày nhập")]
        public DateTime? ImportDate { get; set; }
        [DisplayName("Nhà cung cấp")]
        public int? IdSupplier { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Thời gian cập nhật")]
        public DateTime? UpdateAt { get; set; }
        [NotMapped]
        [DisplayName("Tổng tiền")]
        public int? Total { get; set; }
        public virtual Supplier IdSupplierNavigation { get; set; }
        public virtual ICollection<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
    }
}
