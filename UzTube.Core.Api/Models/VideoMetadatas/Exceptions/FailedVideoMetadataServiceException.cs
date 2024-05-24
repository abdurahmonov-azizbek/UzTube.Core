// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.VideoMetadatas.Exceptions
{
    public class FailedVideoMetadataServiceException : Xeption
    {
        public FailedVideoMetadataServiceException(string message, Exception innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}
