using ReWork.Activation;
using ReWork.Activation.LifeTime;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.Handlers.BuiltIn;
using ReWork.SystemMessages;

namespace ReWork.Config
{
    public class Bootstrapper
    {
        private readonly IActivator _activator;

        public Bootstrapper(IActivator activator)
        {
            _activator = activator;
        }

        public Bootstrapper()
        {
            _activator = new DefaultActivator();
        }

        public IActivator RegisterServices()
        {
            _activator.Register<IHandle<DisconnectMessage>, BuiltInDisconnectMessageHandler>();
            _activator.Register<IHandlerDispatcher, HandlerDispatcher>();
            _activator.Register<IHandle<WelcomeMessage>, BuiltInWelcomeMessageHandler>();
            _activator.Register<IHandle<CompleteHandshakeMessage>, BuiltInCompleteHandshakeHandler>();
            _activator.Register<IHandle<InitiateHandshakeMessage>, BuiltInInitiateHandshakeHandler>();
            _activator.Register<IHandle<TimeoutMessage>, BuiltInTimeoutMessageHandler>();
            _activator.Register<IHandle<ReceivedMessage>, BuiltInReceivedMessageHandler>();
            _activator.Register<IHandle<HandledMessage>, BuiltInHandledMessageHandler>();
            _activator.Register<IHandle<NotAuthorizedMessage>, BuiltinNotAuthorizedMessageHandler>();
            _activator.Register<IConnectionFactory, ConnectionFactory>(new ActivatorLifeTime());
            _activator.Register<IConnectionManager, ConnectionManager>(new ActivatorLifeTime());
            _activator.Register<ReWorkConfigurer>();
            _activator.Register<IActivator>(_activator);
            return _activator;
        }
    }
}
