using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfContrib.Client;

namespace MP.Common.Infrastructure.Wcf.Client
{
    internal class WcfServiceProxy<TContract> : ClientChannel <TContract> ,IServiceProxy<TContract> 
        where TContract:class
    {
        public event EventHandler<Exception> OnError = delegate { };
        public WcfServiceProxy():base(WcfContrib.Client.ChannelManageOptions.RenewFaultedChannel)
        {

        }

        protected override InvokeErrorAction OnInvokeException(ChannelExceptionEventArgs args)
        {
            OnError(this, args.Exception);
            return base.OnInvokeException(args);
        }
    }
}
