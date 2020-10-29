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

        public int Id { get; set; }
        public int? IdCustomer { get; set; }
        public string CarName { get; set; }
        public string Manufacturer { get; set; }
        public string LicensePlates { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }

        public virtual Customer IdCustomerNavigation { get; set; }
        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
