using Microsoft.AspNetCore.Mvc;
using TinyUrl.GenerationService.Infrastructure.Common;
using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Exceptions;
using TinyUrl.GenerationService.Infrastructure.Services;

namespace TinyUrl.GenerationService.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ShortenController : ControllerBase
    {
        private readonly IUrlMappingService _urlMappingService;

        public ShortenController(IUrlMappingService urlMappingService)
        {
            _urlMappingService = urlMappingService;
        }

        [HttpPost("shorten")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UrlMappingContract>> ShortenUrlAsync([FromBody] ShortenUrlRequest request)
        {
            try
            {
               var shortenedUrl = await _urlMappingService.ShortenUrlAsync(request).ConfigureAwait(false);

                return Ok(shortenedUrl);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorContract(StatusCodes.Status404NotFound, ex.Message, ErrorTitles.PostShortenUrlFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            catch (Exception ex)
            {
                var error = new ErrorContract(StatusCodes.Status500InternalServerError, ex.Message, ErrorTitles.PostShortenUrlFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
