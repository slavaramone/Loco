using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using FluentAssertions;
using Management.Db.Consumers;
using Management.Db.Entities;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Moq.It;
using static Moq.Mock;


namespace Management.Db
{
    [Category("Consumers, Management.Db")]
    [TestFixture]
    public class LocoInfosConsumer_Specs
    {
        readonly Mock<IManagementDbRepo> _managementDbRepoMock;

        public LocoInfosConsumer_Specs()
        {
            _managementDbRepoMock = new Mock<IManagementDbRepo>();
        }

        [Test]
        public async Task Should_respond_with_locos_returned_by_repository()
        {
            //
            var harness = new InMemoryTestHarness();

            var mapperMock = new Mock<IMapper>();

            int total = TestInstances.Random.Next(1, 10);
            var locos = Enumerable.Range(0, total).Select(e => new Loco { IsActive = true }).ToList();
            var locoContracts = Enumerable.Range(0, total).Select(e => new LocoContract()).ToList();

            mapperMock.Setup(m => m.Map<List<LocoContract>>(IsAny<List<Loco>>()))
                .Returns(locoContracts);

            var consumerHarness = harness.Consumer(() => new LocoInfosConsumer(_managementDbRepoMock.Object, Of<ILogger<LocoInfosConsumer>>(), mapperMock.Object));

            try
            {
                await harness.Start();

                _managementDbRepoMock
                    .Setup(x => x.GetLocosWithCamerasAndSensors(IsAny<LocoInfosRequest>()))
                    .ReturnsAsync(locos);

                await harness.InputQueueSendEndpoint.Send<LocoInfosRequest>(new { });

                //
                await consumerHarness.Consumed.Any<LocoInfosRequest>();

                //
                var published = harness.Published.Select<LocoInfosResponse>().Single();
                published.Context.Message.Locos.Should().HaveSameCount(locos);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}