using Contracts.Commands;
using Contracts.Events;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static Moq.Mock;

namespace Tracker.Glosav.Consumers
{
    [Category("Consumers, Tracker.Glosav")]
    [TestFixture]
    public class MapItemsReceivedConsumer_Specs
    {
        [Test]
        public async Task Should_send_receiveglosavdevices()
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new MapItemsReceivedConsumer(TestInstances.Mapper, Of<ILogger<MapItemsReceivedConsumer>>()));

            try
            {
                await harness.Start();

                await harness.InputQueueSendEndpoint.Send<MapItemsReceived>(new { });

                //
                await consumerHarness.Consumed.Any<MapItemsReceived>();

                //
                var sent = harness.Sent.Select<ReceiveGlosavDevices>().Single();
                sent.Context.DestinationAddress.AbsolutePath.Should().Contain("Glosav.ReceiveGlosavDevices");
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}