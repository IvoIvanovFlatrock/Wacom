using WacomCore.Entities;

namespace WacomCore.Contracts
{
	public interface IFileRepository
	{
		Task CreateAsync(string fileName, string type, string date, Guid userId, string path);

		Task<FileEntity> GetByUserIdAsync(Guid userId);

		Task<FileEntity> GetByUserIdAndNameAsync(string name, Guid userId);
	}
}
