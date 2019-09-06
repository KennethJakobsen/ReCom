using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Rework.Transport.Tcp.Configuration;
using ReWork.Activation;
using ReWork.Config;

namespace Rework.Transport.Tcp.Extensions
{
    public static class IActivatorExtensions
    {
        public static void UseTcpTransport(this IActivator activator, Func<TcpTransportConfigurer, TcpTransportConfigurer> config = null)
        {
            if(config == null)
                Bootstrapper.RegisterServices(activator, new TcpTransportConfigurer());
            else
                Bootstrapper.RegisterServices(activator, new TcpTransportConfigurer());
        }
    }
}
