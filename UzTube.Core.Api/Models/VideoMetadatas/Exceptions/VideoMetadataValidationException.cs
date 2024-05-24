// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.VideoMetadatas.Exceptions
{
    public class VideoMetadataValidationException : Xeption
    {
        public VideoMetadataValidationException(string message, Xeption innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}
