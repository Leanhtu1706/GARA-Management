using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GaraManagement.Models
{
    public partial class Position
    {
        
        public int Id { get; set; }
        [DisplayName("Chức vụ")]
        public string Name { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }

    }
}
