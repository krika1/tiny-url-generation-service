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
    [Route("api/v1/manage")]
    public class ManageController : ControllerBase
    {
        private readonly IUrlMappingService _urlMappingService;
        private readonly ILogger<ManageController> _logger;

        public ManageController(IUrlMappingService urlMappingService, ILogger<ManageController> logger)
        {
            _urlMappingService = urlMappingService;
            _logger = logger;
        }

        [HttpGet("urls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UrlMappingContract>>> GetAllUrlsAsync()
        {
            try
            {
                var currentUser = User.GetUserId();

                var urls = await _urlMappingService.GetAllUrlMappingsAsync(int.Parse(currentUser)).ConfigureAwait(false);

                return Ok(urls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.GetAllUrlsFailedErrorTitle);
            }
        }

        [HttpDelete("urls/{url}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteShortUrlAsync([FromRoute] string url)
        {
            try
            {
                var currentUser = User.GetUserId();

                await _urlMappingService.DeleteShortUrlAsync(url, int.Parse(currentUser)).ConfigureAwait(false);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To404NotFoundResult(ex.Message, ErrorTitles.DeleteShortenUrlFailedErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.DeleteShortenUrlFailedErrorTitle);
            }
        }

        [HttpPatch("urls/{url}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SetExpirationDateAsync([FromRoute] string url, [FromBody] UpdateDateExpirationRequest request)
        {
            try
            {
                var currentUser = User.GetUserId();

                await _urlMappingService.SetExpirationDateAsync(request, url, int.Parse(currentUser)).ConfigureAwait(false);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To400BadRequest(ex.Message, ErrorTitles.SetExpirationDateFailedErrorTitle);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To404NotFoundResult(ex.Message, ErrorTitles.SetExpirationDateFailedErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.SetExpirationDateFailedErrorTitle);
            }
        }
    }
}
