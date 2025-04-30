using Foodsharing.API.Constants;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Data;

public class SeedData
{
    /// <summary>
    /// Заполняет БД начальными данными
    /// </summary>
    /// <param name="modelBuilder">builder для построения модели данных</param>
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = new Guid("f05800c4-9e1a-453b-8409-41d46bf7e208"),
                Name = RolesConsts.Moderator
            },
            new Role
            {
                Id = new Guid("fc6be39a-58d5-4ab5-aa62-a20c4d28cee8"),
                Name = RolesConsts.User
            },
            new Role
            {
                Id = new Guid("c898637f-1b41-48e3-8a75-5bb99a5f6f5e"),
                Name = RolesConsts.Admin
            },
            new Role
            {
                Id = new Guid("de83b434-1710-4afa-a6bb-5069028e549c"),
                Name = RolesConsts.RepresentativeOrganization
            });

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = new Guid("b3d5fb72-7622-4c35-be37-c537d873e640"),
                Name = "Консервы"
            },
            new Category
            {
                Id = new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                Name = "Домашняя"
            },
            new Category
            {
                Id = new Guid("42139ca4-f9fc-41b9-8a89-0a8518177279"),
                Name = "Сладкое"
            },
            new Category
            {
                Id = new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                Name = "Крупа"
            },
            new Category
            {
                Id = new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                Name = "Снеки"
            });

        modelBuilder.Entity<TransactionStatus>().HasData(
            new TransactionStatus
            {
                Id = new Guid("bfdc2504-a005-4083-825a-c70a0cb2a1e5"),
                Name = TransactionStatusesConsts.IsCanceled
            },
            new TransactionStatus
            {
                Id = new Guid("7c524836-ec07-49ef-b1a8-3800f56b8423"),
                Name = TransactionStatusesConsts.IsBooked
            },
            new TransactionStatus
            {
                Id = new Guid("81b33e3c-001b-4a85-9b48-e79906bcd11a"),
                Name = TransactionStatusesConsts.IsCompleted
            });
        
        modelBuilder.Entity<MessageStatus>().HasData(
            new MessageStatus
            {
                Id = new Guid("7746ff41-8e79-4245-9cf3-6db42fa61543"),
                Name = MessageStatusesConsts.IsNotRead
            },
            new MessageStatus
            {
                Id = new Guid("66765c15-ceae-42d5-a719-0367316e9c80"),
                Name = MessageStatusesConsts.IsRead
            },
            new MessageStatus
            {
                Id = new Guid("7212fc64-2128-4a16-a60c-c52a4fc36784"),
                Name = MessageStatusesConsts.IsNotDelivered
            });

        modelBuilder.Entity<PartnershipApplicationStatus>().HasData(
            new PartnershipApplicationStatus
            {
                Id = new Guid("5978c5a3-7391-4edb-96eb-0d1f07b2c046"),
                Name = PartnershipApplicationStatusesConsts.IsPending
            },
            new PartnershipApplicationStatus
            {
                Id = new Guid("b8c07b15-c703-40ab-aa09-c3677108f8ea"),
                Name = PartnershipApplicationStatusesConsts.IsReviewed
            });

        modelBuilder.Entity<OrganizationStatus>().HasData(
            new OrganizationStatus
            {
                Id = new Guid("30098c3b-c9ce-4a22-bb0f-3f853a949e1f"),
                Name = OrganizationStatusesConsts.IsNotActive
            },
            new OrganizationStatus
            {
                Id = new Guid("7aa552f1-4ea9-4191-942e-fdbeb7eaeea8"),
                Name = OrganizationStatusesConsts.IsActive
            });

        modelBuilder.Entity<OrganizationForm>().HasData(
            new OrganizationForm
            {
                Id = new Guid("0fcc1133-d158-42f2-afdb-32b1818577dc"),
                OrganizationFormShortName = OrganizationFormsConsts.AOShort,
                OrganizationFormFullName = OrganizationFormsConsts.AOFull
            },
            new OrganizationForm
            {
                Id = new Guid("b3e819f0-7059-4bf5-ad1a-9988fd57bf2b"),
                OrganizationFormShortName = OrganizationFormsConsts.OOOShort,
                OrganizationFormFullName = OrganizationFormsConsts.OOOFull
            },
            new OrganizationForm
            {
                Id = new Guid("c080a543-5696-459a-a47a-77dd0713b492"),
                OrganizationFormShortName = OrganizationFormsConsts.IPShort,
                OrganizationFormFullName = OrganizationFormsConsts.IPFull
            });
    }
}
