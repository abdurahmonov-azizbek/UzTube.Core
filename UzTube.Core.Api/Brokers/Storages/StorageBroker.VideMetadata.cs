using Microsoft.EntityFrameworkCore;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<VideoMetadata> VideoMetadatas { get; set; }

    public ValueTask<VideoMetadata> DeleteVideoMetadataAsync(VideoMetadata videoMetadata, CancellationToken cancellationToken = default)
        => DeleteAsync(videoMetadata);

    public ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata, CancellationToken cancellationToken = default)
        => InsertAsync(videoMetadata);

    public IQueryable<VideoMetadata> SelectAllVideoMetadatas()
        => SelectAll<VideoMetadata>();

    public ValueTask<VideoMetadata> SelectVideoMetadataByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => SelectAsync<VideoMetadata>(id);

    public ValueTask<VideoMetadata> UpdateVideoMetadataAsync(VideoMetadata videoMetadata, CancellationToken cancellationToken = default)
        => UpdateAsync(videoMetadata);

}
