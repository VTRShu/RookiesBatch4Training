using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBE.Repositories.EFContext;
public static class LibraryManagementSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {   
        modelBuilder.Entity<AppRole>().HasData(
            new AppRole
            {
                Id = new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9"),
                Name = "NormalUser",
                NormalizedName = "NORMALUSER",
                ConcurrencyStamp = "31BF5413-8303-4E21-8D3A-10099FCA95FE",
                Description = "Normal user",
            }, new AppRole
            {
                Id = new Guid("eb994f87-ed00-477e-ab20-d66214de73cc"),
                Name = "SuperUser",
                NormalizedName = "SUPERUSER",
                ConcurrencyStamp = "94BD65EE-DE64-4476-91AA-6258155DE018",
                Description = "Act like Admin",
            }
        );
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser()
            {
                Id = new Guid("1725b63b-f707-4b49-4ed2-08da06f835d7"),
                FullName = "Le Trung Nghia",
                Dob = new DateTime(2000,2,2),
                IsDisabled = false,
                Gender = (Gender)0,
                Type = (Role)0,
                UserName = "nghialt",
                NormalizedUserName = "NGHIALT",
                Email = "nghia@gmail.com",
                NormalizedEmail = "NGHIA@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEIlZvLNbRbavKqtei6MSGTYZmh3s0juNAmlKvOXMQ0DP+YDBmpCN9ryMHOFh3hlbAw==",
                SecurityStamp = "QXZYXIFQIFFM7TYFWNTSFT32V2J2Y7HM",
                ConcurrencyStamp = "79c962b7-4035-4016-bb71-f8a69e2deda3",
            },
            new AppUser()
            {
                Id = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                FullName = "Pham Ngoc Dai",
                Dob = new DateTime(2000,1,1),
                IsDisabled = false,
                Gender = (Gender)0,
                Type = (Role)1,
                UserName = "daipn",
                NormalizedUserName = "DAIPN",
                Email = "dai@gmail.com",
                NormalizedEmail = "DAI@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAELvSL0FZEmF+U1eOPOPxZmlypBIxliBJCcynTzFGQmVd6FhiyAG9m56lFv6MNH4lNQ==",
                SecurityStamp = "ST4X4QYRAABOJUBI2OA2F6CSNATJF7WB",
                ConcurrencyStamp = "65525777-0cc0-4364-8598-cdf93f0d5b14",
            }
        );
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>{
                UserId = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                RoleId = new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9")
            }, 
            new IdentityUserRole<Guid>{
                UserId = new Guid("1725b63b-f707-4b49-4ed2-08da06f835d7"),
                RoleId = new Guid("eb994f87-ed00-477e-ab20-d66214de73cc")
            }

        );

        modelBuilder.Entity<CategoryEntity>().HasData(
            new CategoryEntity{
                Id = new Guid("4a3eda74-216c-4a99-9f97-8866250f15e6"),
                CategoryName = "Romance",
                CreatedAt = DateTime.Now,
            },
            new CategoryEntity{
                Id = new Guid("5070844a-2416-4101-bbef-c82f18923b37"),
                CategoryName = "Isekai",
                CreatedAt = DateTime.Now,
            },
            new CategoryEntity{
                Id = new Guid("741d9d50-8d50-498d-a73a-0e5074aca3c6"),
                CategoryName = "Mecha",
                CreatedAt = DateTime.Now,
            }
        );
        modelBuilder.Entity<BookEntity>().HasData(
            new BookEntity{
                Id = new Guid("f6029ede-ecbe-47e0-84ed-9e94cdcbc2b5"),
                Name = "Eighty Six",
                Description = "The Republic of San Magnolia was attacked by its neighboring neighbor. It's an empire. In addition to the 85 counties of the Republic there is also \"the 86th district that does not exist\", where the elite young men and women continue to fight. Shin directs the combat of young suicide bombers, while Lena is the manager of a detachment of \"caretakers\" from the far rear. This story is about the tragic war between the two main characters above.",
                CategoryId = new Guid("741d9d50-8d50-498d-a73a-0e5074aca3c6"),
                PublishedAt = DateTime.Now,
                CoverSrc = "https://static.wikia.nocookie.net/86-eighty-six/images/4/4c/Light_Novel_Volume_7_Cover.jpg"
            },
            new BookEntity{
                Id = new Guid("53884e6a-7bf5-4f62-87ad-f02bff3274ca"),
                Name = "Guilty Crown",
                Description = "In the near future, a meteorite carrying a strange virus (Apocalypse Virus) falls on Japan, leading to a biological disaster with a nationwide scale. And to help Japan, an international organization called GHQ came to the rescue. However, what Japan has to trade in return is independence. Ten years later (2039), a high school student named Ouma Shuu - who has a special power awakened by the Apocalypse Virus - coincidentally meets a girl named Yuzuriha Inori who is being hunted by GHQ. Since then, his life has been caught up in a series of unsolved mysteries",
                CategoryId = new Guid("4a3eda74-216c-4a99-9f97-8866250f15e6"),
                PublishedAt = DateTime.Now,
                CoverSrc = "https://static.wikia.nocookie.net/guiltycrown/images/f/fb/Guilty_Crown_poster.jpg",
            },
            new BookEntity{
                Id = new Guid("1119ea52-5703-48e3-9917-76bb49612653"),
                Name = "Mukushou Tensei",
                Description = "The story is about an unemployed otaku who ends his life at the age of 34 due to being hit by a truck. What is surprising is that he finds himself reincarnated in the form of a newborn baby, living in a strange world full of magic and swordsmanship.\nHis new name is Rudeus Grayrat, but he still remembers his previous life. The story revolves around life from childhood to adulthood in a wonderful but dangerous world",
                CategoryId = new Guid("4a3eda74-216c-4a99-9f97-8866250f15e6"),
                PublishedAt = DateTime.Now,
                CoverSrc = "https://static.wikia.nocookie.net/mushokutensei/images/5/5f/LN_Vol_25_JP.jpg"
            },
            new BookEntity{
                Id = new Guid("bffb1013-98ef-4d79-a040-7b0443a32dd2"),
                Name = "Tsuki ga Michibiku Isekai Douchuu",
                CategoryId = new Guid("5070844a-2416-4101-bbef-c82f18923b37"),
                PublishedAt = DateTime.Now,
                Description = "Makoto Misumi is just an ordinary high school student who lives an ordinary life, but is suddenly summoned to another world to become a \"hero\". However, the goddess of that world insulted him for being different and stripped him of his title of \"hero\", before banishing him to the wilderness at the edge of the world. As he roamed the wilderness, Makoto encountered dragons, spiders, orcs, dwarves, and all sorts of non-human tribes. Because Makoto came from another world, he was able to unleash magical powers and combat skills beyond imagination. But how will he handle when he meets many different creatures and survives in a new environment?",
                CoverSrc = "https://s199.imacdn.com/tt24/2021/04/09/cd3319db9a8fbc65_784a78034e63d855_3031741617979942845957.jpg"
            }
        );
        modelBuilder.Entity<BookBorrowingRequest>().HasData(
            new BookBorrowingRequest(){
                Id = new Guid("c9ea1f61-6658-415d-883a-abf3a31599d5"),
                RequestedById = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                RequestedAt = DateTime.Now,
                Status = (Status)2,
                ResponseById = null,
                ResponseAt = null,
            },
            new BookBorrowingRequest(){
                Id = new Guid("5ce6d93a-1c3e-47dc-ab74-fb401029a715"),
                RequestedById = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                RequestedAt = DateTime.Now,
                Status = (Status)2,
                ResponseById = null,
                ResponseAt = null,
            },
            new BookBorrowingRequest(){
                Id = new Guid("eb73d5bd-c73f-448c-b6ca-f1c8e32f6443"),
                RequestedById = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                RequestedAt = DateTime.Now,
                Status = (Status)2,
                ResponseById = null,
                ResponseAt = null,
            }
        );

        modelBuilder.Entity<BookBorrowingRequestDetails>().HasData(
            new BookBorrowingRequestDetails()
            {
                Id = Guid.NewGuid(),
                DetailOfRequestId = new Guid("c9ea1f61-6658-415d-883a-abf3a31599d5"), 
                BookName = "Mukushou Tensei",
                BookId =  new Guid("1119ea52-5703-48e3-9917-76bb49612653"),
            },
            new BookBorrowingRequestDetails()
            {
                Id = Guid.NewGuid(),
                DetailOfRequestId = new Guid("5ce6d93a-1c3e-47dc-ab74-fb401029a715"), 
                BookName = "Tsuki ga Michibiku Isekai Douchuu",
                BookId = new Guid("bffb1013-98ef-4d79-a040-7b0443a32dd2"),
            },
            new BookBorrowingRequestDetails()
            {
                Id = Guid.NewGuid(),
                DetailOfRequestId = new Guid("eb73d5bd-c73f-448c-b6ca-f1c8e32f6443"), 
                BookName = "Eighty Six",
                BookId = new Guid("f6029ede-ecbe-47e0-84ed-9e94cdcbc2b5"),
            }
        );
    }
}