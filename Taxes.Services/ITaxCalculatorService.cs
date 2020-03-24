using System.Threading.Tasks;

namespace Taxes.Services
{
    public interface ITaxCalculatorService
    {
        Task<GetTaxRateResponse> GetTaxRate(GetTaxRateRequest request);
    }
}
