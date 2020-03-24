using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Taxes.Services.Model;
using TaxesCalculator.Core.Proxy;
using TaxesCalculator.Core.Proxy.Serialization;

namespace Taxes.Services.Proxy
{

    public class TaxCalculatorProxy : HttpProxy, ITaxCalculatorProxy
    {
        private HttpClient _httpClient;
        private const string endPoint = "rates";
        public TaxCalculatorProxy(HttpClient httpClient, IObjectSerializer objectSerializer) : base(httpClient, objectSerializer)
        {
            _httpClient = httpClient;
        }

        private string GetBaseAddress()
        {
            return _httpClient.BaseAddress.ToString();
        }

        public async Task<GetTaxRateResponse> GetTaxRate(GetTaxRateRequest request)
        {
            if (request == null)
            {
                throw new Exception($"{nameof(GetTaxRateRequest)} expected");
            }

            var url = BuildUrl($"{GetBaseAddress()}{endPoint}/{GetZipFullCode(request)}", request);
            JObject f = await GetAsync<JObject>(url);
            return ToRateResponse(f);
        }

        internal string BuildUrl(string url, GetTaxRateRequest request)
        {
            url = AddCountryParameter(url, request.Country);
            return AddCityParameter(url, request.City);
        }

        internal string AddCountryParameter(string url, string country)
        {
            if (!string.IsNullOrWhiteSpace(country))
            {
                return $"{url}{Concatenator(url)}country={country}";
            }

            return url;
        }

        internal string AddCityParameter(string url, string city)
        {
            if (!string.IsNullOrWhiteSpace(city))
            {
                return $"{url}{Concatenator(url)}city={city}";
            }

            return url;
        }
        internal string GetZipFullCode(GetTaxRateRequest request)
        {
            ValidateZipCode5(request);

            return !string.IsNullOrWhiteSpace(request.ZipCode4) ? $"{request.ZipCode5}-{request.ZipCode4}" : request.ZipCode5;
        }

        private static void ValidateZipCode5(GetTaxRateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ZipCode5))
            {
                throw new Exception($"{nameof(request.ZipCode5)} expected");
            }
        }

        internal bool HasQuestionMark(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Url expected");
            }

            return url.Contains("?");
        }

        internal string Concatenator(string url)
        {
            return HasQuestionMark(url) ? "&" : "?";
        }

        internal GetTaxRateResponse ToRateResponse(JObject jsonObject)
        {
            var rate = jsonObject["rate"];
            return new GetTaxRateResponse
            {
                Rate = new Rate
                {
                    City = rate["city"].ToString(),
                    CityRate = Convert.ToDecimal(rate["city_rate"]),
                    DistrictRate = Convert.ToDecimal(rate["combined_district_rate"]),
                    CombinedRate = Convert.ToDecimal(rate["combined_rate"]),
                    Country = rate["country"].ToString(),
                    CountryRate = Convert.ToDecimal(rate["country_rate"]),
                    County = rate["county"].ToString(),
                    CountyRate = Convert.ToDecimal(rate["county_rate"]),
                    IsFreightTaxable = Convert.ToBoolean(rate["freight_taxable"]),
                    State = rate["state"].ToString(),
                    StateRate = Convert.ToDecimal(rate["state_rate"]),
                    ZipCode = Convert.ToString(rate["zip"])
                }
            };
        }
    }
}
