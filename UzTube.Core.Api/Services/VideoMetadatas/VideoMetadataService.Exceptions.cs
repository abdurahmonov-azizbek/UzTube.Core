// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using UzTube.Core.Api.Models.VideoMetadatas;
using UzTube.Core.Api.Models.VideoMetadatas.Exceptions;
using Xeptions;

namespace UzTube.Core.Api.Services.VideoMetadatas
{
    internal partial class VideoMetadataService
    {
        private delegate ValueTask<VideoMetadata> ReturningVideoMetadataFunction();

        private async ValueTask<VideoMetadata> TryCatch(ReturningVideoMetadataFunction returningVideoMetadataFunction)
        {
            try
            {
                return await returningVideoMetadataFunction();
            }
            catch (NullVideoMetadataException nullVideoMetadataException)
            {
                throw CreateAndLogValidationException(nullVideoMetadataException);
            }
            catch (InvalidVideoMetadataException invalidVideoMetadataException)
            {
                throw CreateAndLogValidationException(invalidVideoMetadataException);
            }
            catch (SqlException sqlException)
            {
                FailedVideoMetadataStorageException failedVideoMetadataStorageException =
                    new FailedVideoMetadataStorageException(
                        message: "Failed video metadata error occured, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedVideoMetadataStorageException);
            }
            catch (DuplicateKeyException dublicateKeyException)
            {
                var alreadyExistsVideoMetadataException = new AlreadyExitsVideoMetadataException(
                    message: "Video metadata already exists.",
                    innerException: dublicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsVideoMetadataException);
            }
            catch (Exception exception)
            {
                var failedVideoMetadataServiceException = new FailedVideoMetadataServiceException(
                    message: "Failed video metadata service error occured, please contact support.",
                    innerException: exception);

                throw CreateAndLogServiceException(failedVideoMetadataServiceException);
            }
        }

        private Exception CreateAndLogServiceException(Xeption exception)
        {
            var videoMetadataServiceException = new VideoMetadataServiceException(
                message: "Video metadata service error occured, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(videoMetadataServiceException);

            return videoMetadataServiceException;
        }

        private Exception CreateAndLogDependencyValidationException(Xeption exception)
        {
            var videoMetadataDependencyValidationException = new VideoMetadataDependencyValidationException(
                message: "Video metadata Dependency validation error occured , fix the errors and try again",
                innerException: exception);

            this.loggingBroker.LogError(videoMetadataDependencyValidationException);

            return videoMetadataDependencyValidationException;
        }

        private Exception CreateAndLogCriticalDependencyException(Xeption exception)
        {
            VideoMetadataDependencyException videoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    message: "Video metadata dependency error occured, fix the errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogCritical(videoMetadataDependencyException);

            return videoMetadataDependencyException;
        }

        private VideoMetadataValidationException CreateAndLogValidationException(Xeption innerException)
        {
            var videoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata validation error occured, fix errors and try again.",
                    innerException: innerException);

            this.loggingBroker.LogError(videoMetadataValidationException);

            return videoMetadataValidationException;
        }
    }
}
