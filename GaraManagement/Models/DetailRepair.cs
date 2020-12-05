using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailRepair
    {
        public int IdRepair { get; set; }
        public int IdWork { get; set; }

        public virtual Repair IdRepairNavigation { get; set; }
        public virtual Work IdWorkNavigation { get; set; }
    }
}
