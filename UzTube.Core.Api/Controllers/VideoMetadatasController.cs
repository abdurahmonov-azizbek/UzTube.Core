// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using UzTube.Core.Api.Models.VideoMetadatas;
using UzTube.Core.Api.Models.VideoMetadatas.Exceptions;
using UzTube.Core.Api.Services.VideoMetadatas;

namespace UzTube.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoMetadatasController : RESTFulController
    {
        private readonly IVideoMetadataService videoMetadataService;

        public VideoMetadatasController(IVideoMetadataService videoMetadataService) =>
            this.videoMetadataService = videoMetadataService;

        [HttpPost]
        public async ValueTask<ActionResult<VideoMetadata>> PostVideoMetadataAsync(VideoMetadata videoMetadata)
        {
            try
            {
                return await this.videoMetadataService.AddVideoMetadataAsync(videoMetadata);
            }
            catch (VideoMetadataValidationException videoMetadataValidationException)
            {
                return BadRequest(videoMetadataValidationException.InnerException);
            }
            catch (VideoMetadataDependencyValidationException videoMetadataDependencyValidationException)
                when (videoMetadataDependencyValidationException.InnerException is AlreadyExitsVideoMetadataException)
            {
                return Conflict(videoMetadataDependencyValidationException.InnerException);
            }
            catch (VideoMetadataDependencyValidationException videoMetadataDependencyValidationException)
            {
                return BadRequest(videoMetadataDependencyValidationException.InnerException);
            }
            catch (VideoMetadataDependencyException videoMetadataDependencyExcetpion)
            {
                return InternalServerError(videoMetadataDependencyExcetpion);
            }
            catch (VideoMetadataServiceException videoMetadataServiceException)
            {
                return InternalServerError(videoMetadataServiceException.InnerException);
            }
        }

    }
}
