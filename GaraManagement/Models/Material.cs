using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string Unit { get; set; }
        public int? Price { get; set; }
        public int? Amount { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual TypeOfSupply IdTypeNavigation { get; set; }
        public virtual ICollection<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
        public virtual ICollection<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
    }
}
