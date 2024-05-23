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
            var nullVideoMetadataException = new NullVideoMetadataException("Video metadata is null.");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    "Video metadata validation error occured, fix errors and try again.",
                        nullVideoMetadataException);

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            var invalidVideoMetadata = new VideoMetadata
            {
                Title = invalidText
            };

            var invalidVideoMetadataException =
                new InvalidVideoMetadataException(
                    message: "Video metadata is invalid.");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.Id),
                values: "Id is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.Title),
                values: "Text is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.BlobPath),
                values: "Text is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.CreatedDate),
                values: "Date is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.UpdatedDate),
                values: "Date is required");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata validation error occured, fix errors and try again.",
                    innerException: invalidVideoMetadataException);

            //when
            ValueTask<VideoMetadata> addVideoMetadata =
                this.videoMetadataService.AddVideoMetadataAsync(invalidVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(addVideoMetadata.AsTask);

            //then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
                   Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
