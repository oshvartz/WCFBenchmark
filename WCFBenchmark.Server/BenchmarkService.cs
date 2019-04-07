using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFBenchmark.Contracts;

namespace WCFBenchmark.Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class BenchmarkService : IBenchmarkService
    {
        public BenchmarkResponse Call(BenchmarkRequest benchmarkRequest)
        {
       //     Thread.Sleep(2);
            return new BenchmarkResponse { ProcessEndTime = DateTime.UtcNow, RequestId = benchmarkRequest.RequestId };
        }
    }
}
