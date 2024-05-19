// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using UzTube.Core.Api.Models.Exceptions;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Services.VideoMetadatas
{
    public partial class VideoMetadataService
    {
        private void ValidateVideoMetadata(VideoMetadata videoMetadata)
        {
            if (videoMetadata is null)
            {
                throw new NullVideoMetadataException();
            }
        }
    }
}
