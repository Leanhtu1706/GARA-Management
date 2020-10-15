using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class TypeOfSupply
    {
        public TypeOfSupply()
        {
            Supplies = new HashSet<Supply>();
        }

        public string Id { get; set; }
        [DisplayName("Loại")]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
