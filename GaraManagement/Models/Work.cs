using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Work
    {
        public Work()
        {
            Repairs = new HashSet<Repair>();
        }

        public int Id { get; set; }
        public string WorkName { get; set; }
        public int? Cost { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
