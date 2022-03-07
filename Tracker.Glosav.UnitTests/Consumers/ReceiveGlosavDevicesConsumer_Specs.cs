using Contracts;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Responses;
using FluentAssertions;
using GreenPipes.Filters;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Monitoring.Client;
using Tracker.Glosav.Api.Monitoring.Models;
using Tracker.Glosav.Api.Monitoring.Models.Request;
using Tracker.Glosav.Api.Monitoring.Models.Response;
using Tracker.Glosav.Helpers;
using static Moq.It;
using static Moq.Mock;
using static Moq.Times;

namespace Tracker.Glosav.Consumers
{
	[Category("Consumers, Tracker.Glosav")]
	[TestFixture]
	public class ReceiveGlosavDevicesConsumer_Specs
	{
		readonly Mock<IRequestClient<TrackerDataMessage>> _requestClientMock;

		readonly Mock<IGlosavMonitoringClient> _glosavMonitoringClientMock;

		public ReceiveGlosavDevicesConsumer_Specs()
		{
			_requestClientMock = new Mock<IRequestClient<TrackerDataMessage>>();
			_glosavMonitoringClientMock = new Mock<IGlosavMonitoringClient>();
		}

		[TearDown]
		public void After_each()
		{
			_requestClientMock.Invocations.Clear();
		}

		[Test]
		public async Task Should_send_request_by_the_count_of_monitoring_messages_returned_from_glosavclient()
		{
			//
			var harness = new InMemoryTestHarness();

			var consumerHarness = harness.Consumer(() => new ReceiveGlosavDevicesConsumer(_requestClientMock.Object,
				_glosavMonitoringClientMock.Object, TestInstances.Mapper, Of<ILogger<ReceiveGlosavDevicesConsumer>>()));

			var latestFilter = Of<ILatestFilter<ConsumeContext<MapItemsReceived>>>(x =>
				x.Latest == Task.FromResult(Of<ConsumeContext<MapItemsReceived>>(y =>
					y.Message == Of<MapItemsReceived>(z =>
						z.MapItemTrackIds == new[] { "1", "2" }))));

			_glosavMonitoringClientMock
				.Setup(x => x.GetLastMonitoringMessages(IsAny<ApiMonitoringMessagesRequestPayload>()))
				.ReturnsAsync(new ApiMonitoringMessagesResponse
				{
					MonitoringMessages = new[]
					{
						new ApiMonitoringMessageModel { Device = new ApiDeviceModel() },
						new ApiMonitoringMessageModel { Device = new ApiDeviceModel() }
					}
				});

			SharedLib.Filters.LatestFilter<MapItemsReceived>.SetLatest(latestFilter);

			try
			{
				await harness.Start();

				await harness.InputQueueSendEndpoint.Send<ReceiveGlosavDevices>(new { });

				//
				var consumed = await consumerHarness.Consumed.Any<ReceiveGlosavDevices>();

				//
				_requestClientMock
					.Verify(
						x => x.GetResponse<TrackerDataResponse>(IsAny<TrackerDataMessage>(), IsAny<CancellationToken>(),
							IsAny<RequestTimeout>()), Never);
			}
			finally
			{
				await harness.Stop();
			}
		}

		[Test]
		public async Task Should_not_send_requests_if_no_trackers_that_match_glosav_template()
		{
			//
			var harness = new InMemoryTestHarness();

			var consumerHarness = harness.Consumer(() => new ReceiveGlosavDevicesConsumer(_requestClientMock.Object,
				_glosavMonitoringClientMock.Object, TestInstances.Mapper, Of<ILogger<ReceiveGlosavDevicesConsumer>>()));

			var latestFilter = Of<ILatestFilter<ConsumeContext<MapItemsReceived>>>(x =>
				x.Latest == Task.FromResult(Of<ConsumeContext<MapItemsReceived>>(y =>
					y.Message == Of<MapItemsReceived>(z =>
						z.MapItemTrackIds == new[] { "1", "2" }))));

			SharedLib.Filters.LatestFilter<MapItemsReceived>.SetLatest(latestFilter);

			try
			{
				await harness.Start();

				await harness.InputQueueSendEndpoint.Send<ReceiveGlosavDevices>(new { });

				//
				var consumed = await consumerHarness.Consumed.Any<ReceiveGlosavDevices>();

				//
				_requestClientMock
					.Verify(
						x => x.GetResponse<TrackerDataResponse>(IsAny<TrackerDataMessage>(), IsAny<CancellationToken>(),
							IsAny<RequestTimeout>()), Never);
			}
			finally
			{
				await harness.Stop();
			}
		}

		[Test]
		public async Task Should_handle_exception_without_rethrow_and_not_send_any_request()
		{
			//
			var harness = new InMemoryTestHarness();

			var consumerHarness = harness.Consumer(() => new ReceiveGlosavDevicesConsumer(_requestClientMock.Object,
				_glosavMonitoringClientMock.Object, TestInstances.Mapper, Of<ILogger<ReceiveGlosavDevicesConsumer>>()));

			var latestFilter = Of<ILatestFilter<ConsumeContext<MapItemsReceived>>>(x =>
				x.Latest == Task.FromResult(Of<ConsumeContext<MapItemsReceived>>(y =>
					y.Message == Of<MapItemsReceived>(z =>
						z.MapItemTrackIds == new[] { "1", "2", "3" }))));

			_glosavMonitoringClientMock
				.Setup(x => x.GetLastMonitoringMessages(IsAny<ApiMonitoringMessagesRequestPayload>()))
				.ThrowsAsync(new Exception());

			SharedLib.Filters.LatestFilter<MapItemsReceived>.SetLatest(latestFilter);

			try
			{
				await harness.Start();

				await harness.InputQueueSendEndpoint.Send<ReceiveGlosavDevices>(new { });

				//
				var consumed = await consumerHarness.Consumed.SelectAsync<ReceiveGlosavDevices>().First();

				//
				_requestClientMock
					.Verify(
						x => x.GetResponse<TrackerDataResponse>(IsAny<TrackerDataMessage>(), IsAny<CancellationToken>(),
							IsAny<RequestTimeout>()), Never);
				consumed.Exception.Should().BeNull();
			}
			finally
			{
				await harness.Stop();
			}
		}
	}
}
