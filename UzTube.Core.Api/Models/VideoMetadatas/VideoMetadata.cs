// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

namespace UzTube.Core.Api.Models.VideoMetadatas
{
    public class VideoMetadata
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = default!;
        public string BlobPath { get; set; } = default!;
        public string Thubnail { get; set; } = default!;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
