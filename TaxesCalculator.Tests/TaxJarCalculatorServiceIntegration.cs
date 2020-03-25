using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Taxes.Services.Model;
using TaxesCalculator.Api;

namespace TaxesCalculator.Tests
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
    }

    [TestClass]
    public class TaxJarCalculatorServiceIntegration
    {

        private HttpClient _client;


        [TestInitialize]
        public void Initialize()
        {
            _client = new TestingWebAppFactory<Startup>().CreateClient();
        }

        [TestMethod]
        public async Task Get()
        {
            //Arrange
            var responseJson = await _client.GetStringAsync("rates/40218");

            //Act
            var actualResponse = JsonConvert.DeserializeObject<Rate>(responseJson);

            //Assert
            Assert.IsNotNull(actualResponse);
        }
    }
}
