using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class TypeOfSupply
    {
        public TypeOfSupply()
        {
            Supplies = new HashSet<Supply>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
