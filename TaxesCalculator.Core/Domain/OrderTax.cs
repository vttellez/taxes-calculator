namespace TaxesCalculator.Core.Domain
{
    public class OrderTax
    {
        public decimal TotalAmount { get; set; }
        public decimal Rate { get; set; }
        public decimal Sipping { get; set; }
        public decimal TaxableAmount { get; set; }
    }
}
