﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using UzTube.Core.Api.Models.Exceptions;
using UzTube.Core.Api.Models.VideoMetadatas;

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
        }

        private VideoMetadataValidationException CreateAndLogValidationException(NullVideoMetadataException nullVideoMetadataException)
        {
            var videoMetadataValidationException =
                new VideoMetadataValidationException("VideoMetadata validation error occured, fix errors and try again.", nullVideoMetadataException);

            return videoMetadataValidationException;
        }
    }
}
