using Store.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Entities
{
    public class Sale : BaseEntity
    {
        public virtual ICollection<SaleProduct> SaleProducts { get; set; } = new HashSet<SaleProduct>();
        public decimal TotalPrice { get; set; }
        public Bill Bill { get; set; }
    }
}
