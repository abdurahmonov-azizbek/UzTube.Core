// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.VideoMetadatas.Exceptions
{
    public class FailedVideoMetadataStorageException : Xeption
    {
        public FailedVideoMetadataStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}