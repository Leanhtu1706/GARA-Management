using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Work
    {
        public Work()
        {
            DetailRepairs = new HashSet<DetailRepair>();
        }

        public int Id { get; set; }
        public int? IdService { get; set; }
        [DisplayName("Tên công việc")]
        public string WorkName { get; set; }
        [DisplayName("Chi phí")]
        public int? Cost { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public virtual Service IdServiceNavigation { get; set; }
        public virtual ICollection<DetailRepair> DetailRepairs { get; set; }
    }
}
