using System.Threading.Tasks;
using Taxes.Services.Communication;
using Taxes.Services.Model;
using TaxesCalculator.Core.Domain;

namespace Taxes.Services
{
    public interface ITaxCalculatorService
    {
        Task<Response<Rate>> GetTaxRate(GetTaxRateRequest request);
        Task<Response<OrderTax>> GetOrderTax(Order order);
    }
}
