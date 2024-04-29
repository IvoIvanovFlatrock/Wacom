using WacomCore.Models;

namespace WacomCore.Contracts
{
	public interface IAuthorisationService
	{
		Task<TokenResponse> SignIn(SignInRequest model);
	}
}
