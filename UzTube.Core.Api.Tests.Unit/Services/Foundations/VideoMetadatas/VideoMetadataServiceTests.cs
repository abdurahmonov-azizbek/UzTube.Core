// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Moq;
using Tynamix.ObjectFiller;
using UzTube.Core.Api.Brokers.Storages;
using UzTube.Core.Api.Models.VideoMetadatas;
using UzTube.Core.Api.Services.VideoMetadatas;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IVideoMetadataService videoMetadataService;

        public VideoMetadataServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.videoMetadataService = new VideoMetadataService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        public static VideoMetadata CreateRandomVideoMetadata() =>
            CreateVideoMetadataFiller().Create();

        private static Filler<VideoMetadata> CreateVideoMetadataFiller()
        {
            var filler = new Filler<VideoMetadata>();

            filler.Setup()
                    .OnType<DateTimeOffset>().Use(GetRandomDateTime);

            return filler;
        }
    }
}
