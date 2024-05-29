// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Services.VideoMetadatas
{
    public interface IVideoMetadataService
    {
        /// <exception cref="Models.VideoMetadatas.Exceptions.VideoMetadataValidationException"></exception>
        /// <exception cref="Models.VideoMetadatas.Exceptions.VideoMetadataDependencyValidationException"></exception>
        /// <exception cref="Models.VideoMetadatas.Exceptions.VideoMetadataDependencyException"></exception>
        /// <exception cref="Models.VideoMetadatas.Exceptions.VideoMetadataServiceException"></exception>
        ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata);
    }
}