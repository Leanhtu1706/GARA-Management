using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Service
    {
        public Service()
        {
            Works = new HashSet<Work>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Work> Works { get; set; }
    }
}
