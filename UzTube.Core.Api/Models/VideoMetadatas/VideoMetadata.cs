namespace UzTube.Core.Api.Models.VideoMetadatas;

public class VideoMetadata
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string BlobPath { get; set; } = default!;

    public string Thubnail { get; set; } = default!;

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset UpdatedDate { get; set; }
}
