using System;
using System.Net.Sockets;

namespace ReWork.Connectivity
{
    public interface IConnectionFactory
    {
        Connection Create(TcpClient client, Guid clientId);
    }
}