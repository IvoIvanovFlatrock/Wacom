using WacomCore.Entities;
using WacomCore.Models;

namespace WacomCore.Contracts
{
	public interface IUserRepository
	{
		Task<User> GetByIdAsync(Guid id);
		Task<User> GetByEmailAsync(string email);
		Task CreateAsync(UserModel user);
	}
}
