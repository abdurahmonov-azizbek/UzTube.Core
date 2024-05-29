// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllVideoMetadatas()
        {
            //given
            IQueryable<VideoMetadata> randomVideoMetadatas = CreateRandomVideoMetadatas();
            IQueryable<VideoMetadata> storageVideoMetadatas = randomVideoMetadatas;
            IQueryable<VideoMetadata> expectedVideoMetadatas = storageVideoMetadatas.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllVideoMetadatas()).Returns(storageVideoMetadatas);

            //when
            IQueryable<VideoMetadata> actualVideoMetadatas =
                this.videoMetadataService.RetrieveAllVideoMetadatas();

            //then
            actualVideoMetadatas.Should().BeEquivalentTo(expectedVideoMetadatas);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllVideoMetadatas(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
