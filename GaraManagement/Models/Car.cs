using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Car
    {
        public Car()
        {
            Repairs = new HashSet<Repair>();
        }

        public string Id { get; set; }
        public string IdCustomer { get; set; }
        public string CarName { get; set; }
        public string Manufacturer { get; set; }
        public string LicensePlates { get; set; }
        public string Color { get; set; }

        public virtual Customer IdCustomerNavigation { get; set; }
        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
