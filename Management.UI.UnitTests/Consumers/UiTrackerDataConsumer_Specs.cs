using Contracts;
using Management.Ui.Consumers;
using Management.Ui.Hubs;
using MassTransit.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using static Moq.Mock;

namespace Management.UI.Consumers
{
	[Category("Consumers, Management.UI")]
    [TestFixture]
    public class UiTrackerDataConsumer_Specs
    {
        readonly Mock<IHubContext<ArmHub>> _armHubContext;
        readonly Mock<IHubContext<DispatcherHub>> _dispatcherHubContext;

        public UiTrackerDataConsumer_Specs()
        {
            _armHubContext = new Mock<IHubContext<ArmHub>>();
            _dispatcherHubContext = new Mock<IHubContext<DispatcherHub>>();
        }

        [Test]
        public async Task Should_send_coordinates_to_all_cients_connected_to_hub()
        {
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new UiTrackerDataConsumer(_armHubContext.Object, _dispatcherHubContext.Object, TestInstances.Mapper, Of<ILogger<UiTrackerDataConsumer>>()));

            try
            {
                await harness.Start();

                await harness.InputQueueSendEndpoint.Send<UiTrackerDataMessage>(new { });

                Assert.That(await consumerHarness.Consumed.Any<UiTrackerDataMessage>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
