using Castle.DynamicProxy;


namespace MP.Common.Infrastructure.Wcf.Client
{
    public interface IChannelProxyGenerator
    {
        TInterface CreateCannelWithRetries<TInterface>(IServiceProxy<TInterface> serviceProxy)
            where TInterface:class ;
    }

    public class ChannelProxyGenerator : IChannelProxyGenerator
    {
        
        private readonly ProxyGenerator proxyGenerator;

        public ChannelProxyGenerator()
        {
            
            proxyGenerator = new ProxyGenerator();
        }

        public TInterface CreateCannelWithRetries<TInterface>(IServiceProxy<TInterface> serviceProxy) where TInterface : class
        {
            return proxyGenerator.CreateInterfaceProxyWithTargetInterface(serviceProxy.Channel, new ClientProxyRetryInterceptor());
        }
    }
}
