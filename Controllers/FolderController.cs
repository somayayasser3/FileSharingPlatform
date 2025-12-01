using FileSharingPlatform.DTOs.FolderDTOs;
using FileSharingPlatform.DTOs.FoldersAndFilesListingDTOs;
using FileSharingPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileSharingPlatform.Controllers
{
    [Route("api/folder")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<ActionResult<FolderResponseDto>> CreateFolder([FromBody] CreateFolderDto createFolderDto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _folderService.CreateFolderAsync(userId, createFolderDto);
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

        [HttpGet("{folderId}/items")]
        public async Task<ActionResult<FolderItemsResponseDto>> GetFolderItems(
                Guid folderId,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 50,
                [FromQuery] string sortBy = "name",
                [FromQuery] string order = "asc")
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _folderService.GetFolderItemsAsync(userId, folderId, page, pageSize, sortBy, order);
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
