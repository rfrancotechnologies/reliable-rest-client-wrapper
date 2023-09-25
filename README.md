# reliable-rest-client-wrapper
RestSharp wrapper with resilience policies, making synchronous requests more reliable to transient failure via Polly library.

[![Build History](https://buildstats.info/travisci/chart/mediatechsolutions/reliable-rest-client-wrapper?branch=master)](https://travis-ci.org/mediatechsolutions/reliable-rest-client-wrapper)
[![NuGet Version](https://buildstats.info/nuget/ReliableRestClientWrapper?includePreReleases=true)](https://www.nuget.org/packages/ReliableRestClientWrapper)
[![Build Status](https://travis-ci.org/mediatechsolutions/reliable-rest-client-wrapper.svg?branch=master)](https://travis-ci.org/mediatechsolutions/reliable-rest-client-wrapper)

## Why ReliableRestClientWrapper
Network services can fail or become temporarily unreachable. This is especially true when running code on cloud providers. It is extremely valuable to add resilience to the HTTP connection in an already existing project without having to change code. 

ReliableRestClientWrapper is a wrapper over `IRestClient` which allows .NET applications to apply configurable resilience policies to HTTP syncrhonous operations.

## How to use it
In order to automatically add resilience policies (as retry) to all your request you have to reference the ReliableRestClientWrapper Nuget package in your project and wrap your HTTP connection (`IRestClient`) into a `ReliableRestClientWrapper`. 
```csharp
_client = new ReliableRestClientWrapper(new RestClient(BaseUrl), RetryPolicy);
```

Be careful to create the REST client  via the wrapped `ReliableRestClientWrapper` because otherwise the resilience policies  will not be applied:

You need to provide a configured [Polly](https://github.com/App-vNext/Polly) policies to the constructor of `ReliableDbConnectionWrapper`:
```csharp
var retryPolicy = Policy
    .Handle<RestServerSideException>()
    .WaitAndRetry(5, retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt))); 
_client = new ReliableRestClientWrapper(new RestClient(BaseUrl), retryPolicy);
```
Also is possible to configure multiples policies:
```csharp
policyWrap = Policy.Wrap(fallback, cache, retry, breaker, bulkhead, timeout);
_client = new ReliableRestClientWrapper(new RestClient(BaseUrl), policyWrap);
```
Check [Polly documentation](https://github.com/App-vNext/Polly/wiki) to define the policies needed in each case correctly
You are absolutely free to configure Polly's policies so you can decide what exceptions or even error codes, how many times you want to retry,  how much time you want to wait between retries, timeout of the whole operation, etc. you want to apply.

