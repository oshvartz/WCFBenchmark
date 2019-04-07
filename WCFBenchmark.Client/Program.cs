using App.Metrics;
using App.Metrics.Formatters.Json;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using MP.Common.Infrastructure.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
           // IServiceProxyFactory channelFactory = new ServiceProxyFactoryWithRetries(new ChannelProxyGenerator());
            var channelFactory = new ChannelFactory<IBenchmarkService>("BenchmarkService");



            var recieveMsgCounter = new MeterOptions
            {
                Name = "Calls",
                MeasurementUnit = App.Metrics.Unit.Calls
            };


            var requestTimer = new TimerOptions
            {
                Name = "Call Timer",
                MeasurementUnit = Unit.Requests,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };

            bool IsIdle = false;

            ActionBlock<bool> callServiceActionBlock = new ActionBlock<bool>(_ =>
           {

               if (IsIdle)
               {
                   Console.Write(".");
                   return;
               }
               try
               {
                   using (metrics.Measure.Timer.Time(requestTimer))
                   {

                       //using (var proxy = channelFactory.CreateServiceProxy<IBenchmarkService>())
                       //{
                       //    metrics.Measure.Meter.Mark(recieveMsgCounter);
                       //    var response = proxy.Channel.Call(new BenchmarkRequest { RequestId = 1, RequestTimestamp = DateTime.UtcNow });
                       //}

                       var proxy = channelFactory.CreateChannel();

                       metrics.Measure.Meter.Mark(recieveMsgCounter);
                       var response = proxy.Call(new BenchmarkRequest { RequestId = 1, RequestTimestamp = DateTime.UtcNow });
                       try
                       {
                           ((IClientChannel)proxy).Close();
                       }
                       catch
                       {
                           ((IClientChannel)proxy).Abort();
                       }
                   }
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex);
               }

             

           }, new ExecutionDataflowBlockOptions
           {
               MaxDegreeOfParallelism = Environment.ProcessorCount,
               BoundedCapacity = 2000
           });

            var timer = new System.Timers.Timer(16 * 1000);
            timer.AutoReset = true;
            timer.Elapsed += (s, e) => IsIdle = !IsIdle;
            timer.Start();


            while (true)
            {
                callServiceActionBlock.Post(true);
            }

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
