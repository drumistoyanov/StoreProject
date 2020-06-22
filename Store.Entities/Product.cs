using Store.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Entities
{
    public class Product:BaseEntity
    {
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Measure { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal DeliveryPrice { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("FK_Product_ProductType")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        public virtual ICollection<SaleProduct> SaleProducts { get; set; } = new HashSet<SaleProduct>();
    }
}
