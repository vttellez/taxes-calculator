using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Taxes.Services.Model;
using TaxesCalculator.Core.Domain;

namespace Taxes.Services.TaxJarModels.Extensions
{
    public static class TaxJarExtensions
    {
        internal static TaxJarOrder ToTaxJarOrder(this Order order)
        {
            return new TaxJarOrder
            {
                FromCountry = order.Origin.Country,
                FromState = order.Origin.State,
                FromZip = order.Origin.ZipCode5,
                ToCountry = order.Destination.Country,
                ToState = order.Destination.State,
                ToZip = order.Destination.ZipCode5,
                Amount = order.Amount,
                Shipping = order.Shipping,
                LineItems = ToTaxJarLineItems(order)
            };
        }

        private static IEnumerable<TaxJarLineItem> ToTaxJarLineItems(Order order)
        {
            return order.LineItems.Select(x => new TaxJarLineItem
            {
                Price = x.Product.Price,
                TaxCode = x.Product.TaxCode,
                Quantity = x.Quantity
            });
        }


        internal static OrderTax ToOrdertax(this JObject jsonObject)
        {
            var tax = jsonObject["tax"];

            return new OrderTax
            {
                TotalAmount = Convert.ToDecimal(tax["order_total_amount"]),
                Rate = Convert.ToDecimal(tax["rate"]),
                Sipping = Convert.ToDecimal(tax["shipping"]),
                TaxableAmount = Convert.ToDecimal(tax["taxable_amount"])
            };
        }

        internal static Rate ToRate(this JObject jsonObject)
        {
            var rate = jsonObject["rate"];
            return new Rate
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
            };
        }

    }
}
