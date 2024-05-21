// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using UzTube.Core.Api.Brokers.Storages;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Services.VideoMetadatas
{
    internal partial class VideoMetadataService : IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;

        public VideoMetadataService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata) =>
        TryCatch(async () =>
        {
            ValidateVideoMetadataOnAdd(videoMetadata);

            return await storageBroker.InsertVideoMetadataAsync(videoMetadata);
        });
    }
}
