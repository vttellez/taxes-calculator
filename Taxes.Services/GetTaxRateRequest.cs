namespace Taxes.Services
{
    public class GetTaxRateRequest
    {
        public string ZipCode5 { get; set; }
        public string ZipCode4 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
