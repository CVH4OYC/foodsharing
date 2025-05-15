
using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Extensions;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Http;

namespace Foodsharing.API.Services;

public class UserService : IUserService
{

    private readonly IPasswordHasher passwordHasher;
    private readonly IUserRepository userRepository;
    private readonly IRoleRepository roleRepository;
    private readonly IUserRoleRepository userRoleRepository;
    private readonly IJwtProvider jwtProvider;
    private readonly IImageService imageService;
    private readonly IProfileRepository profileRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserService(IPasswordHasher passwordHasher,
                       IUserRepository userRepository,
                       IRoleRepository roleRepository,
                       IUserRoleRepository userRoleRepository,
                       IJwtProvider jwtProvider,
                       IImageService imageService,
                       IProfileRepository profileRepository,
                       IHttpContextAccessor httpContextAccessor)
    {
        this.passwordHasher = passwordHasher;
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.userRoleRepository = userRoleRepository;
        this.jwtProvider = jwtProvider;
        this.imageService = imageService;
        this.profileRepository = profileRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Метод регистрации нового пользователя
    /// </summary>
    /// <param name="request">Имя пользователя</param>
    /// <param name="roleName">Имя роли</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    public async Task<OperationResult> RegisterAsync(RegisterDTO request, string roleName, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        if (user != null)
            return OperationResult.FailureResult("Пользователь с таким именем уже существует");

        var hashedPassword = passwordHasher.Generate(request.Password);

        var role = await roleRepository.GetByRoleNameAsync(roleName, cancellationToken);
        if (role == null)
            return OperationResult.FailureResult("Роль не существует");

        var newUser = new User { UserName = request.UserName, Password = hashedPassword };
        await userRepository.AddAsync(newUser, cancellationToken);

        var userRole = new UserRole
        {
            UserId = newUser.Id,     // Присваиваем пользователя
            RoleId = role.Id      // Присваиваем роль
        };
        await userRoleRepository.AddAsync(userRole, cancellationToken);

        var profile = new Profile
        {
            UserId = newUser.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Bio = request.Bio,
            Image = await imageService.SaveImageAsync(request.ImageFile, PathsConsts.AvatarsFolder)
        };

        await userRepository.AddProfileAsync(profile, cancellationToken);

        var token = await jwtProvider.GenerateTokenAsync(newUser);
        return OperationResult.SuccessResult("Регистрация прошла успешно", token);
    }

    /// <summary>
    /// Метод идентификации и аутентификации пользователя по имени пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    public async Task<OperationResult> LoginAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var user = await GetByUserNameAsync(userName, cancellationToken);
        if (user == null)
            return OperationResult.FailureResult("Пользователь с таким именем не найден");
        var result = passwordHasher.Verify(password, user.Password);
        if (!result)
            return OperationResult.FailureResult("Ошибка входа, неверный пароль");

        var token = await jwtProvider.GenerateTokenAsync(user);
        return OperationResult.SuccessResult("Вход выполнен успешно", token);
    }

    public async Task<UserWithProfileDTO> GetMyProfileAsync(CancellationToken cancellationToken)
    {
        var currentUserId = httpContextAccessor.HttpContext?.User.GetUserId();
        return await GetUserProfileAsync((Guid)currentUserId, cancellationToken);
    }

    public async Task<UserWithProfileDTO> GetOtherProfileAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await GetUserProfileAsync((Guid)userId, cancellationToken);
    }

    private async Task<UserWithProfileDTO?> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.GetProfileWithUserName(userId, cancellationToken);
        return profile is null ? null : new UserWithProfileDTO
        {
            UserId = profile.UserId,
            UserName = profile.User.UserName,
            Image = profile.Image,
            Bio = profile.Bio,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
        };
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await userRepository.GetByUserNameAsync(userName, cancellationToken);
    }

    public async Task AddRepresentativeAsync(Guid userId, Guid orgId, CancellationToken cancellationToken)
    {
        var representative = new RepresentativeOrganization
        {
            UserId = userId,
            OrganizationId = orgId,
        };

        await userRepository.AddRepresentativeAsync(representative, cancellationToken);
    }
}
