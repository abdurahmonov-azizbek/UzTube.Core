// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using UzTube.Core.Api.Brokers.Loggings;
using UzTube.Core.Api.Brokers.Storages;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Services.VideoMetadatas
{
    internal partial class VideoMetadataService : IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public VideoMetadataService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata) =>
        TryCatch(async () =>
        {
            ValidateVideoMetadataOnAdd(videoMetadata);

            return await storageBroker.InsertVideoMetadataAsync(videoMetadata);
        });
    }
}
