using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SharedLib
{
    public abstract class TcpListener : ITcpListener
	{
		private const int DefaultTcpPort = 8081;

		public static ManualResetEvent allDone = new ManualResetEvent(false);
		protected readonly TcpListenerOptions _tcpListenerOptions;
		protected readonly ILogger<TcpListener> _logger;

		public TcpListener(IOptions<TcpListenerOptions> tcpListenerOptions, ILogger<TcpListener> logger)
		{
			_tcpListenerOptions = tcpListenerOptions.Value;
			_logger = logger;
		}

		public void Listen()
		{
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, _tcpListenerOptions.Port != 0 ? _tcpListenerOptions.Port : DefaultTcpPort);
			Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				listener.Bind(localEndPoint);
				listener.Listen(100);

				while (true)
				{
					allDone.Reset();

					_logger.LogInformation("Waiting for a connection...");
					listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

					allDone.WaitOne();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public void AcceptCallback(IAsyncResult ar)
		{
			allDone.Set();
			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			StateObject state = new StateObject();
			state.workSocket = handler;
			handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
		}

		public void ReadCallback(IAsyncResult ar)
		{
			try
			{
				string content = string.Empty;
				StateObject state = (StateObject)ar.AsyncState;
				Socket handler = state.workSocket;

				int bytesRead = handler.EndReceive(ar);
				if (bytesRead > 0)
				{
					ProcessReceivedBytes(state, bytesRead);

					_logger.LogInformation($"UTF8:{content}");
					_logger.LogInformation($"Base64:{Convert.ToBase64String(state.buffer, 0, bytesRead)}");
					if (content.IndexOf("<EOF>") > -1)
					{
						_logger.LogInformation($"Read {content.Length} bytes from socket.");
					}
					else
					{
						handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}

		public abstract void ProcessReceivedBytes(StateObject state, int bytesRead);
	}
}
