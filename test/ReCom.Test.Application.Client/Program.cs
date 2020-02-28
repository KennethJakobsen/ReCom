using System;
using System.Net.Sockets;
using System.Threading;
using ReCom.Transport.Tcp.Extensions;
using ReCom.Activation;
using ReCom.Config;
using ReCom.Config.Roles;
using ReCom.Handlers;
using ReCom.SystemMessages;
using ReCom.Test.Application.Client.Handlers;
using ReCom.Test.Common.Domain;

namespace ReCom.Test.Application.Client
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
                activator.Register<ICommand<ReceivedMessage>, MessageReceivedCommandHandler>();
                activator.Register<ICommand<HandledMessage>, MessageHandledCommandHandler>();
                var connection = Configure
                    .With(activator)
                    .Transport(t => t.UseTcpTransport())
                    .Start(new ReWorkClientRole("127.0.0.1", 13000));

                while (true)
                {
                    connection.Send(new PingPongCommand() { Message = "Ping!" }, true, true).Wait();
                    Console.ReadLine();
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
