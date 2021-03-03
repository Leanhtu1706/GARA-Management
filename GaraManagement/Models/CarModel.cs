using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class CarModel
    {
        public CarModel()
        {
            Cars = new HashSet<Car>();
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string ModelName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
