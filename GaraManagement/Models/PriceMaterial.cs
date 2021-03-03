using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class PriceMaterial
    {
        public int Id { get; set; }
        public int? IdMaterial { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? Price { get; set; }

        public virtual Material IdMaterialNavigation { get; set; }
    }
}
