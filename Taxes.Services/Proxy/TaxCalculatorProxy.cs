using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Taxes.Services.Model;
using Taxes.Services.TaxJarModels;
using Taxes.Services.TaxJarModels.Extensions;
using TaxesCalculator.Core.Domain;
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

        public async Task<Rate> GetTaxRate(GetTaxRateRequest request)
        {
            if (request == null)
            {
                throw new Exception($"{nameof(GetTaxRateRequest)} expected");
            }

            var url = BuildUrl($"{GetBaseAddress()}{endPoint}/{GetZipFullCode(request)}", request);
            JObject f = await GetAsync<JObject>(url);
            return f.ToRate();
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


        public async Task<OrderTax> GetOrderTaxRate(Order request)
        {
            string url = GetOrderTaxesUrl();
            JObject f = await PostAsync<TaxJarOrder, JObject>(url, request.ToTaxJarOrder());
            return f.ToOrdertax();
        }

        private string GetOrderTaxesUrl()
        {
            return $"{GetBaseAddress()}taxes";
        }
    }
}



