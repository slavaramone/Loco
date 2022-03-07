using System.Threading;
using System.Threading.Tasks;

namespace SharedLib.Tcp
{
	public interface IProcessor
	{
		Task ProcessRequest(CancellationToken cancellationToken);
	}
}