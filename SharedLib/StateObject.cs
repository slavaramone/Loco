using System.Net.Sockets;
using System.Text;

namespace SharedLib
{
    public class StateObject
	{
		public Socket workSocket = null;
		public const int BufferSize = 4096;
		public byte[] buffer = new byte[BufferSize];
		public StringBuilder sb = new StringBuilder();
	}
}
