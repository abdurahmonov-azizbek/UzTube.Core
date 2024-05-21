// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

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
        }

        private VideoMetadataValidationException CreateAndLogValidationException(Xeption innerException)
        {
            var videoMetadataValidationException =
                new VideoMetadataValidationException("Video metadata validation error occured, fix errors and try again.", innerException);

            return videoMetadataValidationException;
        }
    }
}
