using System;

namespace ReWork.Activation
{
    internal class ServiceFactory
    {
        public ServiceFactory(bool keepAlive, Func<object> factory)
        {
            KeepAlive = keepAlive;
            Create = factory;
        }

        public Func<object> Create { get;  }
        public bool KeepAlive { get;  }
    }
}
