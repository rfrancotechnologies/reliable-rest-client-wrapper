using Polly;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Security;
using ReliableRestConnectionWrapper.Exceptions;

namespace ReliableRestClientWrapper
{
    public class ReliableRestClientWrapper : IRestClient
    {
        private IRestClient _innerClient;

        private readonly ISyncPolicy _retryPolicy;

        private int[] HttpStatusCodesWorthRetrying = { 500, 502, 503 };

        private int[] HttpStatusCodesTimeout = { 0, 408, 504 };

        public ReliableRestClientWrapper(IRestClient innerClient, ISyncPolicy retryPolicy) : base()
        {
            _innerClient = innerClient;
            _retryPolicy = retryPolicy;
        }

        public IAuthenticator Authenticator
        {
            get
            {
                return _innerClient.Authenticator;
            }

            set
            {
                _innerClient.Authenticator = value;
            }
        }

        public Uri BaseUrl
        {
            get
            {
                return _innerClient.BaseUrl;
            }

            set
            {
                _innerClient.BaseUrl = value;
            }
        }

        public RequestCachePolicy CachePolicy
        {
            get
            {
                return _innerClient.CachePolicy;
            }

            set
            {
                _innerClient.CachePolicy = value;
            }
        }

        public X509CertificateCollection ClientCertificates
        {
            get
            {
                return _innerClient.ClientCertificates;
            }

            set
            {
                _innerClient.ClientCertificates = value;
            }
        }

        public CookieContainer CookieContainer
        {
            get
            {
                return _innerClient.CookieContainer;
            }

            set
            {
                _innerClient.CookieContainer = value;
            }
        }

        public IList<Parameter> DefaultParameters
        {
            get
            {
                return _innerClient.DefaultParameters;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return _innerClient.Encoding;
            }

            set
            {
                _innerClient.Encoding = value;
            }
        }

        public bool FollowRedirects
        {
            get
            {
                return _innerClient.FollowRedirects;
            }

            set
            {
                _innerClient.FollowRedirects = value;
            }
        }

        public int? MaxRedirects
        {
            get
            {
                return _innerClient.MaxRedirects;
            }

            set
            {
                _innerClient.MaxRedirects = value;
            }
        }

        public bool PreAuthenticate
        {
            get
            {
                return _innerClient.PreAuthenticate;
            }

            set
            {
                _innerClient.PreAuthenticate = value;
            }
        }

        public IWebProxy Proxy
        {
            get
            {
                return _innerClient.Proxy;
            }

            set
            {
                _innerClient.Proxy = value;
            }
        }

        public int ReadWriteTimeout
        {
            get
            {
                return _innerClient.ReadWriteTimeout;
            }

            set
            {
                _innerClient.ReadWriteTimeout = value;
            }
        }

        public int Timeout
        {
            get
            {
                return _innerClient.Timeout;
            }

            set
            {
                _innerClient.Timeout = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return _innerClient.UserAgent;
            }

            set
            {
                _innerClient.UserAgent = value;
            }
        }

        public bool UseSynchronizationContext
        {
            get
            {
                return _innerClient.UseSynchronizationContext;
            }

            set
            {
                _innerClient.UseSynchronizationContext = value;
            }
        }

        public bool AutomaticDecompression
        {
            get
            {
                return _innerClient.AutomaticDecompression;
            }

            set
            {
                _innerClient.AutomaticDecompression = value;
            }
        }

        public string ConnectionGroupName
        {
            get
            {
                return _innerClient.ConnectionGroupName;
            }

            set
            {
                _innerClient.ConnectionGroupName = value;
            }
        }

        public bool UnsafeAuthenticatedConnectionSharing
        {
            get
            {
                return _innerClient.UnsafeAuthenticatedConnectionSharing;
            }

            set
            {
                _innerClient.UnsafeAuthenticatedConnectionSharing = value;
            }
        }

        public string BaseHost
        {
            get
            {
                return _innerClient.BaseHost;
            }

            set
            {
                _innerClient.BaseHost = value;
            }
        }

        public bool AllowMultipleDefaultParametersWithSameName
        {
            get
            {
                return _innerClient.AllowMultipleDefaultParametersWithSameName;
            }

            set
            {
                _innerClient.AllowMultipleDefaultParametersWithSameName = value;
            }
        }

        public bool Pipelined
        {
            get
            {
                return _innerClient.Pipelined;
            }

            set
            {
                _innerClient.Pipelined = value;
            }
        }

        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback
        {
            get
            {
                return _innerClient.RemoteCertificateValidationCallback;
            }

            set
            {
                _innerClient.RemoteCertificateValidationCallback = value;
            }
        }

        public void AddHandler(string contentType, IDeserializer deserializer)
        {
            _innerClient.AddHandler(contentType, deserializer);
        }

        public Uri BuildUri(IRestRequest request)
        {
            return _innerClient.BuildUri(request);
        }

        public void ClearHandlers()
        {
            _innerClient.ClearHandlers();
        }

        public byte[] DownloadData(IRestRequest request)
        {
            return _innerClient.DownloadData(request);
        }

        public virtual IRestResponse Execute(IRestRequest request)
        {
            IRestResponse response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.Execute(request);
                ProcessResponse(response);

            });


            return response;
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {

            IRestResponse<T> response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.Execute<T>(request);
                ProcessResponse(response);

            });

            return response;
        }


        public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
        {

            IRestResponse response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.ExecuteAsGet(request, httpMethod);
                ProcessResponse(response);

            });

            return response;
        }

        public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
        {

            IRestResponse<T> response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.ExecuteAsGet<T>(request, httpMethod);
                ProcessResponse(response);

            });

            return response;
        }

        public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
        {

            IRestResponse response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.ExecuteAsPost(request, httpMethod);
                ProcessResponse(response);

            });

            return response;
        }

        public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
        {
            IRestResponse<T> response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.ExecuteAsPost<T>(request, httpMethod);
                ProcessResponse(response);

            });

            return response;
        }


        public IRestResponse<T> Deserialize<T>(IRestResponse response)
        {
            return _innerClient.Deserialize<T>(response);
        }

        public IRestResponse Execute(IRestRequest request, Method httpMethod)
        {
            IRestResponse response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.Execute(request, httpMethod);
                ProcessResponse(response);

            });

            return response;
        }

        public IRestResponse<T> Execute<T>(IRestRequest request, Method httpMethod) where T : new()
        {
            IRestResponse<T> response = null;

            _retryPolicy.Execute(() =>
            {
                response = _innerClient.Execute<T>(request, httpMethod);
                ProcessResponse(response);

            });

            return response;
        }

        public byte[] DownloadData(IRestRequest request, bool throwOnError)
        {
            return _innerClient.DownloadData(request, throwOnError);
        }

        public void ConfigureWebRequest(Action<HttpWebRequest> configurator)
        {
            _innerClient.ConfigureWebRequest(configurator);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, Method httpMethod)
        {
            return _innerClient.ExecuteTaskAsync<T>(request, httpMethod);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token, Method httpMethod)
        {
            return _innerClient.ExecuteTaskAsync(request, token, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, Method httpMethod)
        {
            return _innerClient.ExecuteAsync(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, Method httpMethod)
        {
            return _innerClient.ExecuteAsync<T>(request, callback, httpMethod);
        }
        //Hasta aqui
        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            return _innerClient.ExecuteAsync(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            return _innerClient.ExecuteAsync<T>(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _innerClient.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _innerClient.ExecuteAsyncGet<T>(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _innerClient.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _innerClient.ExecuteAsyncPost<T>(request, callback, httpMethod);
        }

        public Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request)
        {
            return _innerClient.ExecuteGetTaskAsync(request);
        }

        public Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request, CancellationToken token)
        {
            return _innerClient.ExecuteGetTaskAsync(request, token);
        }

        public Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request)
        {
            return _innerClient.ExecuteGetTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            return _innerClient.ExecuteGetTaskAsync<T>(request, token);
        }

        public Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request)
        {
            return _innerClient.ExecutePostTaskAsync(request);
        }

        public Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request, CancellationToken token)
        {
            return _innerClient.ExecutePostTaskAsync(request, token);
        }

        public Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request)
        {
            return _innerClient.ExecutePostTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            return _innerClient.ExecutePostTaskAsync<T>(request, token);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            return _innerClient.ExecuteTaskAsync(request);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token)
        {
            return _innerClient.ExecuteTaskAsync(request, token);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            return _innerClient.ExecuteTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            return _innerClient.ExecuteTaskAsync<T>(request, token);
        }

        public void RemoveHandler(string contentType)
        {
            _innerClient.RemoveHandler(contentType);
        }

        private void ProcessResponse<T>(IRestResponse<T> response)
        {

            if (HttpStatusCodesWorthRetrying.Contains((int)response.StatusCode))
            {
                throw new RestServerSideException((int)response.StatusCode, response.ErrorMessage, response.ErrorException);
            }
            else if (HttpStatusCodesTimeout.Contains((int)response.StatusCode))
            {
                throw new RestTimeoutException((int)response.StatusCode, response.ErrorMessage, response.ErrorException);
            }

        }


        private void ProcessResponse(IRestResponse response)
        {

            if (HttpStatusCodesWorthRetrying.Contains((int)response.StatusCode))
            {
               throw new RestServerSideException((int)response.StatusCode, response.ErrorMessage, response.ErrorException);
            }
            else if (HttpStatusCodesTimeout.Contains((int)response.StatusCode))
            {
               throw new RestTimeoutException((int)response.StatusCode, response.ErrorMessage, response.ErrorException);
            }
        }

        
    }
}
