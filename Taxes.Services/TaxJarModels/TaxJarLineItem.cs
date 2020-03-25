using Newtonsoft.Json;

namespace Taxes.Services.TaxJarModels
{
    internal class TaxJarLineItem
    {
        [JsonProperty("quantity")]
        public short Quantity { get; set; }

        [JsonProperty("unit_price")]
        public decimal Price { get; set; }

        [JsonProperty("product_tax_code")]
        public string TaxCode { get; set; }
    }
}
