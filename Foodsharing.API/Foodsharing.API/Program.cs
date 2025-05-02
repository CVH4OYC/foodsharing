using System.Text;
using Foodsharing.API.Constants;
using Foodsharing.API.Data;
using Foodsharing.API.Extensions;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Foodsharing.API.Repository;
using Foodsharing.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


builder.Services.AddScoped<IUserService,UserService>();


builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connection = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connection);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "https://localhost:3000",  // Frontend
                "https://localhost:7044"   // Backend (для статических файлов)
            )
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Конфигурация статических файлов
StaticFileOptions CreateStaticFilesOptions(
    string folderName,
    IWebHostEnvironment env)
{
    return new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(
                env.WebRootPath,
                PathsConsts.PicturesFolder,
                folderName)),
        RequestPath = $"/{folderName}",
        ContentTypeProvider = new FileExtensionContentTypeProvider
        {
            Mappings =
            {
                [".webp"] = "image/webp",
                [".heic"] = "image/heic",
                [".heif"] = "image/heif"
            }
        },
        OnPrepareResponse = ctx =>
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800")
    };
}

// Аватары
app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.AvatarsFolder,
    app.Environment));

// Объявления
app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.AnnouncementsFolder,
    app.Environment));

app.Run();
