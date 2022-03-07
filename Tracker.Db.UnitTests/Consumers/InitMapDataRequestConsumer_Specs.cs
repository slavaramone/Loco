using Contracts.Requests;
using Contracts.Responses;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Db.Entities;
using static Moq.Mock;

namespace Tracker.Db.Consumers
{
    [Category("Consumers, Tracker.Db")]
    [TestFixture]
    public class InitMapDataRequestConsumer_Specs
    {
        readonly Mock<ITrackerDbRepo> _trackerDbRepoMock;

        public InitMapDataRequestConsumer_Specs()
        {
            _trackerDbRepoMock = new Mock<ITrackerDbRepo>();
        }

        [Test]
        public async Task Should_get_all_mapitems()
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new InitMapDataRequestConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<InitMapDataRequestConsumer>>()));

            try
            {
                await harness.Start();

                var mapItems = Enumerable.Range(1, TestInstances.Random.Next(1, 20)).Select(i =>
                {
                    return (new MapItem(), new RawGeoData());
                }).ToList();

                _trackerDbRepoMock
                    .Setup(x => x.GetMapItemsWithLatestGeoData())
                    .ReturnsAsync(mapItems);

                await harness.InputQueueSendEndpoint.Send(new InitMapDataRequest { });

                //
                await consumerHarness.Consumed.Any<InitMapDataRequest>();

                //
                var published = harness.Published.Select<InitMapDataResponse>().Single();
                published.Context.Message.MapItems.Should().HaveSameCount(mapItems);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}