using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReCom.Activation;
using ReCom.Config;

namespace ReCom.Transport.Tcp.Extensions
{
    public static class IActivatorExtensions
    {
        public static void UseTcpTransport(this IActivator activator)
        {
            Bootstrapper.RegisterServices(activator);
        }
    }
}
