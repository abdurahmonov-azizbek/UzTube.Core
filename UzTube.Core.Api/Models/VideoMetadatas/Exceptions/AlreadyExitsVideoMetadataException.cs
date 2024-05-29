// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.VideoMetadatas.Exceptions
{
    public class AlreadyExitsVideoMetadataException : Xeption
    {
        public AlreadyExitsVideoMetadataException(string message, Exception innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}
