using WacomCore.Models;

namespace WacomCore.Contracts
{
	public interface IUserService
	{ 
		Task<UserModel> GetByEmailAsync(string email);

		Task<UserModel> GetByIdAsync(Guid id);
	}
}
