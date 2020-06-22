using Store.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Entities
{
    public class ProductType: BaseEntity
    {
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
