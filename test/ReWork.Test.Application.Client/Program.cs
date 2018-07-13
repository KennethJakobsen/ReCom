using System;
using System.Net.Sockets;
using System.Threading;
using ReWork.Activation;
using ReWork.Config;
using ReWork.Config.Roles;
using ReWork.Handlers;
using ReWork.SystemMessages;
using ReWork.Test.Application.Client.Handlers;
using ReWork.Test.Common.Domain;

namespace ReWork.Test.Application.Client
{
    class Program
    {
        public static void Main()
        {
            Connect("127.0.0.1", "test");
        }
        static void Connect(String server, String message)
        {
            try
            {
                Thread.Sleep(1000);
                
                var activator = new DefaultActivator();
                activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
                activator.Register<ICommand<CompleteHandshakeMessage>, CompleteHandshakeMessageHandler>();
                var connection = Configure
                    .With(activator)
                    .Start(new ReWorkClientRole("127.0.0.1", 13000));

                while (true)
                {
                    connection.Send(new PingPongCommand() { Message = "Ping!" }).Wait();
                    Thread.Sleep(1000);
                }
                

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
