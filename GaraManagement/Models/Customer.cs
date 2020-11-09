using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdentityCardNumber { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
