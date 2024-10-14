using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.GenerationService.Infrastructure.Common;
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

        public ManageController(IUrlMappingService urlMappingService)
        {
            _urlMappingService = urlMappingService;
        }

        [HttpGet("urls")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                var error = new ErrorContract(StatusCodes.Status500InternalServerError, ex.Message, ErrorTitles.GetAllUrlsFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
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
                var error = new ErrorContract(StatusCodes.Status404NotFound, ex.Message, ErrorTitles.DeleteShortenUrlFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            catch (Exception ex)
            {
                var error = new ErrorContract(StatusCodes.Status500InternalServerError, ex.Message, ErrorTitles.DeleteShortenUrlFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
