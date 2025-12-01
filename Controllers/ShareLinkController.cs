using FileSharingPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileSharingPlatform.Controllers
{
    [Route("api/share/link")]
    [ApiController]
    public class ShareLinkController : ControllerBase
    {
        private readonly IShareLinkService _shareLinkService;

        public ShareLinkController(IShareLinkService shareLinkService)
        {
            _shareLinkService = shareLinkService;
        }

        [HttpDelete("{linkId}")]
        public async Task<IActionResult> DisableShareLink(Guid linkId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _shareLinkService.DisableShareLinkAsync(userId, linkId);
                return Ok(new { message = "Share link disabled successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }
    }
}
