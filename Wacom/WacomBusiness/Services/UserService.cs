using WacomCore.Contracts;
using WacomCore.Models;

namespace WacomBusiness.Services
{
	public class UserService : IUserService
	{
		private IUserRepository userRepository;

		public UserService(IUserRepository userRepository) 
		{
			this.userRepository = userRepository;
		}

		public async Task<UserModel> GetByIdAsync(Guid id)
		{
			var entity = await userRepository.GetByIdAsync(id);
			var model = new UserModel()
			{
				Id = Guid.Parse(entity.Id),
				Email = entity.Email,
				UserName = entity.UserName
			};
			return model;
		}

		public async Task<UserModel> GetByEmailAsync(string email)
		{
			var id = Guid.NewGuid();
			//Since we don't have register lets pretend we take him from somewhere
			var user = new UserModel() { Id = id, Email = $"{id}.email.com", UserName = "Default" };
			var entity = userRepository.CreateAsync(user);
			return user;
		}
	}
}
