using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.GenerationService.Infrastructure.Common;
using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Exceptions;
using TinyUrl.GenerationService.Infrastructure.Services;

namespace TinyUrl.GenerationService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1")]
    public class ShortenController : ControllerBase
    {
        private readonly IUrlMappingService _urlMappingService;
        private readonly ILogger<ShortenController> _logger;

        public ShortenController(IUrlMappingService urlMappingService, ILogger<ShortenController> logger)
        {
            _urlMappingService = urlMappingService;
            _logger = logger;
        }

        [HttpPost("shorten")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UrlMappingContract>> ShortenUrlAsync([FromBody] ShortenUrlRequest request)
        {
            try
            {
                var currentUser = User.GetUserId();

                var shortenedUrl = await _urlMappingService.ShortenUrlAsync(request, int.Parse(currentUser)).ConfigureAwait(false);

                return Ok(shortenedUrl);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To400BadRequest(ex.Message, ErrorTitles.PostShortenUrlFailedErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.PostShortenUrlFailedErrorTitle);
            }
        }
    }
}
