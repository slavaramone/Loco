using Contracts;
using Management.Ui.Consumers;
using Management.Ui.Hubs;
using Management.Ui.Options;
using MassTransit.Configuration;
using MassTransit.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using static Moq.Mock;

namespace Management.UI.Consumers
{
    [Category("Consumers, Management.UI")]
    [TestFixture]
    public class NotificationMessageConsumer_Specs
    {
        readonly Mock<IHubContext<ArmHub>> _armHubContext;
        readonly Mock<IHubContext<DispatcherHub>> _dispatcherHubContext;

        public NotificationMessageConsumer_Specs()
        {
            _armHubContext = new Mock<IHubContext<ArmHub>>();
            _dispatcherHubContext = new Mock<IHubContext<DispatcherHub>>();
        }

        [Test]
        public async Task Should_send_notification_to_all_cients_connected_to_hub()
        {
            var harness = new InMemoryTestHarness();

			IOptions<NotificationOptions> notificationOptions = Options.Create(new NotificationOptions
			{
				LifetimeSeconds = 30
			});
			var consumerHarness = harness.Consumer(() => new NotificationMessageConsumer(_armHubContext.Object, _dispatcherHubContext.Object, TestInstances.Mapper, Of<ILogger<NotificationMessageConsumer>>(), notificationOptions));

            try
            {
                await harness.Start();

                await harness.InputQueueSendEndpoint.Send<NotificationMessage>(new { });

                Assert.That(await consumerHarness.Consumed.Any<NotificationMessage>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}