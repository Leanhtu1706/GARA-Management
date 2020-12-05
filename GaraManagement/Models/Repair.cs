using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Repair
    {
        public Repair()
        {
            DetailRepairs = new HashSet<DetailRepair>();
            GoodsDeliveryNotes = new HashSet<GoodsDeliveryNote>();
        }

        public int Id { get; set; }
        public int? IdCar { get; set; }
        public int? IdWork { get; set; }
        public DateTime? DateOfFactoryEntry { get; set; }
        public DateTime? DateFinished { get; set; }
        public int? State { get; set; }

        public virtual Car IdCarNavigation { get; set; }
        public virtual ICollection<DetailRepair> DetailRepairs { get; set; }
        public virtual ICollection<GoodsDeliveryNote> GoodsDeliveryNotes { get; set; }
    }
}
