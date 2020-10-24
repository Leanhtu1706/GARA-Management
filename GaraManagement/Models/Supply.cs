using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Supply
    {
        public Supply()
        {
            DetailGoodsDeliveryNotes = new HashSet<DetailGoodsDeliveryNote>();
            DetailGoodsReceivedNotes = new HashSet<DetailGoodsReceivedNote>();
        }

        public string Id { get; set; }
        [DisplayName("Loại")]
        public string IdType { get; set; }
        [DisplayName("Tên")]
        public string Name { get; set; }
        [DisplayName("Đơn vị tính")]
        public string Unit { get; set; }
        [DisplayName("Giá")]
        public int? Price { get; set; }
        [DisplayName("Số lượng")]
        public int? Amount { get; set; }
        [DisplayName("Ảnh")]
        public string Image { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public virtual TypeOfSupply IdTypeNavigation { get; set; }
        public virtual ICollection<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
        public virtual ICollection<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
    }
}
