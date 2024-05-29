// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using UzTube.Core.Api.Brokers.DateTimes;
using UzTube.Core.Api.Brokers.Loggings;
using UzTube.Core.Api.Brokers.Storages;
using UzTube.Core.Api.Services.VideoMetadatas;

namespace UzTube.Core.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) =>
            this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddOData(options =>
                options.Select().Filter().OrderBy().Count().Expand());

            services.AddDbContext<StorageBroker>();

            services.AddCors(option =>
            {
                option.AddPolicy("MyPolicy", config =>
                {
                    config.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo { Title = "UzTube.Core", Version = "v1" });
            });

            AddBrokers(services);
            AddFoundationServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(config =>
                config.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "CashOverflow v1"));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<IVideoMetadataService, VideoMetadataService>();
        }
    }
}
