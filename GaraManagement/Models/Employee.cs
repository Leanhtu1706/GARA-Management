using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public int? Salary { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Image { get; set; }
        public string Department { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
