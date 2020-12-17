using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Service
    {
        public Service()
        {
            Works = new HashSet<Work>();
        }

        public int Id { get; set; }
        [DisplayName("Tên dịch vụ")]
        public string Name { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public virtual ICollection<Work> Works { get; set; }
    }
}
