using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WacomCore.Helpers
{
	public class JwtHelper
	{
		public static string Generate(Guid userId, string secret, int expirationInSeconds = 3600)
		{
			var key = Encoding.ASCII.GetBytes(secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Expires = DateTime.UtcNow.AddSeconds(expirationInSeconds),
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("id", userId.ToString())
				}),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		public static (Guid, bool) Validate(string? token, string secret)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				return (Guid.Empty ,false);
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secret);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var isUserIdParsed = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

				return (isUserIdParsed, true);
			}
			catch
			{
				return (Guid.Empty, false);
			}
		}
	}
}
