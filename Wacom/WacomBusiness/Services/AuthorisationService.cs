using Microsoft.Extensions.Options;
using WacomCore.Contracts;
using WacomCore.Exceptions;
using WacomCore.Helpers;
using WacomCore.Models;
using WacomCore.Settings;

namespace WacomBusiness.Services
{
	public class AuthorisationService : IAuthorisationService
	{
		private JWTSettings jwtSettings;
		private IUserService userService;

		public AuthorisationService(IOptions<JWTSettings> jwtSettings,
			IUserService userService) 
		{
			this.jwtSettings = jwtSettings.Value;
			this.userService = userService;
		}

		public async Task<TokenResponse> SignIn(SignInRequest model)
		{
			var user = await this.userService.GetByEmailAsync(model.Email);

			//Validate
			if (user == null || !this.passwordValidator(model.Password, user.Password))
				throw new ValidationException("Authorization_UsernameOrPasswordDoesNotMatch");

			return await this.generateTokens(user.Id);
		}

		private async Task<TokenResponse> generateTokens(Guid userId)
		{
			var jwtToken = JwtHelper.Generate(userId,  jwtSettings.Secret, jwtSettings.ExpireTime);

			return new TokenResponse
			{
				AccessToken = jwtToken,
				Type = jwtSettings.Type
			};
		}

		private bool passwordValidator(string password, string passwordHash)
		{
			//Check password
			return true;
		}
	}
}
