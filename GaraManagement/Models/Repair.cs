using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Repair
    {
        public Repair()
        {
            DetailRepairs = new HashSet<DetailRepair>();
            GoodsDeliveryNotes = new HashSet<GoodsDeliveryNote>();
            Pays = new HashSet<Pay>();
        }

        public int Id { get; set; }
        public int? IdCar { get; set; }
        [DisplayName("Ngày nhập xưởng")]
        public DateTime DateOfFactoryEntry { get; set; }
        [DisplayName("Ngày hoàn thành")]
        public DateTime? DateFinished { get; set; }
        [DisplayName("Trạng thái")]
        public StateType? State { get; set; }
        public virtual Car IdCarNavigation { get; set; }
        public virtual ICollection<DetailRepair> DetailRepairs { get; set; }
        public virtual ICollection<GoodsDeliveryNote> GoodsDeliveryNotes { get; set; }
        public virtual ICollection<Pay> Pays { get; set; }
    }
}
