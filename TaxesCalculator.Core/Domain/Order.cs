using System.Collections.Generic;

namespace TaxesCalculator.Core.Domain
{
    public class Order
    {

        public Location Origin { get; set; }
        public Location Destination { get; set; }
        public decimal Amount { get; set; }
        public decimal Shipping { get; set; }
        public IEnumerable<LineItem> LineItems { get; set; }
    }
}
