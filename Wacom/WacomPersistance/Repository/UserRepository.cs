using Dapper;
using WacomCore.Contracts;
using WacomCore.Entities;
using WacomCore.Models;
using WacomPersistance.Database;

namespace WacomPersistance.Repository
{
	public class UserRepository : IUserRepository
	{
		private DataContext context;

		public UserRepository(DataContext context)
		{
			this.context = context;
		}

		public async Task<User> GetByEmailAsync(string email)
		{
			using var connection = context.CreateConnection();
			var sql = @"
            SELECT * FROM Users 
            WHERE Email = @email;";
			return await connection.QueryFirstAsync(sql, new { email });
		}

		public async Task<User> GetByIdAsync(Guid id)
		{
			using var connection = context.CreateConnection();
			var sql = @"
				SELECT * FROM Users 
				WHERE Id = @id;";

			return await connection.QueryFirstAsync<User>(sql, new { id });
		}

		public async Task CreateAsync(UserModel user)
		{
			using var connection = context.CreateConnection();
			var sql = @"INSERT INTO Users (Id, UserName, Email)
            VALUES (@Id, @UserName, @Email);";

			await connection.ExecuteAsync(sql, new
			{
				Id = user.Id,
				UserName = user.UserName,
				Email = user.Email
			});
		}
	}
}
