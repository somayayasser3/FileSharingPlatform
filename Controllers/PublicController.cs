using FileSharingPlatform.DTOs.ShareLinkDTOs;
using FileSharingPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingPlatform.Controllers
{
    [Route("api/public")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IShareLinkService _shareLinkService;

        public PublicController(IShareLinkService shareLinkService)
        {
            _shareLinkService = shareLinkService;
        }

        [HttpPost("share/{token}")]
        public async Task<ActionResult<ShareLinkDetailsDto>> GetShareDetails(
            string token,
            [FromBody] AccessShareLinkDto dto)
        {
            try
            {
                var result = await _shareLinkService.GetPublicShareDetailsAsync(token, dto.Password);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("share/{token}/download")]
        public async Task<IActionResult> DownloadPublicFile(
            string token,
            [FromBody] AccessShareLinkDto dto)
        {
            try
            {
                var fileContent = await _shareLinkService.DownloadPublicFileAsync(token, dto.Password);
                return File(fileContent, "application/octet-stream");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
