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
            //var act = new DefaultActivator();
            //act.Register<ICommand<HelloWorldCommand>, HelloWorldCommandHandler>();
            //act.Register<IActivator>(act);
            //var factory = new HandlerDispatcher(act);
            //factory.Execute(new HelloWorldCommand() {Message = "test"}).Wait();
            Connect("127.0.0.1", "test");
        }
        static void Connect(String server, String message)
        {
            try
            {
                Thread.Sleep(1000);
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                var activator = new DefaultActivator();
                activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
                activator.Register<ICommand<WelcomeMessage>, WelcomeMessageHandler>();
                var connection = Configure
                    .With(activator)
                    .Start(new ReWorkClientRole("127.0.0.1", 13000));

                while (true)
                {
                    connection.Send(new PingPongCommand() { Message = "Ping!" }).Wait();
                    Thread.Sleep(1000);
                }
                

                //// Translate the passed message into ASCII and store it as a Byte array.
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                //// Get a client stream for reading and writing.
                ////  Stream stream = client.GetStream();

                //NetworkStream stream = client.GetStream();
                //while (true)
                //{
                //    // Send the message to the connected TcpServer. 
                //    stream.Write(data, 0, data.Length);

                //    Console.WriteLine("Sent: {0}", message);

                //    // Receive the TcpServer.response.

                //    // Buffer to store the response var .
                //    data = new Byte[256];

                //    // String to store the response ASCII representation.
                //    String responseData = String.Empty;

                //    // Read the first batch of the TcpServer response bytes.
                //    Int32 bytes = stream.Read(data, 0, data.Length);
                //    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //    Console.WriteLine("Received: {0}", responseData);

                //    Thread.Sleep(2000);
                //}


                //// Close everything.
                //stream.Close();
                //client.Close();
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
