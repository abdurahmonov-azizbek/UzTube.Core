// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.Exceptions
{
    public class NullVideoMetadataException : Xeption
    {
        public NullVideoMetadataException()
            : base(message: "VideoMetadata is null.")
        { }
    }
}
