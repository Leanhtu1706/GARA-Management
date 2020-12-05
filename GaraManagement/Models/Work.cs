using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Work
    {
        public Work()
        {
            DetailRepairs = new HashSet<DetailRepair>();
        }

        public int Id { get; set; }
        public int? IdService { get; set; }
        public string WorkName { get; set; }
        public int? Cost { get; set; }
        public string Description { get; set; }

        public virtual Service IdServiceNavigation { get; set; }
        public virtual ICollection<DetailRepair> DetailRepairs { get; set; }
    }
}
