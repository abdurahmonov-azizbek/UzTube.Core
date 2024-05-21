// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace UzTube.Core.Api.Models.Exceptions
{
    public class InvalidVideoMetadataException : Xeption
    {
        public InvalidVideoMetadataException(string message)
            : base(message)
        { }
    }
}
