using System.Text;
using Cubipool.API.Abstractions;
using Cubipool.API.Configuration;
using Cubipool.API.Implementations;
using Cubipool.API.Middlewares;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Cubipool.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			
			services.AddControllers();

			// Database Configuration
			services.AddDbContext<EFDbContext>(options =>
			{
				options
					.UseNpgsql(Configuration.GetConnectionString("PostgresqlConnection"));
			});

			// Object configurations
			IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			// Repositories Dependency Injection
			services.AddTransient<IConstantsRepository, ConstantsRepository>();
			services.AddTransient<ICubicleRepository, CubicleRepository>();
			services.AddTransient<IPointsRecordRepository, PointsRecordRepository>();
			services.AddTransient<IPrizeRepository, PrizeRepository>();
			services.AddTransient<IPublicationRepository, PublicationRepository>();
			services.AddTransient<IRequestRepository, RequestRepository>();
			services.AddTransient<IReservationRepository, ReservationRepository>();
			services.AddTransient<IResourceRepository, ResourceRepository>();
			services.AddTransient<ISharedSpaceRepository, SharedSpaceRepository>();
			services.AddTransient<IUserPrizeRepository, UserPrizeRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IUserReservationRepository, UserReservationRepository>();
			services.AddTransient<ICampusRepository, CampusRepository>();
			services.AddTransient<IResourceTypeRepository, ResourceTypeRepository>();
			services.AddTransient<IReservationStateRepository, ReservationStateRepository>();
			services.AddTransient<IUserReservationRepository, UserReservationRepository>();
			

			// Services Dependency Injection
			services.AddTransient<IJwtService, JwtService>();

			services.AddTransient<IAuthService, AuthService>();
			services.AddTransient<IConstantsService, ConstantsService>();
			services.AddTransient<ICubicleService, CubicleService>();
			services.AddTransient<IPointsRecordService, PointsRecordService>();
			services.AddTransient<IPrizeService, PrizeService>();
			services.AddTransient<IPublicationService, PublicationService>();
			services.AddTransient<IRequestService, RequestService>();
			services.AddTransient<IReservationService, ReservationService>();
			services.AddTransient<IResourceService, ResourceService>();
			services.AddTransient<ISharedSpaceService, SharedSpaceService>();
			services.AddTransient<IUserPrizeService, UserPrizeService>();
			services.AddTransient<IUserReservationService, UserReservationService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<ICampusService, CampusService>();
			services.AddTransient<IResourceTypeService, ResourceTypeService>();
			services.AddTransient<IReservationStateService, ReservationStateService>();
			services.AddTransient<IUserReservationService, UserReservationService>();
			
			services.AddScoped<IReservationService, ReservationService>();
			// Exception filters
			services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));

			// JWT Configuration
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x =>
				{
					x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(x =>
				{
					x.RequireHttpsMetadata = false;
					x.SaveToken = true;
					x.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});

			// Swagger Generation
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v0", new OpenApiInfo()
				{
					Version = "v0",
					Title = "CubiPool API",
					Description = "CubiPool Application API"
				});

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme."
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
						new string[] { }
					}
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// Handle exceptions
			app.UseExceptionHandler(a => a.Run(async context =>
			{
				var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
				var exception = exceptionHandlerPathFeature.Error;

				var result = JsonConvert.SerializeObject(new
				{
					error = exception.Message
				});
				context.Response.ContentType = "application/json";
				await context.Response.WriteAsync(result);
			}));

			// CORS Policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);

			// Swagger
			app.UseSwagger();
			// SwaggerUI
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v0/swagger.json", "CubiPool API v0"); });

			app.UseRouting();

			// Jwt Authentication
			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}