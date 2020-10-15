using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Repair
    {
        public Repair()
        {
            GoodsDeliveryNotes = new HashSet<GoodsDeliveryNote>();
        }

        public string Id { get; set; }
        public string IdService { get; set; }
        public string IdCar { get; set; }
        public DateTime? DateOfFactoryEntry { get; set; }
        public DateTime? DateFinished { get; set; }
        public int? Cost { get; set; }
        public int? Paid { get; set; }
        public int? State { get; set; }

        public virtual Car IdCarNavigation { get; set; }
        public virtual Service IdServiceNavigation { get; set; }
        public virtual ICollection<GoodsDeliveryNote> GoodsDeliveryNotes { get; set; }
    }
}
