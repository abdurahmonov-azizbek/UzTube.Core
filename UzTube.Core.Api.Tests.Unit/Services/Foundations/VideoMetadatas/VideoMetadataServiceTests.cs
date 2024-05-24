// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using UzTube.Core.Api.Brokers.DateTimes;
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
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IVideoMetadataService videoMetadataService;

        public VideoMetadataServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.videoMetadataService = new VideoMetadataService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        public static VideoMetadata CreateRandomVideoMetadata() =>
            CreateVideoMetadataFiller(GetRandomDateTime()).Create();

        public static VideoMetadata CreateRandomVideoMetadata(DateTimeOffset date) =>
           CreateVideoMetadataFiller(date).Create();

        private static Filler<VideoMetadata> CreateVideoMetadataFiller(DateTimeOffset dates)
        {
            var filler = new Filler<VideoMetadata>();

            filler.Setup()
                    .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 9, max: 99).GetValue();
    }
}
