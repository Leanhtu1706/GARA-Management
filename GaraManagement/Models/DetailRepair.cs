using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailRepair
    {
        public int IdRepair { get; set; }
        [DisplayName("Tên công việc")]
        public int IdWork { get; set; }
        [DisplayName("Số lượng")]
        [Range(minimum: 1, maximum: 1000000000, ErrorMessage = "Giá trị không hợp lệ")]
        public int Amount { get; set; }
        public virtual Repair IdRepairNavigation { get; set; }
        public virtual Work IdWorkNavigation { get; set; }
    }
}
