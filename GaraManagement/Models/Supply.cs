using System;
using System.Collections.Generic;
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
        [Display]
        public string IdType { get; set; }
        [Display]
        public string Name { get; set; }
        [Display]
        public string Unit { get; set; }
        [Display]
        public int? Price { get; set; }
        [Display]
        public int? Amount { get; set; }
        [Display]
        public string Description { get; set; }

        public virtual TypeOfSupply IdTypeNavigation { get; set; }
        public virtual ICollection<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
        public virtual ICollection<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
    }
}
