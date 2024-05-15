using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata, CancellationToken cancellationToken = default);
    IQueryable<VideoMetadata> SelectAllVideoMetadatas();
    ValueTask<VideoMetadata> SelectVideoMetadataByIdAsync(Guid id, CancellationToken cancellationToken = default);
    ValueTask<VideoMetadata> UpdateVideoMetadataAsync(VideoMetadata videoMetadata, CancellationToken cancellationToken = default);
    ValueTask<VideoMetadata> DeleteVideoMetadataAsync(VideoMetadata videoMetadata, CancellationToken cancellationToken = default);
}
