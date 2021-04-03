using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
            GoodsDeliveryNotes = new HashSet<GoodsDeliveryNote>();

        }

        public int Id { get; set; }
        [DisplayName("Họ tên")]
        public string Name { get; set; }
        [DisplayName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Ngày BĐHĐ")]
        public DateTime? ContractStartDate { get; set; }
        [DisplayName("Giới tính")]
        public GenderType? Gender { get; set; }
        [DisplayName("Lương")]
        [Range(minimum: 0, maximum: 1000000000, ErrorMessage = "Giá trị không hợp lệ")]
        public int? Salary { get; set; }
        [DisplayName("Điện thoại")]
        public string Phone { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("CMND")]
        public string IdentityCardNumber { get; set; }
        [DisplayName("Ảnh")]
        public string Image { get; set; }
        [DisplayName("Bộ phận")]
        public string Department { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<GoodsDeliveryNote> GoodsDeliveryNotes { get; set; }
    }
}
