// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.Exceptions
{
    public class VideoMetadataValidationException : Xeption
    {
        public VideoMetadataValidationException(Xeption innerException)
            : base(message: "VideoMetadata validation error occured, fix errors and try again.", innerException)
        { }
    }
}
