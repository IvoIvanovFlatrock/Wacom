using Microsoft.Data.Sqlite;

namespace WacomPersistance.Database
{
	public static class SaveFileAndUserData
	{
		public static void Save(Guid userId, string fileName, string fileInfo, string date)
		{
			using (var conn = new SqliteConnection(@"Data Source=test.db"))
			{
				conn.Open();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					//cmd.CommandText = "SELECT * FROM Users WHERE Id = @UserId";
					//cmd.Parameters.Add("@UserId", SqliteType.Blob).Value = userId;
					//var reader = cmd.ExecuteReader();
					//if (true)
					//{
					//	cmd.CommandText = $"INSERT INTO Users (Id, UserName, Email)" +
					//		$" VALUES (@Id, @UserName,@Email)";
					//	cmd.Parameters.Add("@Id", SqliteType.Blob).Value = userId;
					//	cmd.Parameters.Add("@UserName", SqliteType.Text).Value = "Ivak";
					//	cmd.Parameters.Add("@Email", SqliteType.Text).Value = "123@123.com";
					//	reader = cmd.ExecuteReader();
					//}

					cmd.CommandText = $"INSERT INTO Files (UserId, FileName, FileInfo, Date)" +
						$" VALUES (@UserId, @FileName,@FileInfo,@Date)";
					cmd.Parameters.Add("@UserId", SqliteType.Blob).Value = userId;
					cmd.Parameters.Add("@FileName", SqliteType.Text).Value = fileName.Trim();
					cmd.Parameters.Add("@FileInfo", SqliteType.Text).Value = fileInfo.Trim();
					cmd.Parameters.Add("@Date", SqliteType.Text).Value = date.Trim();

					cmd.ExecuteNonQuery();
					conn.Close();
					conn.Close();
				}
			}

			//var conn = new SqliteConnection(@"Data Source=test.db");
			//conn.Open();

			//var cmd = conn.CreateCommand();

			//cmd.CommandText = "INSERT INTO Users";
			//var u = cmd.ExecuteReader();
			//conn.Close();
		}
	}
}
