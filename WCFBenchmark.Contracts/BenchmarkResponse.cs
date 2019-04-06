using System;
using System.Runtime.Serialization;

namespace WCFBenchmark.Contracts
{
    [DataContract]
    public class BenchmarkResponse
    {
        [DataMember]
        public long RequestId { get; set; }

        [DataMember]
        public DateTime ProcessEndTime { get; set; }
    }
}