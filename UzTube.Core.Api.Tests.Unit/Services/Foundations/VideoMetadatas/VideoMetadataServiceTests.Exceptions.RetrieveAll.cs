// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using UzTube.Core.Api.Models.VideoMetadatas.Exceptions;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedVideoMetadataStorageException(
                    message: "Failed video metadata error occured, contact support.",
                    innerException: sqlException);

            var expectedVideoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    message: "Video metadata dependency error occured, fix the errors and try again.",
                    innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllVideoMetadatas()).Throws(sqlException);

            //when
            Action retrieveAllVideoMetadatasAction = () =>
                this.videoMetadataService.RetrieveAllVideoMetadatas();

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                Assert.Throws<VideoMetadataDependencyException>(retrieveAllVideoMetadatasAction);

            //then
            actualVideoMetadataDependencyException.Should()
                .BeEquivalentTo(expectedVideoMetadataDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllVideoMetadatas(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedVideoMetadataDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedVideoMetadataServiceException =
                new FailedVideoMetadataServiceException(
                    message: "Failed video metadata service error occured, please contact support.",
                    innerException: serviceException);

            var expectedVideoMetadataServiceException =
                new VideoMetadataServiceException(
                    message: "Video metadata service error occured, contact support.",
                    innerException: failedVideoMetadataServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllVideoMetadatas()).Throws(serviceException);

            // when
            Action retrieveAllVideoMetadatasAction = () =>
                this.videoMetadataService.RetrieveAllVideoMetadatas();

            VideoMetadataServiceException actualVideoMetadataServiceException =
                Assert.Throws<VideoMetadataServiceException>(retrieveAllVideoMetadatasAction);

            // then
            actualVideoMetadataServiceException.Should().BeEquivalentTo(expectedVideoMetadataServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllVideoMetadatas(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedVideoMetadataServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
