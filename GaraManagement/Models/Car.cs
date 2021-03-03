using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int? IdCarModel { get; set; }
        [DisplayName("Biển số")]
        public string LicensePlates { get; set; }
        [DisplayName("Màu")]
        public string Color { get; set; }
        [DisplayName("Logo")]
        public string Image { get; set; }
        [DisplayName("Ghi chú")]
        public string Note { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual CarModel IdCarModelNavigation { get; set; }
        public virtual Customer IdCustomerNavigation { get; set; }
        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
