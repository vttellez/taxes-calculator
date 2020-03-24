using System.Threading.Tasks;

namespace Taxes.Services.Proxy
{
    public interface ITaxCalculatorProxy
    {
        Task<GetTaxRateResponse> GetTaxRate(GetTaxRateRequest request);
    }
}
