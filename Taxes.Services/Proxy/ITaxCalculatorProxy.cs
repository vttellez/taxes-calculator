using System.Threading.Tasks;
using Taxes.Services.Model;
using TaxesCalculator.Core.Domain;

namespace Taxes.Services.Proxy
{
    public interface ITaxCalculatorProxy
    {
        Task<Rate> GetTaxRate(GetTaxRateRequest request);
        Task<OrderTax> GetOrderTaxRate(Order request);
    }
}
