using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using WacomCore.Contracts;
using WacomCore.Helpers;
using WacomCore.Settings;

namespace WacomAPI.Middleware
{
	public class AuthMiddleware : IMiddleware
	{
		private JWTSettings jwtSettings;
		private IUserService userService;

		public AuthMiddleware(IOptions<JWTSettings> jwtSettings,
			IUserService userService)
		{
			this.jwtSettings = jwtSettings.Value;
			this.userService = userService;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var tokenHeader = context.Request.Headers["Authorization"].FirstOrDefault();

			if (!string.IsNullOrWhiteSpace(tokenHeader))
			{
				var tokenParts = tokenHeader.Split(' ');
				if (tokenParts == null || tokenParts.Length <= 0 || tokenParts[0] != jwtSettings.Type)
				{
					//Schema does not match
					throw new ValidationException("Error Authentication.");
				}

				var token = tokenHeader.Replace(jwtSettings.Type, string.Empty).Trim();
				var (id, isFalse) = JwtHelper.Validate(token, jwtSettings.Secret);
				if (!string.IsNullOrWhiteSpace(token) && isFalse)
				{
					context.Items["User"] = await userService.GetByIdAsync(id);
					context.Items["AccessToken"] = token;
				}
			}

			await next(context);
		}
	}
}
