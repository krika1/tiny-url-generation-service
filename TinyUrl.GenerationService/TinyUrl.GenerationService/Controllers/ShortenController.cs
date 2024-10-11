using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.GenerationService.Infrastructure.Common;
using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Services;

namespace TinyUrl.GenerationService.Controllers
{
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UrlMappingContract>> ShortenUrlAsync([FromBody] ShortenUrlRequest request)
        {
            try
            {
                var currentUser = User.GetUserId();

                var shortenedUrl = await _urlMappingService.ShortenUrlAsync(request, int.Parse(currentUser)).ConfigureAwait(false);

                return Ok(shortenedUrl);
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
