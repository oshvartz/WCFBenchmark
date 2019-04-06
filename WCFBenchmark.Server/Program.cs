using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFBenchmark.Server
{
    class Program
    {
        static void Main(string[] args)
        {
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
}
