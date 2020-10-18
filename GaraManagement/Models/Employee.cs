using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
        }

        public string Id { get; set; }
        [DisplayName("Tên")]
        public string Name { get; set; }
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Ngày BĐ hợp đồng")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ContractStartDate { get; set; }
        [DisplayName("Lương")]
        public int? Salary { get; set; }
        [DisplayName("Điện thoại")]
        public string Phone { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("CMND")]
        public string IdentityCardNumber { get; set; }
        [DisplayName("Bộ phận")]
        public string Department { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
