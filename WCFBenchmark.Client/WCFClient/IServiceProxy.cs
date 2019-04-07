using System;

namespace MP.Common.Infrastructure.Wcf.Client
{
    public interface IServiceProxy<out T> : IDisposable where T:class
    {
        T Channel { get; }

        event EventHandler<Exception> OnError;

    }
}
