using System.Linq;
using System.Threading.Tasks;
using Taxes.Services.Communication;
using Taxes.Services.Model;
using Taxes.Services.Proxy;
using TaxesCalculator.Core.Domain;

namespace Taxes.Services
{
    /// <summary>
    /// TaxJar taxes calculator service
    /// </summary>
    public class TaxJarCalculatorService : ITaxCalculatorService
    {
        private readonly ITaxCalculatorProxy _taxRateProxy;
        public TaxJarCalculatorService(ITaxCalculatorProxy taxRateProxy)
        {
            _taxRateProxy = taxRateProxy;
        }

        /// <summary>
        /// Retrieves tax rate for an order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Response<OrderTax>> GetOrderTax(Order order)
        {
            if (order == null)
            {
                return new Response<OrderTax>("Order argument expected");
            }

            if (order.LineItems == null || !order.LineItems.Any())
            {
                return new Response<OrderTax>("No line item present");
            }

            return new Response<OrderTax>(await _taxRateProxy.GetOrderTaxRate(order));
        }

        /// <summary>
        /// Retrieve tax rate for a location
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<Rate>> GetTaxRate(GetTaxRateRequest request)
        {
            if (request == null)
            {
                return new Response<Rate>($"{nameof(GetTaxRateRequest)} expected");
            }

            return new Response<Rate>(await _taxRateProxy.GetTaxRate(request));
        }
    }
}
