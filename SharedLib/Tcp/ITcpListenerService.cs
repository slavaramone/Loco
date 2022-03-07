using System.Threading;

namespace Tracker.Wialon.Tcp
{
    public interface ITcpListenerService
    {
        void Start(CancellationToken cancellationToken);
    }
}
