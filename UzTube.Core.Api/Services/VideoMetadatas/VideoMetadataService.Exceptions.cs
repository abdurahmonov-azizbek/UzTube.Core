// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UzTube.Core.Api.Models.VideoMetadatas;
using UzTube.Core.Api.Models.VideoMetadatas.Exceptions;
using Xeptions;

namespace UzTube.Core.Api.Services.VideoMetadatas
{
    internal partial class VideoMetadataService
    {
        private delegate ValueTask<VideoMetadata> ReturningVideoMetadataFunction();
        private delegate IQueryable<VideoMetadata> ReturningVideoMetadatasFunction();

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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedVideoMetadataException = new LockedVideoMetadataException(
                    message: "Video metadata is locked, please try again.",
                    innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedVideoMetadataException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedVideoMetadataStorageException = new FailedVideoMetadataStorageException(
                    message: "Failed video metadata error occured, contact support.",
                    innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedVideoMetadataStorageException);
            }
            catch (Exception exception)
            {
                var failedVideoMetadataServiceException = new FailedVideoMetadataServiceException(
                    message: "Failed video metadata service error occured, please contact support.",
                    innerException: exception);

                throw CreateAndLogServiceException(failedVideoMetadataServiceException);
            }
        }

        private IQueryable<VideoMetadata> TryCatch(ReturningVideoMetadatasFunction returningVideoMetadatasFunction)
        {
            try
            {
                return returningVideoMetadatasFunction();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Exception CreateAndLogDependencyException(Xeption exception)
        {
            var videoMetadataDependencyException = new VideoMetadataDependencyException(
                message: "Video metadata dependency error occured, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogError(videoMetadataDependencyException);

            return videoMetadataDependencyException;
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
                message: "Video metadata dependency validation error occurred, fix the errors and try again.",
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
                    message: "Video metadata validation error occured, fix the errors and try again.",
                    innerException: innerException);

            this.loggingBroker.LogError(videoMetadataValidationException);

            return videoMetadataValidationException;
        }
    }
}
