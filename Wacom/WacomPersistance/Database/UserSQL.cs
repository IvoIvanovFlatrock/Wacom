using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;
using WacomCore.Models;

namespace WacomPersistance.Database
{
	public static class UserSQL
	{
		public static UserModel Add(Guid id, string userName, string email)
		{
			using (var conn = new SqliteConnection(@"Data Source=test.db"))
			{
				conn.Open();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;

					cmd.CommandText = $"INSERT INTO Users (Id, UserName, Email)" +
						$" VALUES (@Id, @UserName,@Email)";
					cmd.Parameters.Add("@Id", SqliteType.Blob).Value = id;
					cmd.Parameters.Add("@UserName", SqliteType.Text).Value = userName;
					cmd.Parameters.Add("@Email", SqliteType.Text).Value = email;

					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}

			return new UserModel() { Id = id, UserName = userName, Email = email };
		}

		public static UserModel Get(Guid id)
		{
			UserModel res;
			using (var conn = new SqliteConnection(@"Data Source=test.db"))
			{
				conn.Open();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = $"SELECT * FROM Users";
					cmd.CommandType = CommandType.Text;

					var r = cmd.ExecuteReader();
					while (r.Read())
					{
						var a = Encoding.UTF8.GetString(r.GetValue("Id") as byte[]);
						var b = r["UserName"].ToString();
						var c = r["Email"].ToString();
						//var a = r as UserModel;
					}

					conn.Close();
				}
			}

			return new UserModel() {};
		}
	}
}
