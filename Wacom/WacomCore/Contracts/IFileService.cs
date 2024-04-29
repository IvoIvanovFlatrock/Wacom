using Microsoft.AspNetCore.Http;
using WacomCore.Entities;

namespace WacomCore.Contracts
{
	public interface IFileService
	{
		Task PushAsync(IFormFile file, Guid userId);

		Task<FileEntity> GetByIdAsync(Guid userId);

		Task<FileEntity> GetByIdAndNameAsync(string name, Guid userId);
	}
}
