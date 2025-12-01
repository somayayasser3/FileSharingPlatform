using FileSharingPlatform.DTOs.FileDTOs;
using FileSharingPlatform.DTOs.ShareLinkDTOs;
using FileSharingPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileSharingPlatform.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IShareLinkService _shareLinkService;

        public FileController(IFileService fileService, IShareLinkService shareLinkService)
        {
            _fileService = fileService;
            _shareLinkService = shareLinkService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<FileResponseDto>> UploadFile([FromForm] UploadFileDto uploadFileDto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _fileService.UploadFileAsync(userId, uploadFileDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }


        [HttpGet("{fileId}/download")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var (fileContent, fileName, mimeType) = await _fileService.DownloadFileAsync(userId, fileId);
                return File(fileContent, mimeType, fileName);
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




        [HttpPost("{fileId}/share/link")]
        public async Task<ActionResult<ShareLinkeResponseDto>> CreateFileShareLink(
           Guid fileId,
           [FromBody] CreateShareLinkDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _shareLinkService.CreateFileShareLinkAsync(userId, fileId, dto);
                return Ok(result);
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


        [HttpPost("folders/{folderId}/share/link")]
        public async Task<ActionResult<ShareLinkeResponseDto>> CreateFolderShareLink(
            Guid folderId,
            [FromBody] CreateShareLinkDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _shareLinkService.CreateFolderShareLinkAsync(userId, folderId, dto);
                return Ok(result);
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
