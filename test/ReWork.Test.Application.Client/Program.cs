using System;
using System.Net.Sockets;
using System.Threading;
using Rework.Transport.Tcp.Extensions;
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
                activator.Register<ICommand<ReceivedMessage>, MessageReceivedCommandHandler>();
                activator.Register<ICommand<HandledMessage>, MessageHandledCommandHandler>();
                var connection = Configure
                    .With(activator)
                    .Transport(t => t
                        .UseTcpTransport())
                    .Start(new ReWorkClientRole("127.0.0.1", 13000));

                for(int i = 1; i <= 100000; i++)
                {
                    connection.Send(new PingPongCommand() { Message = "Message " + i }, true, true).Wait();
                    Console.WriteLine(i);
                    Thread.Sleep(200);
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
            //Console.Read();
        }
    }
}
