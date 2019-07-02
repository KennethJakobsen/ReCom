using System;
using System.Threading.Tasks;
using ReWork.Activation;
using ReWork.Connectivity;
using ReWork.Handlers.BuiltIn;
using ReWork.SystemMessages;

namespace ReWork.Handlers
{
    public class HandlerDispatcher : IHandlerDispatcher
    {
        private readonly IActivator _activator;

        public HandlerDispatcher(IActivator activator)
        {
            _activator = activator;
        }

      
        public async Task Execute(object command, Connection connection)
        {
            if (command is IReWorkSystemMessage)
                await Dispatch(command, connection, typeof(IHandle<>));
            
            await Dispatch(command, connection, typeof(ICommand<>));
        }

        private async Task Dispatch(object command, Connection connection, Type baseInteface)
        {
            var type = baseInteface.MakeGenericType(command.GetType());
            if (command is IReWorkSystemMessage && !_activator.HasRegistration(type)) return;

            var instance = _activator.GetInstance(type);
            var method = type.GetMethod("Handle");
            if (method != null)
                await (Task) method.Invoke(instance, new[] {command, connection});

            
        }
    }
}
