using Contracts.Events;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Commands;
using static Moq.Mock;

namespace Tracker.Db.Consumers
{
    [Category("Consumers, Tracker.Db")]
    [TestFixture]
    public class ReceiveMapItemsConsumer_Specs
    {
        readonly Mock<ITrackerDbRepo> _trackerDbRepoMock;

        public ReceiveMapItemsConsumer_Specs()
        {
            _trackerDbRepoMock = new Mock<ITrackerDbRepo>();
        }

        [Test]
        public async Task Should_get_all_dynamicmaptrackerid()
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new ReceiveMapItemsConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<ReceiveMapItemsConsumer>>()));

            try
            {
                await harness.Start();

                var dynamicTrackingIds = Enumerable.Range(1, TestInstances.Random.Next(1, 20)).Select(i =>
                {
                    return Guid.NewGuid().ToString();
                }).ToList();

                _trackerDbRepoMock
                    .Setup(x => x.GetDynamicMapItemTrackIds())
                    .ReturnsAsync(dynamicTrackingIds);

                //
                await harness.InputQueueSendEndpoint.Send<ReceiveMapItems>(new { });

                //
                await consumerHarness.Consumed.Any<ReceiveMapItems>();

                //
                var published = harness.Published.Select<MapItemsReceived>().Single();
                published.Context.Message.MapItemTrackIds.Should().HaveSameCount(dynamicTrackingIds);
            }
            finally
            {
                await harness.Stop();
            }

        }
    }
}
