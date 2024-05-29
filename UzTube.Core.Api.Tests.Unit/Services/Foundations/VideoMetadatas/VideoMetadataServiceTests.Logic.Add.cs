// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Force.DeepCloner;
using Moq;
using UzTube.Core.Api.Models.VideoMetadatas;
using FluentAssertions;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        [Fact]
        public async Task ShouldAddVideoMetadataAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTime();
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata(randomDate);
            VideoMetadata inputVideoMetadata = randomVideoMetadata;
            VideoMetadata persistedVideoMetadata = inputVideoMetadata;
            VideoMetadata expectedVideoMetadata = persistedVideoMetadata.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTimeOffset()).Returns(randomDate);  

            this.storageBrokerMock.Setup(broker =>
                broker.InsertVideoMetadataAsync(inputVideoMetadata))
                    .ReturnsAsync(persistedVideoMetadata);

            //when
            VideoMetadata actualVideoMetadata = await this.videoMetadataService
                .AddVideoMetadataAsync(inputVideoMetadata);

            //then  
            actualVideoMetadata.Should().BeEquivalentTo(
                expectedVideoMetadata);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(inputVideoMetadata),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
