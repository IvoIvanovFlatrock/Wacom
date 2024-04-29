using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace WacomPersistance.Database
{
	public class DataContext
	{
		protected readonly IConfiguration Configuration;

		public DataContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IDbConnection CreateConnection()
		{
			return new SqliteConnection("Data Source=LocalDatabase.db");
		}

		public async Task Init()
		{
			using var connection = CreateConnection();
			await initTables();

			async Task initTables()
			{
				var sql = @"CREATE TABLE IF NOT EXISTS Users(
					Id GUID UNIQUE NOT NULL PRIMARY KEY,
					UserName VARCHAR(255) NOT NULL,
					Email VARCHAR(255) UNIQUE NOT NULL);";
				await connection.ExecuteAsync(sql);

				sql = @"CREATE TABLE IF NOT EXISTS Files(
					Id GUID UNIQUE NOT NULL PRIMARY KEY,
					UserId GUID,
					FileName VARCHAR(255) NOT NULL,
					FileInfo VARCHAR(255) NOT NULL,
					Date TEXT(255) NOT NULL,
					Path TEXT(255) NOT NULL,
					FOREIGN KEY(UserId) REFERENCES Users(Id));";
				await connection.ExecuteAsync(sql);
			}
		}
	}
}
