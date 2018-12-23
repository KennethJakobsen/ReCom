﻿namespace ReWork.Config.Roles
{
    public class ReWorkClientRole : IReWorkRole
    {
        public ReWorkClientRole(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; }
        public int Port { get; }
        public string Id { get; set; }
    }
}
