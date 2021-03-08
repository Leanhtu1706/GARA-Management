using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class CarModel
    {
        public CarModel()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }
        [DisplayName("Dòng xe")]
        public string ModelName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
