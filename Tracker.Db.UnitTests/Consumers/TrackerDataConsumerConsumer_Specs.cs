using Contracts;
using Contracts.Enums;
using Contracts.Responses;
using FluentAssertions;
using MassTransit.Configuration;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SharedLib.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Db.Entities;
using static Moq.It;
using static Moq.Mock;
using static Moq.Times;

namespace Tracker.Db.Consumers
{
    [Category("Consumers, Tracker.Db")]
    [TestFixture]
    public class TrackerDataConsumer_Specs
    {
        readonly Mock<ITrackerDbRepo> _trackerDbRepoMock;

        public TrackerDataConsumer_Specs()
        {
            _trackerDbRepoMock = new Mock<ITrackerDbRepo>();
        }

        [Test]
        public async Task Should_throw_notfoundexception_if_map_item_not_found()
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new TrackerDataConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<TrackerDataConsumer>>(), TestInstances.PeriodOptions));

            try
            {
                await harness.Start();

                var trackerId = Guid.NewGuid().ToString();

                _trackerDbRepoMock
                    .Setup(x => x.GetMapItemByTrackerId(IsAny<string>()))
                    .ReturnsAsync((MapItem)null);

                await harness.InputQueueSendEndpoint.Send(new TrackerDataMessage { TrackerId = trackerId });

                //
                var consumed = await consumerHarness.Consumed.SelectAsync<TrackerDataMessage>().FirstOrDefault();

                //
                consumed.Should().NotBeNull();
                consumed.Exception.Should().BeOfType<NotFoundException>();
            }
            finally
            {
                await harness.Stop();
            }
        }

        [TestCase(MapItemType.ShuntingLocomotive)]
        public async Task Should_publish_checknotification_on_mapitem_type(MapItemType mapItemtype)
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new TrackerDataConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<TrackerDataConsumer>>(), TestInstances.PeriodOptions));

            try
            {
                await harness.Start();

                var trackerId = Guid.NewGuid().ToString();

                _trackerDbRepoMock
                    .Setup(x => x.GetMapItemByTrackerId(IsAny<string>()))
                    .ReturnsAsync(new MapItem { Type = mapItemtype });

                var correlationId = Guid.NewGuid();

                await harness.InputQueueSendEndpoint.Send<TrackerDataMessage>(new { TrackerId = trackerId, __CorrelationId = correlationId });

                //
                var consumed = await consumerHarness.Consumed.SelectAsync<TrackerDataMessage>(filter => filter.Context.CorrelationId == correlationId).FirstOrDefault();
            }
            finally
            {
                await harness.Stop();
            }
        }

        [TestCase(MapItemType.Arrow)]
        [TestCase(MapItemType.Brake)]
        [TestCase(MapItemType.Shunter)]
        [TestCase(MapItemType.TrafficLight)]
        [TestCase(MapItemType.Undefined)]
        public async Task Should_NOT_publish_checknotification_on_mapitem_type(MapItemType mapItemtype)
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new TrackerDataConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<TrackerDataConsumer>>(), TestInstances.PeriodOptions));

            try
            {
                await harness.Start();

                var trackerId = Guid.NewGuid().ToString();

                _trackerDbRepoMock
                    .Setup(x => x.GetMapItemByTrackerId(IsAny<string>()))
                    .ReturnsAsync(new MapItem { Type = mapItemtype });

                await harness.InputQueueSendEndpoint.Send(new TrackerDataMessage { TrackerId = trackerId });

                //
                var consumed = await consumerHarness.Consumed.SelectAsync<TrackerDataMessage>(filter => ((TrackerDataMessage)filter.MessageObject).TrackerId == trackerId).FirstOrDefault();
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Test]
        public async Task Should_publish_uitrackerdatamessage()
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new TrackerDataConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<TrackerDataConsumer>>(), TestInstances.PeriodOptions));

            try
            {
                await harness.Start();

                var trackerId = Guid.NewGuid().ToString();

                _trackerDbRepoMock
                    .Setup(x => x.GetMapItemByTrackerId(IsAny<string>()))
                    .ReturnsAsync(new MapItem());

                await harness.InputQueueSendEndpoint.Send(new TrackerDataMessage 
                {
                    TrackerId = trackerId, 
                    TrackDate = DateTimeOffset.Now 
                });

                //
                var consumed = await consumerHarness.Consumed.SelectAsync<TrackerDataMessage>(filter => ((TrackerDataMessage)filter.MessageObject).TrackerId == trackerId).FirstOrDefault();

                //
                bool published = await harness.Published.SelectAsync<UiTrackerDataMessage>(filter => ((UiTrackerDataMessage)filter.MessageObject).TrackerDataMessage.TrackerId == trackerId).Any();
                published.Should().BeTrue();
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Test]
        public async Task Should_add_rawgeopoint_and_respond_with_the_id()
        {
            //
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer(() => new TrackerDataConsumer(TestInstances.Mapper, _trackerDbRepoMock.Object, Of<ILogger<TrackerDataConsumer>>(), TestInstances.PeriodOptions));

            try
            {
                await harness.Start();

                var trackerId = Guid.NewGuid().ToString();

                var id = Guid.NewGuid();

                _trackerDbRepoMock
                    .Setup(x => x.GetMapItemByTrackerId(IsAny<string>()))
                    .ReturnsAsync(new MapItem());

                _trackerDbRepoMock
                    .Setup(x => x.AddRawGeoPoint(IsAny<RawGeoData>()))
                    .ReturnsAsync(id);

                await harness.InputQueueSendEndpoint.Send(new TrackerDataMessage { TrackerId = trackerId });

                //
                var consumed = await consumerHarness.Consumed.SelectAsync<TrackerDataMessage>(filter => ((TrackerDataMessage)filter.MessageObject).TrackerId == trackerId).FirstOrDefault();

                //
                _trackerDbRepoMock.Verify(x => x.AddRawGeoPoint(IsAny<RawGeoData>()), Once);

                bool responded = await harness.Published.SelectAsync<TrackerDataResponse>(filter => ((TrackerDataResponse)filter.MessageObject).Id == id).Any();
                responded.Should().BeTrue();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}