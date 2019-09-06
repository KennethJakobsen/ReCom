using System;
using ReWork.Activation;
using ReWork.Config.Roles;
using ReWork.Connectivity;

namespace ReWork.Config
{
    public class ReWorkConfigurer
    {
        private IActivator _activator;

        public ReWorkConfigurer(IActivator activator)
        {
            _activator = activator;
        }

        internal void SetActivator(IActivator activator)
        {
            _activator = activator;
        }
        public ReWorkConfigurer Transport(Action<IActivator> transport)
        {
            transport(_activator);
            return this;
        }
        public void Start(ReWorkServerRole role)
        {
            StartServer().StartListening(role).Wait();
        }

        public Connection Start(ReWorkClientRole role)
        {
            return StartClient().Connect(role).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private IClientConnectionManager StartClient()
        {
            return _activator.GetInstance<IClientConnectionManager>();
        }
        private IServerConnectionManager StartServer()
        {
            return _activator.GetInstance<IServerConnectionManager>();
        }
    }
}
