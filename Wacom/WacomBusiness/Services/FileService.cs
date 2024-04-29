using Microsoft.AspNetCore.Http;
using WacomCore.Contracts;
using WacomCore.Entities;

namespace WacomBusiness.Services
{
	public class FileService : IFileService
	{
		private IFileRepository fileRepository;

		public FileService(IFileRepository fileRepository)
		{
			this.fileRepository = fileRepository;
		}

		public async Task PushAsync(IFormFile file, Guid userId)
		{
            if (string.IsNullOrEmpty(file.FileName))
            {
				throw new InvalidDataException("File name required.");
            }

			var type = Path.GetExtension(file.FileName);
            var path = await this.saveFilesToDiskSpaceAsync(file);
			await this.fileRepository
				.CreateAsync(file.FileName, file.ContentType, DateTime.Now.ToString(), userId, path);
		}

		public async Task<FileEntity> GetByIdAsync(Guid userId)
		{
			var fileInfo = await this.fileRepository.GetByUserIdAsync(userId);
			return fileInfo;
		}

		public async Task<FileEntity> GetByIdAndNameAsync(string name, Guid userId)
		{
			var fileInfo = await this.fileRepository.GetByUserIdAndNameAsync(name ,userId);
			return fileInfo;
		}

		#region Private Methods

		private async Task<string> saveFilesToDiskSpaceAsync(IFormFile file)
		{
			string path;
			string app = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			string dirname = Directory.GetParent(app).Parent.FullName;

			path = Path.Combine(dirname, "uploads");
			Directory.CreateDirectory($"{path}");
			if (file.Length > 0)
			{
				string filePath = Path.Combine(path, file.FileName);
				using (Stream fileStream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
				}
			}

			return path;
		}

		#endregion
	}
}
