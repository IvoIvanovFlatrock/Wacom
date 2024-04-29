using Dapper;
using WacomCore.Contracts;
using WacomCore.Entities;
using WacomPersistance.Database;

namespace WacomPersistance.Repository
{
	public class FileRepository : IFileRepository
	{
		private DataContext context;

		public FileRepository(DataContext context)
		{
			this.context = context;
		}

		public async Task CreateAsync(string fileName,
			string type, string date, Guid userId, string path)
		{
			using var connection = context.CreateConnection();
			var sql = @"INSERT INTO Files (Id, UserId, FileName, FileInfo, Date, Path)
				VALUES (@Id, @UserId, @FileName,@FileInfo,@Date,@Path);";

			var id = Guid.NewGuid();

			await connection.ExecuteAsync(sql, new
			{
				Id = id,
				UserId = userId,
				FileName = fileName,
				FileInfo = type,
				Date = date,
				Path = path
			});
		}

		public async Task<FileEntity> GetByUserIdAndNameAsync(string name, Guid userId)
		{
			using var connection = context.CreateConnection();
			var sql = @"
				SELECT * FROM Files 
				WHERE UserId = @id AND FileName = @name;";

			return await connection.QueryFirstAsync<FileEntity>(sql, new { id = userId, name });
		}

		public async Task<FileEntity> GetByUserIdAsync(Guid id)
		{
			using var connection = context.CreateConnection();
			var sql = @"
				SELECT * FROM Files 
				WHERE UserId = @id;";

			return await connection.QueryFirstAsync<FileEntity>(sql, new { id });
		}
	}
}
