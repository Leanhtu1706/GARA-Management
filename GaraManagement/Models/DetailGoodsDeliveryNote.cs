﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GaraManagement.Models
{
    public partial class DetailGoodsDeliveryNote
    {
        public int IdGoodsDeliveryNote { get; set; }
        public int IdMaterial { get; set; }
        [DisplayName("Số lượng xuất")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(minimum: 0, maximum: 1000000000, ErrorMessage = "Giá trị không hợp lệ")]
        public int? Amount { get; set; }
        [DisplayName("Giá bán")]
        public int? Price { get; set; }

        public virtual GoodsDeliveryNote IdGoodsDeliveryNoteNavigation { get; set; }
        public virtual Material IdMaterialNavigation { get; set; }
    }
}
