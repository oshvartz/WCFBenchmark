using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Common.Infrastructure.Wcf.Client
{
    public interface IServiceProxyFactory
    {
        IServiceProxy<TContract> CreateServiceProxy<TContract>() where TContract : class;
    }
}
