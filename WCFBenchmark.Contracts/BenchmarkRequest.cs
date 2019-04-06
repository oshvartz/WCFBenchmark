using System;
using System.Runtime.Serialization;

namespace WCFBenchmark.Contracts
{

    [DataContract]
    public class BenchmarkRequest
    {
        [DataMember]
        public long RequestId { get; set; }

        [DataMember]
        public DateTime RequestTimestamp { get; set; }
    }
}