// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using UzTube.Core.Api.Brokers.Loggings;
using UzTube.Core.Api.Brokers.Storages;
using UzTube.Core.Api.Models.VideoMetadatas;
using UzTube.Core.Api.Services.VideoMetadatas;
using Xeptions;

namespace UzTube.Core.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IVideoMetadataService videoMetadataService;

        public VideoMetadataServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.videoMetadataService = new VideoMetadataService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        public static VideoMetadata CreateRandomVideoMetadata() =>
            CreateVideoMetadataFiller().Create();

        private static Filler<VideoMetadata> CreateVideoMetadataFiller()
        {
            var filler = new Filler<VideoMetadata>();

            filler.Setup()
                    .OnType<DateTimeOffset>().Use(GetRandomDateTime);

            return filler;
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    }
}
