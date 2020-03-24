using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TaxesCalculator.Core.Proxy.Serialization;
using TaxesCalculator.Core.Proxy.Validation;

namespace TaxesCalculator.Core.Proxy
{
    public class ValidationHttpProxy : HttpProxy
    {
        private readonly IObjectSerializer _objectSerializer;

        public ValidationHttpProxy(HttpClient httpClient, IObjectSerializer objectSerializer)
            : base(httpClient,
                   objectSerializer)
        {
            _objectSerializer = objectSerializer;
        }

        protected override async Task<bool> HandleResponse(HttpResponseMessage httpResponse,
                                                           Uri requestUri)
        {
            if (httpResponse.StatusCode != HttpStatusCode.UnprocessableEntity)
            {
                return false;
            }

            var validationResult = await httpResponse.Content.ReadAsStringAsync();
            var validationException = new ValidationException($@"HTTP Code: Unprocessable Entity {httpResponse.StatusCode}. 
                                                                URI: {requestUri}. 
                                                                Error: {await httpResponse.Content.ReadAsStringAsync()}.",
                                                                _objectSerializer.Deserialize<ValidationResult>(validationResult)
                                                             );

            throw validationException;
        }
    }
}
