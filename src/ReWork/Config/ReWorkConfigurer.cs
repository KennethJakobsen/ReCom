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
            Start().StartListening(role).Wait();
        }

        public Connection Start(ReWorkClientRole role)
        {
            return Start().Connect(role).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private IConnectionManager Start()
        {
            return _activator.GetInstance<IConnectionManager>();
        }
    }
}
