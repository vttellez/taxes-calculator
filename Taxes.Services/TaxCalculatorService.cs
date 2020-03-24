using System;
using System.Threading.Tasks;
using Taxes.Services.Proxy;

namespace Taxes.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ITaxCalculatorProxy _taxRateProxy;
        public TaxCalculatorService(ITaxCalculatorProxy taxRateProxy)
        {
            _taxRateProxy = taxRateProxy;
        }
        public async Task<GetTaxRateResponse> GetTaxRate(GetTaxRateRequest request)
        {
            return await _taxRateProxy.GetTaxRate(request);
        }
    }
}
