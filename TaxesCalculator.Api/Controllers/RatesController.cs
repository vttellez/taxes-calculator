using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Taxes.Services;
using Taxes.Services.Model;
using TaxesCalculator.Core.Domain;

namespace TaxesCalculator.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly ITaxCalculatorService _taxRateService;
        public RatesController(
            ITaxCalculatorService taxRateService
         )
        {
            _taxRateService = taxRateService;
        }
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Rate), 200)]
        [ProducesResponseType(500)]
        [Route("{zipCode5}/{zipCode4?}/{city?}/{country?}")]
        public async Task<IActionResult> GetAsync([FromRoute] GetTaxRateRequest request)
        {
            try
            {
                // This can be refactored since is a duplicate on PostAsync to process response 

                var response = await _taxRateService.GetTaxRate(request);

                if (!response.Success)
                {
                    BadRequest(response.Message);
                }
                return Ok(response.Resource);
            }
            catch  //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex

                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }


        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrderTax), 200)]
        public async Task<IActionResult> PostAsync([FromBody] Order request)
        {
            try
            {
                var response = await _taxRateService.GetOrderTax(request);

                if (!response.Success)
                {
                    BadRequest(response.Message);
                }

                return Ok(response.Resource);
            }
            catch //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}