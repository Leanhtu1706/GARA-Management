using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GaraManagement.Models
{
    public partial class Permission
    {
        [DisplayName("Tài khoản")]
        public string UserName { get; set; }
        [DisplayName("Chức vụ")]
        public int PositionId { get; set; }
        public virtual Account IdAccountNavigation { get; set; }
        public virtual Position IdPositionNavigation { get; set; }

    }
}
