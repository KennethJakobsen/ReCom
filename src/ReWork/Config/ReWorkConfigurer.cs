using ReWork.Config.Roles;
using ReWork.Connectivity;

namespace ReWork.Config
{
    public class ReWorkConfigurer
    {
        private readonly IConnectionManager _connectionManager;

        public ReWorkConfigurer(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Start(ReWorkServerRole role)
        {
            _connectionManager.StartListening(role).Wait();
        }

        public Connection Start(ReWorkClientRole role)
        {
            return _connectionManager.Connect(role);
        }
    }
}
