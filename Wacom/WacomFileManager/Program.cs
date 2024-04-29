using Microsoft.OpenApi.Models;
using SimpleInjector;
using WacomAPI.Configurations;
using WacomAPI.Middleware;
using WacomPersistance;
using WacomPersistance.Database;

namespace WacomFileManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var container = new Container();
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();

			//Setup Swagger
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSwaggerGen(swagger =>
			{
				swagger.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "JWT Token Authentication API",
					Description = ".NET 8 Web API"
				});
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme." +
						" \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below" +
						".\r\n\r\nExample: \"Bearer 12345abcdef\"",
				});
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] {}

					}
				});
			});

			IServiceCollection services = builder.Services;
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			//Register services
			builder.Services.ConfigureSimpleInjector(builder.Configuration, container);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			//Register Middleware
			app.UseMiddleware<AuthMiddleware>(container);

			//Set up sqlite
			var context = container.GetInstance<DataContext>();
			context.Init();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
