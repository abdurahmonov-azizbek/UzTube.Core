// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using FluentAssertions;
using Moq;
using UzTube.Core.Api.Models.Exceptions;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfIsNullAndLogError()
        {
            //given
            VideoMetadata nullVideoMetadata = null;
            var nullVideoMetadataException = new NullVideoMetadataException();

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(nullVideoMetadataException);

            //when
            ValueTask<VideoMetadata> addVideoMetadata =
                this.videoMetadataService.AddVideoMetadataAsync(nullVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(addVideoMetadata.AsTask);

            //then
            actualVideoMetadataValidationException.Should()
                .BeEquivalentTo(expectedVideoMetadataValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
