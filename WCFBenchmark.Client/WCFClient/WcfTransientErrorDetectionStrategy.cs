using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFBenchmark.Client.WCFClient
{
    public class WcfTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {

        private const int INNER_EXCEPTION_DEPTH_LIMIT = 20;

        //private static ILog _log = LogProvider.GetLogger(typeof(WcfTransientErrorDetectionStrategy));

        private readonly static List<Type> TransientExceptions = new List<Type>
            {
                typeof(TimeoutException),
                typeof(CommunicationException),

            };


        public bool IsTransient(Exception ex)
        {
            if (GetInnerExceptions(ex).Any(e => TransientExceptions.Contains(e.GetType())))
            {
              //  _log.WarnException("Received transient exception, retrying.", ex);
                return true;
            }
            return false;
        }

        private IEnumerable<Exception> GetInnerExceptions(Exception ex)
        {
            int depth = 0;
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var innerException = ex;
            do
            {
                yield return innerException;

                innerException = innerException.InnerException;
            }
            while (innerException != null && depth++ < INNER_EXCEPTION_DEPTH_LIMIT);
        }
    }
}
