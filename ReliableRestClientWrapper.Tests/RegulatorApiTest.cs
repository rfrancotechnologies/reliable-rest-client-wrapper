using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using ReliableRestClient.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReliableRestClient.Tests
{
    public class RegulatorApiTest
    {

        public IRestClient _client { get; set; }
        int retries = 0;

        [Fact]
        public void request_fails_retry()
        {
            ISyncPolicy retryPolicy = GenerateTimeoutAndRetryPolicy(1,1,60);
            
            _client = new ReliableRestClientWrapper(new RestClient("http://localhost:8190/v1"), retryPolicy);

            var request = new RestRequest("/regulator/self-exclusions", Method.GET);

            request.AddParameter("documentNumber", "1111111");
            request.AddParameter("documentType", "CC");

            IRestResponse response = _client.Execute(request);

            Assert.Equal(0, (int) response.StatusCode);
            Console.WriteLine("Number of retries: " + retries);
        }


        [Fact]
        public void request_fails_timeout()
        {
            retries = 0;
            ISyncPolicy retryPolicy = GenerateTimeoutAndRetryPolicy(50, 100, 10);
            _client = new ReliableRestClientWrapper(new RestClient("http://localhost:8190/v1"), retryPolicy);

            var request = new RestRequest("/regulator/self-exclusions", Method.GET);

            request.AddParameter("documentNumber", "1111111");
            request.AddParameter("documentType", "CC");
            IRestResponse response = null;

            try {
                response = _client.Execute(request);
            }catch (TimeoutRejectedException){
                
            }
            
            Assert.Null(response);
            Console.WriteLine("Number of retries: " + retries);
        }


        [Fact]
        public void request_fails_circuit_breaker()
        {
            retries = 0;
            ISyncPolicy retryPolicy = Policy.Handle<RestException>().CircuitBreaker(1, TimeSpan.FromSeconds(10));
            _client = new ReliableRestClientWrapper(new RestClient("http://localhost:8190/v1"), retryPolicy);

            var request = new RestRequest("/regulator/self-exclusions", Method.GET);

            request.AddParameter("documentNumber", "1111111");
            request.AddParameter("documentType", "CC");

            IRestResponse response = null;    
            
            try {
                response = _client.Execute(request);
            }catch (RestTimeoutException){

            }
            
            Assert.Null(response);

            try {
                response = _client.Execute(request);
            }catch (BrokenCircuitException){

            }
            
            Assert.Null(response);

            Console.WriteLine("Number of retries: " + retries);
        }


        #region private methods

        private ISyncPolicy GenerateTimeoutAndRetryPolicy(int retryNumber, int retrySleep, int timeoutSeconds)
        {
            var retry = Policy
                .Handle<RestException>()
                .WaitAndRetry(retryNumber, retryAttempt => TimeSpan.FromMilliseconds(
                        retrySleep * Math.Pow(2, retryAttempt)), onRetry: (exception, calculatedWaitDuration) => // Capture some info for logging!
                        {
                            Console.WriteLine("Policy logging: " + exception.Message);
                            retries++;
                        });

            var timeout = Policy.Timeout(timeoutSeconds);

            var policyWrap = Policy.Wrap(timeout, retry);

            return Policy.Handle<RestException>().Fallback(() => { }).Wrap(policyWrap);
        }

        #endregion

    }
}
