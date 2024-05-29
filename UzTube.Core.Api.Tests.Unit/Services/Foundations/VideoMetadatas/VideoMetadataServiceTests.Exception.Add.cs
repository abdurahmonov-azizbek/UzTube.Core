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
using Microsoft.EntityFrameworkCore;

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            //when
            ValueTask<VideoMetadata> addVideoMetadata =
                this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                await Assert.ThrowsAsync<VideoMetadataDependencyException>(addVideoMetadata.AsTask);

            //then
            actualVideoMetadataDependencyException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    actualVideoMetadataDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDublicateKeyErrorOccursAndLogItAsync()
        {
            //given
            string someMessage = GetRandomString();
            VideoMetadata randomVideoMetadata = new VideoMetadata();
            var duplicateKeyException = new DuplicateKeyException(someMessage);
            var alreadyExistsVideoMetadataException = new AlreadyExitsVideoMetadataException(
                message: "Video metadata already exists.",
                innerException: duplicateKeyException);

            var expectedVideoMetadataDependencyValidationException =
                new VideoMetadataDependencyValidationException(
                    message: "Video metadata Dependency validation error occured , fix the errors and try again",
                    innerException: alreadyExistsVideoMetadataException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Throws(duplicateKeyException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask = this.videoMetadataService
                .AddVideoMetadataAsync(randomVideoMetadata);

            VideoMetadataDependencyValidationException actualVideoMetadataDependencyValidationException =
                await Assert.ThrowsAsync<VideoMetadataDependencyValidationException>(addVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataDependencyValidationException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedVideoMetadataDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedVideoMetadataException = new LockedVideoMetadataException(
                message: "Video metadata is locked, please try again.",
                innerException: dbUpdateConcurrencyException);

            var expectedVideoMetadataDependencyValidationException =
                new VideoMetadataDependencyValidationException(
                    message: "Video metadata dependency validation error occurred, fix the errors and try again.",
                    innerException: lockedVideoMetadataException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateConcurrencyException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask = this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

            VideoMetadataDependencyValidationException actualVideoMetadataDependencyValidationException =
                await Assert.ThrowsAsync<VideoMetadataDependencyValidationException>(addVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataDependencyValidationException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            //given
            VideoMetadata randomVideoMetadata = new VideoMetadata();
            var serviceException = new Exception();
            var failedVideoMetadataServiceException = new FailedVideoMetadataServiceException(
                message: "Failed video metadata service error occured, please contact support.",
                innerException: serviceException);

            var expectedVideoMetadataServiceException = new VideoMetadataServiceException(
                message: "Video metadata service error occured, contact support.",
                innerException: failedVideoMetadataServiceException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                 .Throws(serviceException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask = this.videoMetadataService.AddVideoMetadataAsync(randomVideoMetadata);

            VideoMetadataServiceException actualVideoMetadataServiceException =
                await Assert.ThrowsAsync<VideoMetadataServiceException>(addVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataServiceException.Should().BeEquivalentTo(expectedVideoMetadataServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedVideoMetadataServiceException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            //given
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
            var dbUpdateException = new DbUpdateException();

            var failedVideoMetadataStorageException = new FailedVideoMetadataStorageException(
                message: "Failed video metadata error occured, contact support.",
                innerException: dbUpdateException);

            var expectedVideoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    message: "Video metadata dependency error occured, fix the errors and try again.",
                    innerException: failedVideoMetadataStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask = this.videoMetadataService.AddVideoMetadataAsync(
                videoMetadata: randomVideoMetadata);

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                await Assert.ThrowsAsync<VideoMetadataDependencyException>(addVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataDependencyException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedVideoMetadataDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateVideoMetadataAsync(It.IsAny<VideoMetadata>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
