using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace GaraManagement.Models
{
    public partial class TypeOfSupply
    {
        public TypeOfSupply()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        [DisplayName("Loại vật tư")]
        public string Name { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime? CreateAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
