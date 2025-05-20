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
            // Молочные продукты
            new Category
            {
                Id = new Guid("2d740f5a-173e-4f1b-974d-c007a2256c37"),
                Name = "Молочные продукты",
                Color = "#FFF0E6"
            },
            new Category
            {
                Id = new Guid("3a45831f-89fa-40ce-b236-35adcde88d56"),
                Name = "Сыры",
                ParentId = new Guid("2d740f5a-173e-4f1b-974d-c007a2256c37"),
                Color = "#FFD1B3"
            },
            new Category
            {
                Id = new Guid("863bbb48-1b44-4f05-a380-8995de42b86f"),
                Name = "Сметана",
                ParentId = new Guid("2d740f5a-173e-4f1b-974d-c007a2256c37"),
                Color = "#FFD1B3"
            },
            new Category
            {
                Id = new Guid("c09a6c67-bed1-479c-8a44-6a674ebb2bfa"),
                Name = "Молоко",
                ParentId = new Guid("2d740f5a-173e-4f1b-974d-c007a2256c37"),
                Color = "#FFD1B3"
            },

            // Напитки
            new Category
            {
                Id = new Guid("b3d5fb72-7622-4c35-be37-c537d873e640"),
                Name = "Напитки",
                Color = "#E6F7FF"
            },

            // Консервы
            new Category
            {
                Id = new Guid("1141deac-fddb-43c5-85e3-6c5b6d7ec314"),
                Name = "Консервы",
                Color = "#F5F5DC"
            },

            // Домашняя
            new Category
            {
                Id = new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                Name = "Домашняя",
                Color = "#E6FFE6"
            },
            new Category
            {
                Id = new Guid("bdd7ad45-2e8a-40b7-a712-0fafea40c718"),
                Name = "Супы",
                ParentId = new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                Color = "#C2E0C2"
            },
            new Category
            {
                Id = new Guid("0ed5ccca-722e-45d5-aef0-bb195c59a710"),
                Name = "Каши",
                ParentId = new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                Color = "#C2E0C2"
            },
            new Category
            {
                Id = new Guid("c5f5b7e3-7e24-4a2d-9af1-94c08bd26db5"),
                Name = "Салаты",
                ParentId = new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                Color = "#C2E0C2"
            },
            new Category
            {
                Id = new Guid("4de9dca2-2b6a-4f3c-9c02-56793ac3a222"),
                Name = "Выпечка",
                ParentId = new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                Color = "#C2E0C2"
            },

            // Сладкое
            new Category
            {
                Id = new Guid("42139ca4-f9fc-41b9-8a89-0a8518177279"),
                Name = "Сладкое",
                Color = "#FFF0F5"
            },

            // Крупы
            new Category
            {
                Id = new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                Name = "Крупы",
                Color = "#FAF0DC"
            },
            new Category
            {
                Id = new Guid("cf1fef74-cf50-49e4-b32e-b18c9e4c4567"),
                Name = "Гречка",
                ParentId = new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                Color = "#E6D5B8"
            },
            new Category
            {
                Id = new Guid("a8e43a0e-33fc-4d91-9c84-61e6c0ae0dc5"),
                Name = "Рис",
                ParentId = new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                Color = "#E6D5B8"
            },
            new Category
            {
                Id = new Guid("1e4f8de2-96c4-4901-9bb0-4bc9407db53b"),
                Name = "Овсянка",
                ParentId = new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                Color = "#E6D5B8"
            },

            // Снеки
            new Category
            {
                Id = new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                Name = "Снеки",
                Color = "#FFF6CC"
            },
            new Category
            {
                Id = new Guid("93f17e2f-6e53-4034-9d48-bf4cb12dbdf7"),
                Name = "Чипсы",
                ParentId = new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                Color = "#FFE699"
            },
            new Category
            {
                Id = new Guid("d1c3ab45-1997-4fc2-8ae4-278b5a19f46d"),
                Name = "Сухарики",
                ParentId = new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                Color = "#FFE699"
            },
            new Category
            {
                Id = new Guid("5b55a875-9621-47a2-9cf9-187589a3a9b3"),
                Name = "Орехи",
                ParentId = new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                Color = "#FFE699"
            }
        );


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
