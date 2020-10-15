using System;
using System.Collections.Generic;

#nullable disable

namespace GaraManagement.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            GoodsReceivedNotes = new HashSet<GoodsReceivedNote>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; }
    }
}
