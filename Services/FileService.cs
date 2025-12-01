using FileSharingPlatform.Data.UnitOfWork;
using FileSharingPlatform.DTOs.FileDTOs;
using FileSharingPlatform.Helpers;
using FileSharingPlatform.Models;

namespace FileSharingPlatform.Services
{
    public class FileService: IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public FileService(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<FileResponseDto> UploadFileAsync(Guid userId, UploadFileDto uploadFileDto)
        {
            // Validate file extension
            if (!FileHelper.IsValidFileExtension(uploadFileDto.File.FileName))
            {
                throw new InvalidOperationException("Invalid file extension. Allowed: PNG, JPG, JPEG, PDF, XLSX, ZIP, RAR");
            }

            // Validate file size
            if (!FileHelper.IsValidFileSize(uploadFileDto.File.Length))
            {
                throw new InvalidOperationException("File size exceeds 10MB limit");
            }

            // Validate folder exists and user owns it
            var folder = await _unitOfWork.Folders.GetByIdAsync(uploadFileDto.FolderId);

            if (folder == null)
            {
                throw new InvalidOperationException("Folder not found");
            }

            if (folder.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't own this folder");
            }

            // Check for duplicate file name in folder
            var fileName = Path.GetFileNameWithoutExtension(uploadFileDto.File.FileName);
            if (await _unitOfWork.Files.FileNameExistsAsync(fileName, uploadFileDto.FolderId))
            {
                throw new InvalidOperationException("File with this name already exists in this folder");
            }


            // Create upload directory if not exists
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Save file to disk
            var filePath = Path.Combine(uploadsPath, uploadFileDto.File.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFileDto.File.CopyToAsync(stream);
            }

            // Create file record
            var file = new FFile
            {
                FileId = Guid.NewGuid(),
                Name = fileName,
                StoragePath = filePath,
                Size = uploadFileDto.File.Length,
                MimeType = uploadFileDto.File.ContentType,
                Description = uploadFileDto.Description,
                FolderId = uploadFileDto.FolderId,
                UserId = userId,
                UploadedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Files.AddAsync(file);

            // Update user storage
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.StorageUsed += uploadFileDto.File.Length;
                _unitOfWork.Users.Update(user);
            }

            await _unitOfWork.SaveChangesAsync();

            return new FileResponseDto
            {
                FileId = file.FileId,
                Name = file.Name,
                Size = file.Size,
                MimeType = file.MimeType,
                Description = file.Description,
                FolderPath = folder.Path,
                UploadedAt = file.UploadedAt
            };
        }

        public async Task<(byte[] fileContent, string fileName, string mimeType)> DownloadFileAsync(Guid userId, Guid fileId)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(fileId);

            if (file == null)
            {
                throw new InvalidOperationException("File not found");
            }

            if (file.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't own this file");
            }

            if (!System.IO.File.Exists(file.StoragePath))
            {
                throw new InvalidOperationException("File not found on disk");
            }

            var fileContent = await System.IO.File.ReadAllBytesAsync(file.StoragePath);

            return (fileContent, file.Name, file.MimeType);
        }
    }
}
