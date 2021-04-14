using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaraManagement.Models
{
    public partial class History
    {
        [DisplayName("Thời gian")]
        
        public DateTime DateHistory { get; set; }
        [DisplayName("Sự kiện")]
        public string Event { get; set; }
        [DisplayName("Tài khoản")]
        public string UserName { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
