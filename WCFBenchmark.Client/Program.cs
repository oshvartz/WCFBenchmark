using App.Metrics;
using App.Metrics.Formatters.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFBenchmark.Contracts;

namespace WCFBenchmark.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            var metrics = new MetricsBuilder()
            .Report.ToTextFile(
              ops =>
              {
                  ops.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                  ops.AppendMetricsToTextFile = true;
                  ops.FlushInterval = TimeSpan.FromSeconds(1);
                  ops.OutputPathAndFileName = @"c:/metrics/wcf_benchmark.txt";
              })
           .Build();

            RunReportSchecduler(metrics);

            Thread.Sleep(3000);
            ChannelFactory<IBenchmarkService> channelFactory = new ChannelFactory<IBenchmarkService>("BenchmarkService");

            var proxy = channelFactory.CreateChannel();
            var response = proxy.Call(new BenchmarkRequest { RequestId = 1, RequestTimestamp = DateTime.UtcNow });

            ((IClientChannel)proxy).Close();


        }

        private async static void RunReportSchecduler(IMetricsRoot metrics)
        {
            while (true)
            {
                try
                {
                    await Task.WhenAll(metrics.ReportRunner.RunAllAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                await Task.Delay(1000);
            }
        }

    }
}
