using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharedLib.Tcp
{
	public abstract class TcpServer : IDisposable
	{
		private readonly ILogger<TcpServer> _logger;

		/// <summary>
		/// Maximum time, in milliseconds, to wait on the
		/// listener task when shutting down
		/// </summary>
		public int MaxShutDownTime { get; set; } = 1000;

		// ReSharper disable once MemberCanBePrivate.Global

		/// <summary>
		/// Port which this server has bound to
		/// </summary>
		public int Port { get; }

		private System.Net.Sockets.TcpListener _listener;
		private Task _task;
		private readonly object _lock = new object();


		/// <summary>
		/// Construct the server with the explicitly-provided port
		/// </summary>
		/// <param name="port"></param>
		/// <param name="logger"></param>
		protected TcpServer(int port, ILogger<TcpServer> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			Port = port;
			Init();
		}

		/// <summary>
		/// Override in derived classes: this initializes the server
		/// system
		/// </summary>
		protected abstract void Init();

		/// <summary>
		/// Provides a convenience logging mechanism which outputs via
		/// the established LogAction
		/// </summary>
		/// <param name="message"></param>
		/// <param name="parameters"></param>
		protected void Log(string message, params object[] parameters)
		{
			_logger.LogTrace(string.Format(message, parameters));
		}

		/// <summary>
		/// Create a processor for a particular TCP client
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		protected abstract IProcessor CreateProcessorFor(TcpClient client);

		private ConcurrentDictionary<IProcessor, Task> _processors = new ConcurrentDictionary<IProcessor, Task>();

		/// <summary>
		/// Start the server
		/// </summary>
		public void Start(CancellationToken cancellationToken)
		{
			lock (_lock)
			{
				DoStop();
				AttemptToBind();
				var task = Task.Run(() => AcceptClientsAsync(cancellationToken), cancellationToken);
			}
		}

		private async Task AcceptClientsAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					AcceptClientAsync(cancellationToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Cannot accept client");
				}
			}
		}

		private void AcceptClientAsync(CancellationToken cancellationToken)
		{
			var client = _listener.AcceptTcpClientAsync().Result;

			_logger.LogInformation($"Accepted client: {client.Client.RemoteEndPoint}");

			var processor = CreateProcessorFor(client);

			var task = new Task(() => processor.ProcessRequest(cancellationToken));

			_logger.LogInformation($"Processing request");

			task.Start();
		}

		private void AttemptToBind()
		{
			_listener = new System.Net.Sockets.TcpListener(IPAddress.Any, Port);

			try
			{
				_logger.LogInformation($"Try to bind to port: {Port}");

				_listener.Start();

				_logger.LogInformation($"Bound to port: {Port}");
			}
			catch
			{
				_logger.LogError("Cannot bind to port:");
				throw new PortUnavailableException(Port);
			}
		}


		/// <summary>
		/// Stop the server
		/// </summary>
		public void Stop()
		{
			lock (_lock)
			{
				DoStop();
			}
		}

		private void DoStop()
		{
			try
			{
				var listener = _listener;

				_listener = null;
				if (listener == null)
				{
					return;
				}

				listener.Stop();

				try
				{
					if (!_task.Wait(MaxShutDownTime))
					{
						Debug.WriteLine(
							$"TcpServer did not shut down gracefully within ${MaxShutDownTime}ms"
						);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error on task shutdown");
				}
			}
			catch (Exception ex)
			{
				Log($"Internal DoStop() fails: {ex.Message}");
			}
		}

		/// <summary>
		/// Disposes the server (stops it if it is running)
		/// </summary>
		public virtual void Dispose()
		{
			Stop();
		}
	}
}