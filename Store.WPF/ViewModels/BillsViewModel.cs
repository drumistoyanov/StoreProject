using Store.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.WPF.ViewModels
{
    public class BillsViewModel
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public string Number { get; set; }
        public string Bulstat { get; set; }
        public decimal PriceWithDDS { get; set; }
        public decimal PriceWithoutDDS { get; set; }
    }
}
