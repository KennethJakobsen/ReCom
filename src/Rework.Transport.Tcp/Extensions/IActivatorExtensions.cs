using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReWork.Activation;
using ReWork.Config;

namespace Rework.Transport.Tcp.Extensions
{
    public static class IActivatorExtensions
    {
        public static void UseTcpTransport(this IActivator activator)
        {
            Bootstrapper.RegisterServices(activator);
        }
    }
}
