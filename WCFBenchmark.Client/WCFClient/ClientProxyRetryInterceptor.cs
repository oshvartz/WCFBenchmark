using System;
using Castle.DynamicProxy;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using WCFBenchmark.Client.WCFClient;

namespace MP.Common.Infrastructure.Wcf.Client
{
    public class ClientProxyRetryInterceptor : IInterceptor
    {
        readonly RetryPolicy<WcfTransientErrorDetectionStrategy> retryPolicy;

        const int DEFAULT_RETRY_COUNT = 5;
        const string DEFAULT_RETRY_COUNT_KEY = "ClientProxyRetryCount";
        const int DEFAULT_RETRY_INTERVAL_SEC = 2;
        const string DEFAULT_RETRY_INTERVAL_SEC_KEY = "ClientProxyRetryIntervalSeconds";

        public ClientProxyRetryInterceptor()
        {
            var retryCount = 3;
            var retryIntervalSec = 1;
            RetryStrategy strategy = new FixedInterval(retryCount,TimeSpan.FromSeconds(retryIntervalSec));

            retryPolicy = new RetryPolicy<WcfTransientErrorDetectionStrategy>(strategy);
        }

        public void Intercept(IInvocation invocation)
        {
            Action request = () => invocation.ReturnValue = invocation.Method.Invoke(invocation.InvocationTarget, invocation.Arguments);   
            retryPolicy.ExecuteAction(request);
        }
    }
}
