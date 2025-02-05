using Application.Helpers;
using Application.Interfaces;
using Application.Services;
using Core.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Hubs;
using System.ComponentModel.Design;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
	options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
		b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddHttpContextAccessor();

// Map the AppSettings mailSettings into the Helper class
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserOTPRepository, UserOTPRepository>();
builder.Services.AddScoped<IUserOTPService, UserOTPService>();

builder.Services.Configure<GoogleAuthConfig>(builder.Configuration.GetSection("Google"));
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();




builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<IBeautyService, BeautyService>();
builder.Services.AddScoped<IShopDressesService, ShopDressesService>();
builder.Services.AddScoped<IBeautyRepository, BeautyRepository>();

builder.Services.AddScoped<IShopDressesRepository, ShopDressesRepository>();

builder.Services.AddScoped<IPhotographyService, PhotographyService>();
builder.Services.AddScoped<IPhotographyRepository, PhotographyRepository>();


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();
builder.Services.AddScoped<IGovernorateServices, GovernorateServices>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityServices, CityServices>();


builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();

builder.Services.AddScoped<IHallService, HallService>();
builder.Services.AddScoped<IHallRepository, HallRepository>();

//builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

builder.Services.AddScoped<IFavoriteServiceLayer, FavoriteServiceLayer>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();


builder.Services.AddScoped<IRepository<Service>, Repository<Service>>();


builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();



/////////////////////////////////  SignalR ////////////////////////////////
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();



builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder => builder
		.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowCredentials()
		.SetIsOriginAllowed(alow => true));

});



// authentication services
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	var jwtKey = builder.Configuration["JWT:SecretKey"];
	if (string.IsNullOrEmpty(jwtKey))
	{
		throw new ArgumentNullException(nameof(jwtKey), "JWT Key is missing in configuration.");
	}

	options.SaveToken = true;
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuerSigningKey = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidAudience = builder.Configuration["JWT:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
	};
});




/*-----------------------------Swagger PArt-----------------------------*/
#region Swagger REgion

builder.Services.AddSwaggerGen(swagger =>
{
	swagger.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "ASP.NET 5 Web API",
		Description = "Farah-website"
	});

	swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
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

#endregion
//----------------------------------------------------------


var app = builder.Build();



using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<ApplicationDBContext>();
var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
	await dbContext.Database.MigrateAsync();
	await RoleInitializer.SeedRolesAsync(roleManager);
	await AdminInitializer.SeedAdminUserAsync(userManager);
	await GovernorateCityInitializer.AddDateSeeding(dbContext);
}
catch (Exception ex)
{
	var logger = loggerFactory.CreateLogger<Program>();
	logger.LogError(ex, "Error occurred during database migration or seeding");
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
	});
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestTimingMiddleware>();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHub<ChatHub>("/chathub");
});
app.MapHub<NotificationsHub>("notificationHub");

app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.None,
});

app.MapControllers();

app.Run();