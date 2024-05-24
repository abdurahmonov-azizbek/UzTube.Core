// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using UzTube.Core.Api.Models.VideoMetadatas;
using EFxceptions.Models.Exceptions;
using UzTube.Core.Api.Models.VideoMetadatas.Exceptions;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        [Fact]
        public async Task ShouldthrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
            SqlException sqlException = GetSqlException();

            FailedVideoMetadataStorageException failedVideoMetadataStorageException =
                new FailedVideoMetadataStorageException(
                    message: "Failed video metadata error occured, contact support.",
                    innerException: sqlException);

            VideoMetadataDependencyException expectedVideoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    message: "Video metadata dependency error occured, fix the errors and try again.",
                    innerException: failedVideoMetadataStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertVideoMetadataAsync(someVideoMetadata))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<VideoMetadata> addVideoMetadata =
                this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                await Assert.ThrowsAsync<VideoMetadataDependencyException>(addVideoMetadata.AsTask);

            //then
            actualVideoMetadataDependencyException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(someVideoMetadata),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    actualVideoMetadataDependencyException))),
                        Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDublicateKeyErrorOccursAndLogItAsync()
        {
            //given
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
            var randomMessage = GetRandomString();

            var duplicateKeyException = new DuplicateKeyException(randomMessage);
            var alreadyExistsVideoMetadataException = new AlreadyExitsVideoMetadataException(
                message: "Video metadata already exists.",
                innerException: duplicateKeyException);

            var expectedVideoMetadataDependencyValidationException =
                new VideoMetadataDependencyValidationException(
                    message: "Video metadata Dependency validation error occured , fix the errors and try again",
                    innerException: alreadyExistsVideoMetadataException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertVideoMetadataAsync(randomVideoMetadata)).ThrowsAsync(duplicateKeyException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask = this.videoMetadataService
                .AddVideoMetadataAsync(randomVideoMetadata);

            VideoMetadataDependencyValidationException actualVideoMetadataDependencyValidationException =
                await Assert.ThrowsAsync<VideoMetadataDependencyValidationException>(addVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataDependencyValidationException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyValidationException);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(expectedVideoMetadataDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
