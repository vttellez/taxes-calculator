using System.Text.Json.Serialization;


namespace Taxes.Services.Model
{
    public class Rate
    {
        public string City { get; set; }

        public decimal? CityRate { get; set; }

        public decimal? DistrictRate { get; set; }

        public decimal? CombinedRate { get; set; }

        public string Country { get; set; }

        public decimal? CountryRate { get; set; }

        public string County { get; set; }

        public decimal? CountyRate { get; set; }

        public bool IsFreightTaxable { get; set; }

        public string State { get; set; }
        //"state": "CA",
        public decimal StateRate { get; set; }
        //"state_rate": "0.0625",
        //"zip": "90404"
        //}

        public string ZipCode { get; set; }
    }
}
