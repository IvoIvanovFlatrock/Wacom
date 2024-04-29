using Microsoft.Extensions.Options;
using SimpleInjector;
using WacomAPI.Middleware;
using WacomBusiness.Services;
using WacomCore.Contracts;
using WacomCore.Settings;
using WacomPersistance.Database;
using WacomPersistance.Repository;

namespace WacomAPI.Configurations
{
	public static class SimpleInjector
	{
		public static void ConfigureSimpleInjector(this IServiceCollection services, IConfiguration configuration, Container container)
		{
			services.AddSimpleInjector(container, options =>
			{
				options.AddAspNetCore()
					.AddControllerActivation();

				options.AddLogging();
			});

			container.Register<IAuthorisationService, AuthorisationService>(Lifestyle.Transient);
			container.Register<IFileService, FileService>(Lifestyle.Transient);
			container.Register<IUserService, UserService>(Lifestyle.Transient);
			container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);
			container.Register<IFileRepository, FileRepository>(Lifestyle.Scoped);
			container.Register<AuthMiddleware>();
			container.Register<DataContext>(Lifestyle.Singleton);

			var jwtSettingsSection = configuration.GetSection("JWTSettings");
			var jwtSettings = new JWTSettings();
			jwtSettingsSection.Bind(jwtSettings);
			var jwtSettingsOption = Options.Create(jwtSettings);
			container.Register<IOptions<JWTSettings>>(() => jwtSettingsOption, Lifestyle.Singleton);

			var fileSettingsSection = configuration.GetSection("UploadFilesSettings");
			var fileSettings = new FileSettings();
			fileSettingsSection.Bind(fileSettings);
			var fileSettingsOption = Options.Create(fileSettings);
			container.Register<IOptions<FileSettings>>(() => fileSettingsOption, Lifestyle.Singleton);

			services
				.BuildServiceProvider(validateScopes: true)
				.UseSimpleInjector(container);
			services.UseSimpleInjectorAspNetRequestScoping(container);

			container.Verify();
		}
	}
}
