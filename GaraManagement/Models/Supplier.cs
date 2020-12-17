using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            GoodsReceivedNotes = new HashSet<GoodsReceivedNote>();
        }

        public int Id { get; set; }
        [DisplayName("Tên nhà cung cấp")]
        public string Name { get; set; }
        [DisplayName("Điện thoại")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; }
    }
}
