using Foodsharing.API.Constants;
using Foodsharing.API.Data;
using Foodsharing.API.Extensions;
using Foodsharing.API.Hubs;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Repository;
using Foodsharing.API.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

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
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IStatusesRepository, StatusesRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IPartnershipRepository, PartnershipRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();


builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IStringGenerator, StringGenerator>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPartnershipService, PartnershipService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IGeolocationService, GeolocationService>();

builder.Services.AddHttpClient<IGeocodingService, NominatimGeocodingService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();


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
app.MapHub<ChatHub>("/chathub");

StaticFileOptions CreateStaticFilesOptions(
    string rootFolder, // Базовая папка (Pictures/Files)
    string subFolder,  // Подпапка (Avatars/Announcements)
    IWebHostEnvironment env)
{
    var fullPath = Path.Combine(
        env.WebRootPath,
        rootFolder,
        subFolder);

    // Создаем папку, если она не существует
    Directory.CreateDirectory(fullPath);

    return new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(fullPath),
        RequestPath = $"/{rootFolder}/{subFolder}",
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

app.UseStaticFiles();

// Для Pictures
app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.PicturesFolder,
    PathsConsts.AvatarsFolder,
    app.Environment));

app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.PicturesFolder,
    PathsConsts.AnnouncementsFolder,
    app.Environment));

app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.PicturesFolder,
    PathsConsts.ChatImagesFolder,
    app.Environment));

app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.PicturesFolder,
    PathsConsts.MapImages,
    app.Environment));

app.UseStaticFiles(CreateStaticFilesOptions(
    PathsConsts.FilesFolder,
    PathsConsts.ChatFilesFolder,
    app.Environment));


app.Run();
