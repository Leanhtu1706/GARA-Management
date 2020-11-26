using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

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
        public GenderType? Gender { get; set; }
        public int? Salary { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Image { get; set; }
        public string Department { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }

}
