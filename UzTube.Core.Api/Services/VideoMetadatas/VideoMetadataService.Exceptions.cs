// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Data.SqlClient;
using UzTube.Core.Api.Models.Exceptions;
using UzTube.Core.Api.Models.VideoMetadatas;
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
