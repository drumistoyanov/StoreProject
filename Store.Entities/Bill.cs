using Store.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Entities
{
    public class Bill : BaseEntity
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public string Number { get; set; }
        public string Bulstat { get; set; }
        public decimal PriceWithDDS { get; set; }
        public decimal PriceWithoutDDS { get; set; }

    }
}
