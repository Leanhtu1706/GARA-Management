using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaraManagement.Models
{
    public partial class Pay
    {
        public int Id { get; set; }
        public int IdRepair { get; set; }
        public DateTime? DateOfPayment { get; set; }
        public int Paid { get; set; }

        public virtual Repair IdRepairNavigation { get; set; }
    }
}
