using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WacomCore.Contracts;
using WacomCore.Models;

namespace WacomAPI.Controllers
{
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthorisationService authService;

		public AuthController(IAuthorisationService authService)
		{
			this.authService = authService;
		}

		[HttpPost("[action]")]
		[AllowAnonymous]
		public async Task<TokenResponse> SignIn([FromBody] SignInRequest model)
			=> await authService.SignIn(model);
	}
}
