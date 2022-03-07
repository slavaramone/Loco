using System;

namespace SharedLib.Exceptions
{
    public class HubAuthException : Exception
    {
        public HubAuthException(string name) : base($"Hub {name} request is not authorized")
        {
        }
    }
}
