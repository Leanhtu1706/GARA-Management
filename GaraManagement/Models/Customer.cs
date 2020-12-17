using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("Họ tên")]
        public string Name { get; set; }
        [DisplayName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Giới tính")]
        public GenderType? Gender { get; set; }
        [DisplayName("Điện thoại")]
        public string Phone { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("CMND")]
        public string IdentityCardNumber { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
