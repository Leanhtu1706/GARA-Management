using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Service
    {
        public Service()
        {
            Repairs = new HashSet<Repair>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
