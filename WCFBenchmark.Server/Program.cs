using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WCFBenchmark.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Workaround the CLR Threadpool issue
            ThreadPoolTimeoutWorkaround.DoWorkaround();
            IContainer container = CreateContainer();

            var benchmarkService = new ServiceHost(typeof(BenchmarkService));
            benchmarkService.AddDependencyInjectionBehavior<BenchmarkService>(container);
            benchmarkService.Open();
            Console.WriteLine("service is up");
            Console.ReadLine();
        }

        private static IContainer CreateContainer()
        {
            // Create your builder.
            var builder = new ContainerBuilder();

            builder.RegisterType<BenchmarkService>();

            // Usually you're only interested in exposing the type
            // via its interface:
            // builder.RegisterType<SomeType>().As<IService>();

            return builder.Build();
        }
    }

    static class ThreadPoolTimeoutWorkaround
    {
        static ManualResetEvent s_dummyEvent;
        static RegisteredWaitHandle s_registeredWait;

        public static void DoWorkaround()
        {
            // Create an event that is never set
            s_dummyEvent = new ManualResetEvent(false);

            // Register a wait for the event, with a periodic timeout. This causes callbacks
            // to be queued to an IOCP thread, keeping it alive
            s_registeredWait = ThreadPool.RegisterWaitForSingleObject(
                s_dummyEvent,
                (a, b) =>
                {
                // Do nothing
            },
                null,
                1000,
                false);
        }
    }
}
