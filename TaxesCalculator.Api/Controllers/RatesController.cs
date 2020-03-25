using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Taxes.Services;
using Taxes.Services.Communication;
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
                var response = await _taxRateService.GetTaxRate(request);
                return ProcessResponse(response);
            }
            catch  //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex

                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        private IActionResult ProcessResponse<T>(Response<T> response)
        {
            if (!response.Success)
            {
                BadRequest(response.Message);
            }
            return Ok(response.Resource);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrderTax), 200)]
        public async Task<IActionResult> PostAsync([FromBody] Order request)
        {
            try
            {
                var response = await _taxRateService.GetOrderTax(request);
                return ProcessResponse(response);
            }
            catch //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}