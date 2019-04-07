using System;


namespace MP.Common.Infrastructure.Wcf.Client
{
    public class ServiceProxyFactoryWithRetries : IServiceProxyFactory
    {
        
        private readonly IChannelProxyGenerator channelProxyGenerator;

        public ServiceProxyFactoryWithRetries( IChannelProxyGenerator channelProxyGenerator)
        {
            this.channelProxyGenerator = channelProxyGenerator;
        }

        public IServiceProxy<T> CreateServiceProxy<T>() where T : class
        {
            return new WcfServiceProxyWithRetries<T>(channelProxyGenerator);
        }
    }
}