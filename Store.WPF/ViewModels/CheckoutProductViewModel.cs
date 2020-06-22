using System;
using System.Collections.Generic;
using System.Text;

namespace Store.WPF.ViewModels
{
    public class CheckoutProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Measure { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
