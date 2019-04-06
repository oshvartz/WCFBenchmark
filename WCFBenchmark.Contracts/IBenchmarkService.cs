using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFBenchmark.Contracts
{
    [ServiceContract]
    public interface IBenchmarkService
    {
        [OperationContract]
        BenchmarkResponse Call(BenchmarkRequest benchmarkRequest);
    }
}
