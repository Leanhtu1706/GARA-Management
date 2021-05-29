using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GaraManagement.Models
{
    public partial class PriceMaterial
    {
        public int Id { get; set; }
        public int? IdMaterial { get; set; }
        public DateTime? UpdateAt { get; set; }
        [Range(minimum: 0, maximum: 1000000000, ErrorMessage = "Giá trị không hợp lệ")]
        public int? Price { get; set; }

        public virtual Material IdMaterialNavigation { get; set; }
    }
}
