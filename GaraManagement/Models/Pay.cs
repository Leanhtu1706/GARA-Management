using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GaraManagement.Models
{
    public partial class Pay
    {
        public int Id { get; set; }
        public int IdRepair { get; set; }
        [DisplayName("Ngày thanh toán")]
        public DateTime? DateOfPayment { get; set; }
        [DisplayName("Đã thanh toán")]
        public int Paid { get; set; }
        [DisplayName("Ngày cập nhật")]
        public DateTime? Update_at { get; set; }

        [NotMapped]
        [DisplayName("Thanh toán thêm")]
        public int owe { get; set; }

        public virtual Repair IdRepairNavigation { get; set; }
    }
}
