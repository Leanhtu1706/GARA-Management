using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            Permissions = new HashSet<Permission>();
            Historys = new HashSet<History>();
        }
        [Key]
        [DisplayName("Tài khoản")]
        public string UserName { get; set; }
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [DisplayName("Mã nhân viên")]
        public int? IdEmployee { get; set; }

        public virtual Employee IdEmployeeNavigation { get; set; }
        public virtual ICollection<History> Historys { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
