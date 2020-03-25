using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxes.Services;
using Taxes.Services.Model;
using Taxes.Services.Proxy;
using TaxesCalculator.Core.Domain;

namespace TaxesCalculator.Tests
{
    [TestClass]
    public class TaxJarCalculatorServiceTests
    {
        private TaxJarCalculatorService _taxJarCalculatorService;
        private Mock<ITaxCalculatorProxy> _taxCalculatorProxy;
        private Fixture _fixture;

        [TestInitialize]
        public void Initialize()
        {
            _taxCalculatorProxy = new Mock<ITaxCalculatorProxy>();
            _taxJarCalculatorService = new TaxJarCalculatorService(_taxCalculatorProxy.Object);
            _fixture = new Fixture();
        }

        [TestMethod]
        public void TaxJarCalculatorService_GetOrderTax_Should_Response_Unssucess_When_Order_Is_Null()
        {
            //Arrange
            bool expectedSuccess = false;
            string expectedMessage = "Order argument expected";
            //Act
            var actualResult = _taxJarCalculatorService.GetOrderTax(null).Result;
            //Assert
            Assert.AreEqual(expectedSuccess, actualResult.Success);
            Assert.AreEqual(expectedMessage, actualResult.Message);
        }


        [TestMethod]
        public void TaxJarCalculatorService_GetOrderTax_Should_Response_Unssucess_When_LineItems_Is_Null()
        {
            //Arrange
            bool expectedSuccess = false;
            string expectedMessage = "No line item present";
            Order order = new Order
            {
                LineItems = null
            };
            //Act
            var actualResult = _taxJarCalculatorService.GetOrderTax(order).Result;
            //Assert
            Assert.AreEqual(expectedSuccess, actualResult.Success);
            Assert.AreEqual(expectedMessage, actualResult.Message);
        }

        [TestMethod]
        public void TaxJarCalculatorService_GetOrderTax_Should_Response_Unssucess_When_LineItems_Is_Empty()
        {
            //Arrange
            bool expectedSuccess = false;
            string expectedMessage = "No line item present";
            Order order = new Order
            {
                LineItems = new List<LineItem>()
            };
            //Act
            var actualResult = _taxJarCalculatorService.GetOrderTax(order).Result;
            //Assert
            Assert.AreEqual(expectedSuccess, actualResult.Success);
            Assert.AreEqual(expectedMessage, actualResult.Message);
        }

        [TestMethod]
        public void TaxJarCalculatorService_GetOrderTax_Should_Response_Unssucess_When_Order_Is_Not_Null()
        {
            //Arrange
            bool expectedSuccess = true;

            Order order = new Order
            {
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                }
            };

            _taxCalculatorProxy.Setup(x => x.GetOrderTaxRate(It.IsAny<Order>())).Returns(() =>
            {
                var orderTax = _fixture.Create<OrderTax>();
                return Task.FromResult(orderTax);
            });

            //Act
            var actualResult = _taxJarCalculatorService.GetOrderTax(order).Result;
            //Assert
            Assert.AreEqual(expectedSuccess, actualResult.Success);

        }
        [TestMethod]
        public void TaxJarCalculatorService_GetTaxRate_Should_Response_Unssucess_When_GetTaxRateRequest_Is_Null()
        {
            //Arrange
            bool expectedSuccess = false;
            string expectedMessage = "GetTaxRateRequest expected";
            //Act
            var actualResult = _taxJarCalculatorService.GetTaxRate(null).Result;
            //Assert
            Assert.AreEqual(expectedSuccess, actualResult.Success);
            Assert.AreEqual(expectedMessage, actualResult.Message);
        }

        [TestMethod]
        public async Task TaxJarCalculatorService_GetTaxRate_Should_Response_Unssucess_When_GetTaxRateRequest_Is_Not_Null()
        {
            //Arrange
            bool expectedSuccess = true;
            GetTaxRateRequest request = new GetTaxRateRequest();
            _taxCalculatorProxy.Setup(x => x.GetTaxRate(It.IsAny<GetTaxRateRequest>())).Returns(async () =>
                {
                    var rate = _fixture.Create<Rate>();
                    return await Task.FromResult(rate);
                }
             );
            //Act
            var actualResult = await _taxJarCalculatorService.GetTaxRate(request);
            //Assert
            Assert.AreEqual(expectedSuccess, actualResult.Success);
        }


    }
}
