using FileSharingPlatform.Data.UnitOfWork;
using FileSharingPlatform.DTOs.FolderDTOs;
using FileSharingPlatform.DTOs.FoldersAndFilesListingDTOs;
using FileSharingPlatform.Models;

namespace FileSharingPlatform.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FolderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FolderResponseDto> CreateFolderAsync(Guid userId, CreateFolderDto createFolderDto)
        {
            // Validate folder name doesn't exist in same parent
            if (await _unitOfWork.Folders.FolderNameExistsAsync(
                createFolderDto.Name,
                createFolderDto.ParentFolderId,
                userId))
            {
                throw new InvalidOperationException("Folder with this name already exists in this location");
            }

            string path = "/";
            int depth = 0;

            // If parent folder specified, validate it
            if (createFolderDto.ParentFolderId.HasValue)
            {
                var parentFolder = await _unitOfWork.Folders.GetByIdAsync(createFolderDto.ParentFolderId.Value);

                if (parentFolder == null)
                {
                    throw new InvalidOperationException("Parent folder not found");
                }

                if (parentFolder.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You don't own the parent folder");
                }

                if (parentFolder.Depth >= 10)
                {
                    throw new InvalidOperationException("Maximum folder depth (10 levels) exceeded");
                }

                path = $"{parentFolder.Path}/{createFolderDto.Name}";
                depth = parentFolder.Depth + 1;
            }
            else
            {
                path = $"/{createFolderDto.Name}";
            }

            // Create folder
            var folder = new Folder
            {
                FolderId = Guid.NewGuid(),
                Name = createFolderDto.Name,
                ParentFolderId = createFolderDto.ParentFolderId,
                UserId = userId,
                Path = path,
                Depth = depth,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Folders.AddAsync(folder);
            await _unitOfWork.SaveChangesAsync();

            return new FolderResponseDto
            {
                FolderId = folder.FolderId,
                Name = folder.Name,
                Path = folder.Path,
                ParentFolderId = folder.ParentFolderId,
                Depth = folder.Depth,
                CreatedAt = folder.CreatedAt
            };
        }

        //////////////////////////////////////////////////////
        public async Task<FolderItemsResponseDto> GetFolderItemsAsync(
                Guid userId,
                Guid folderId,
                int page,
                int pageSize,
                string sortBy,
                string order)
        {
            // Validate folder exists and user owns it
            var folder = await _unitOfWork.Folders.GetWithFilesAsync(folderId);

            if (folder == null)
            {
                throw new InvalidOperationException("Folder not found");
            }

            if (folder.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't own this folder");
            }

            // Get subfolders and files
            var items = new List<FolderItemDto>();

            // Add folders
            foreach (var subfolder in folder.SubFolders)
            {
                items.Add(new FolderItemDto
                {
                    Id = subfolder.FolderId,
                    Name = subfolder.Name,
                    Type = "folder",
                    Size = null,
                    ModifiedDate = subfolder.UpdatedAt
                });
            }

            // Add files
            foreach (var file in folder.Files)
            {
                items.Add(new FolderItemDto
                {
                    Id = file.FileId,
                    Name = file.Name,
                    Type = "file",
                    Size = file.Size,
                    ModifiedDate = file.UpdatedAt
                });
            }

            // Sort items (folders first, then files)
            var sortedItems = items
                .OrderBy(i => i.Type == "file" ? 1 : 0) // Folders first
                .ThenBy(i => sortBy.ToLower() switch
                {
                    "date" => i.ModifiedDate,
                    "size" => i.ModifiedDate, // Placeholder for size sorting
                    _ => i.ModifiedDate
                })
                .ThenBy(i => i.Name);

            if (order.ToLower() == "desc")
            {
                sortedItems = items
                    .OrderBy(i => i.Type == "file" ? 1 : 0)
                    .ThenByDescending(i => sortBy.ToLower() switch
                    {
                        "date" => i.ModifiedDate,
                        "size" => i.ModifiedDate,
                        _ => i.ModifiedDate
                    })
                    .ThenByDescending(i => i.Name);
            }

            // Paginate
            var totalItems = items.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paginatedItems = sortedItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            return new FolderItemsResponseDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = paginatedItems
            };
        }

    }
}
