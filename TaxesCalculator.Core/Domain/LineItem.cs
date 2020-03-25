namespace TaxesCalculator.Core.Domain
{
    public class LineItem
    {
        public short Quantity { get; set; }
        public Product Product { get; set; }
    }
}
