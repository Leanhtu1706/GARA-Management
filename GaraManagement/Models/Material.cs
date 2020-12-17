using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Material
    {
        public Material()
        {
            DetailGoodsDeliveryNotes = new HashSet<DetailGoodsDeliveryNote>();
            DetailGoodsReceivedNotes = new HashSet<DetailGoodsReceivedNote>();
        }

        public int Id { get; set; }
        public int? IdType { get; set; }
        [DisplayName("Tên vật tư")]
        public string Name { get; set; }
        [DisplayName("Đơn vị tính")]
        public string Unit { get; set; }
        [DisplayName("Giá")]
        public int? Price { get; set; }
        [DisplayName("Số lượng trong kho")]
        public int? Amount { get; set; }
        [DisplayName("Ảnh")]
        public string Image { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime? CreateAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual TypeOfSupply IdTypeNavigation { get; set; }
        public virtual ICollection<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
        public virtual ICollection<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
    }
}
