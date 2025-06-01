using Foodsharing.API.Data.ModelsConficurations;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Data;

/// <summary>
/// Контекст БД
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Коллекция сущностей типа Address
    /// </summary>
    public DbSet<Address> Addresses { get; set; }
   
    /// <summary>
    /// Коллекция сущностей типа Announcement
    /// </summary>
    public DbSet<Announcement> Announcements { get; set; }
    
    /// <summary>
    /// Коллекция сущностей типа Category
    /// </summary>
    public DbSet<Category> Categories { get; set; }
   
    /// <summary>
    /// Коллекция сущностей типа Chat
    /// </summary>
    public DbSet<Chat> Chats { get; set; }

    /// <summary>
    /// Коллекция сущностей типа FavoriteCategory
    /// </summary>
    public DbSet<FavoriteCategory> FavoriteCategories { get; set; }

    /// <summary>
    /// Коллекция сущностей типа FavoriteOrganization
    /// </summary>
    public DbSet<FavoriteOrganization> FavoriteOrganizations { get; set; }

    /// <summary>
    /// Коллекция сущностей типа Message
    /// </summary>
    public DbSet<Message> Messages { get; set; }
    
    /// <summary>
    /// Коллекция сущностей типа MessageStatus
    /// </summary>
    public DbSet<MessageStatus> MessageStatuses { get; set; }
    
    /// <summary>
    /// Коллекция сущностей типа Organization
    /// </summary>
    public DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Коллекция сущностей типа OrganizationForm
    /// </summary>
    public DbSet<OrganizationForm> OrganizationForms { get; set; }

    /// <summary>
    /// Коллекция сущностей типа OrganizationForm
    /// </summary>
    public DbSet<OrganizationStatus> OrganizationStatuses { get; set; }

    /// <summary>
    /// Коллекция сущностей типа PartnershipApplication
    /// </summary>
    public DbSet<PartnershipApplication> PartnershipApplications { get; set; }

    /// <summary>
    /// Коллекция сущностей типа PartnershipApplicationStatus
    /// </summary>
    public DbSet<PartnershipApplicationStatus> PartnershipApplicationStatuses { get; set; }

    /// <summary>
    /// Коллекция сущностей типа Profile
    /// </summary>
    public DbSet<Profile> Profiles { get; set; }

    /// <summary>
    /// Коллекция сущностей типа Rating
    /// </summary>
    public DbSet<Rating> Ratings { get; set; }

    /// <summary>
    /// Коллекция сущностей типа Representative
    /// </summary>
    public DbSet<RepresentativeOrganization> Representatives { get; set; }
    
    /// <summary>
    /// Коллекция сущностей типа Transaction
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; }
    
    /// <summary>
    /// Коллекция сущностей типа TransactionStatus
    /// </summary>
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }

    /// <summary>
    /// Коллекция сущностей типа UserRole
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Коллекция сущностей типа Role
    /// </summary>
    public DbSet<Role> Roles { get; set; }
    
    /// <summary>
    /// Коллекция сущностей типа User
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Уведомления
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    /// <summary>
    /// Типы уведомлений
    /// </summary>
    public DbSet<NotificationType> NotificationTypes { get; set; }

    /// <summary>
    /// Статусы уведомлений
    /// </summary>
    public DbSet<NotificationStatus> NotificationStatuses { get; set; }

    //public DbSet<AnnouncementStatus> AnnouncementStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteOrganizationConfiguration());

        SeedData.Seed(modelBuilder);
    }
}
