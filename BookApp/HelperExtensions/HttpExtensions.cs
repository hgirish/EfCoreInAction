using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace BookApp.HelperExtensions
{
    public static  class HttpExtensions
    {
        private const string NullIpAddress = "::1";

        public static bool IsLocal(this HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress.IsSet())
            {
                return connection.LocalIpAddress.IsSet()
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }
            return true;
        }

        public static void  ThrowErrorIfNotLocal(this HttpRequest req)
        {
            if (!req.IsLocal())
            {
                throw new InvalidOperationException("You can only call this command if you are running locally");
            }
        }

        private static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }
    }
}
