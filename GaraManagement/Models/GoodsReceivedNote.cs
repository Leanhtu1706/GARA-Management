﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class GoodsReceivedNote
    {
        public GoodsReceivedNote()
        {
            DetailGoodsReceivedNotes = new HashSet<DetailGoodsReceivedNote>();
        }

        public string Id { get; set; }
        public DateTime? ImportDate { get; set; }
        public string IdSupplier { get; set; }
        public string Description { get; set; }

        public virtual Supplier IdSupplierNavigation { get; set; }
        public virtual ICollection<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
    }
}