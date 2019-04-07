using System;

namespace MP.Common.Infrastructure.Wcf.Client
{
    internal class WcfServiceProxyWithRetries<TContract> : IServiceProxy<TContract> where TContract : class
    {

        private readonly WcfServiceProxy<TContract> serviceProxy;

        public WcfServiceProxyWithRetries(IChannelProxyGenerator _channelProxyGenerator)
        {
            serviceProxy = new WcfServiceProxy<TContract>();
            serviceProxy.OnError += (o, exception) => OnError(this,exception);
            Channel = _channelProxyGenerator.CreateCannelWithRetries(serviceProxy);
        }


        public TContract Channel { get; private set; }

        public event EventHandler<Exception> OnError = delegate { };
        public void Dispose()
        {
            serviceProxy.Dispose();
        }
    }
}
