using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// Навигационное свойство для связи с таблицей Profile
    /// </summary>
    public Profile? Profile { get; set; }
       
    /// <summary>
    /// Навигационное свойство для связи с таблицей Representative
    /// </summary>
    public RepresentativeOrganization? Representative { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Announcement
    /// </summary>
    public List<Announcement>? Announcements { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Transaction
    /// </summary>
    public List<Transaction>? Transactions { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Chat
    /// </summary>
    public List<Chat>? Chats { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Message
    /// </summary>
    public List<Message>? Messages { get; set; }

    /// <summary>
    /// Рейтинги
    /// </summary>
    public List<Rating>? Ratings { get; set; }

    /// <summary>
    /// Любимые категории
    /// </summary>
    public List<FavoriteCategory>? FavoriteCategories { get; set; }

    /// <summary>
    /// Любимые организации
    /// </summary>
    public List<FavoriteOrganization>? FavoriteOrganizations { get; set; }
    
    /// <summary>
    /// Заявки
    /// </summary>
    public List<PartnershipApplication>? Applications { get; set; }
}
