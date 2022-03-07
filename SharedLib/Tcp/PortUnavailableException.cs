using System;

namespace SharedLib.Tcp
{
    public class PortUnavailableException: Exception
    {
        /// <inheritdoc />
        public PortUnavailableException(int port)
            : base($"Can't listen on specified port '{port}': probably already in use?")
        {
        }
    }
}