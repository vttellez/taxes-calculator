using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taxes.Services;

namespace TaxesCalculator.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly ITaxCalculatorService _taxRateService;
        public RatesController(ITaxCalculatorService taxRateService)
        {
            _taxRateService = taxRateService;
        }
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("{zipCode5}/{zipCode4?}/{city?}/{country?}")]
        public async Task<IActionResult> Get([FromRoute] GetTaxRateRequest request)
        {
            try
            {
                return Ok(await _taxRateService.GetTaxRate(request));
            }
            catch  //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex

                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}