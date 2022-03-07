using Moq;
using NUnit.Framework;

namespace Notification.Db
{
	[Category("Consumers, Notification.Db")]
    [TestFixture]
    public class TrackerDateConsumer_Specs
    {
        readonly Mock<INotificationDbRepo> _notificationDbRepo;

        public TrackerDateConsumer_Specs()
        {
            _notificationDbRepo = new Mock<INotificationDbRepo>();
        }
    }
}