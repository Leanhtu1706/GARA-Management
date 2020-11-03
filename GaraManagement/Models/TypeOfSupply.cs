using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class TypeOfSupply
    {
        public TypeOfSupply()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
