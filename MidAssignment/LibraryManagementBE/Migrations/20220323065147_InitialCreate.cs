using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementBE.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ResponseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponseAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequest_AppUser_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequest_AppUser_ResponseById",
                        column: x => x.ResponseById,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CoverSrc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookEntity_CategoryEntity_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequestDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DetailOfRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetails_BookBorrowingRequest_DetailOfRequestId",
                        column: x => x.DetailOfRequestId,
                        principalTable: "BookBorrowingRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetails_BookEntity_BookId",
                        column: x => x.BookId,
                        principalTable: "BookEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9"), "31BF5413-8303-4E21-8D3A-10099FCA95FE", "Normal user", "NormalUser", "NORMALUSER" },
                    { new Guid("eb994f87-ed00-477e-ab20-d66214de73cc"), "94BD65EE-DE64-4476-91AA-6258155DE018", "Act like Admin", "SuperUser", "SUPERUSER" }
                });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FullName", "Gender", "IsDisabled", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Type", "UserName" },
                values: new object[,]
                {
                    { new Guid("1725b63b-f707-4b49-4ed2-08da06f835d7"), 0, "79c962b7-4035-4016-bb71-f8a69e2deda3", new DateTime(2000, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "nghia@gmail.com", false, "Le Trung Nghia", 0, false, false, null, "NGHIA@GMAIL.COM", "NGHIALT", "AQAAAAEAACcQAAAAEIlZvLNbRbavKqtei6MSGTYZmh3s0juNAmlKvOXMQ0DP+YDBmpCN9ryMHOFh3hlbAw==", null, false, "QXZYXIFQIFFM7TYFWNTSFT32V2J2Y7HM", false, 0, "nghialt" },
                    { new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"), 0, "65525777-0cc0-4364-8598-cdf93f0d5b14", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dai@gmail.com", false, "Pham Ngoc Dai", 0, false, false, null, "DAI@GMAIL.COM", "DAIPN", "AQAAAAEAACcQAAAAELvSL0FZEmF+U1eOPOPxZmlypBIxliBJCcynTzFGQmVd6FhiyAG9m56lFv6MNH4lNQ==", null, false, "ST4X4QYRAABOJUBI2OA2F6CSNATJF7WB", false, 1, "daipn" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("eb994f87-ed00-477e-ab20-d66214de73cc"), new Guid("1725b63b-f707-4b49-4ed2-08da06f835d7") },
                    { new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9"), new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7") }
                });

            migrationBuilder.InsertData(
                table: "CategoryEntity",
                columns: new[] { "Id", "CategoryName", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("4a3eda74-216c-4a99-9f97-8866250f15e6"), "Romance", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9515) },
                    { new Guid("5070844a-2416-4101-bbef-c82f18923b37"), "Isekai", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9532) },
                    { new Guid("741d9d50-8d50-498d-a73a-0e5074aca3c6"), "Mecha", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9533) }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequest",
                columns: new[] { "Id", "RequestedAt", "RequestedById", "ResponseAt", "ResponseById", "Status" },
                values: new object[,]
                {
                    { new Guid("5ce6d93a-1c3e-47dc-ab74-fb401029a715"), new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9572), new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"), null, null, 2 },
                    { new Guid("c9ea1f61-6658-415d-883a-abf3a31599d5"), new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9569), new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"), null, null, 2 },
                    { new Guid("eb73d5bd-c73f-448c-b6ca-f1c8e32f6443"), new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9574), new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"), null, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "BookEntity",
                columns: new[] { "Id", "CategoryId", "CoverSrc", "Description", "Name", "PublishedAt" },
                values: new object[,]
                {
                    { new Guid("1119ea52-5703-48e3-9917-76bb49612653"), new Guid("4a3eda74-216c-4a99-9f97-8866250f15e6"), "https://static.wikia.nocookie.net/mushokutensei/images/5/5f/LN_Vol_25_JP.jpg", "The story is about an unemployed otaku who ends his life at the age of 34 due to being hit by a truck. What is surprising is that he finds himself reincarnated in the form of a newborn baby, living in a strange world full of magic and swordsmanship.\nHis new name is Rudeus Grayrat, but he still remembers his previous life. The story revolves around life from childhood to adulthood in a wonderful but dangerous world", "Mukushou Tensei", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9553) },
                    { new Guid("53884e6a-7bf5-4f62-87ad-f02bff3274ca"), new Guid("4a3eda74-216c-4a99-9f97-8866250f15e6"), "https://static.wikia.nocookie.net/guiltycrown/images/f/fb/Guilty_Crown_poster.jpg", "In the near future, a meteorite carrying a strange virus (Apocalypse Virus) falls on Japan, leading to a biological disaster with a nationwide scale. And to help Japan, an international organization called GHQ came to the rescue. However, what Japan has to trade in return is independence. Ten years later (2039), a high school student named Ouma Shuu - who has a special power awakened by the Apocalypse Virus - coincidentally meets a girl named Yuzuriha Inori who is being hunted by GHQ. Since then, his life has been caught up in a series of unsolved mysteries", "Guilty Crown", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9551) },
                    { new Guid("bffb1013-98ef-4d79-a040-7b0443a32dd2"), new Guid("5070844a-2416-4101-bbef-c82f18923b37"), "https://s199.imacdn.com/tt24/2021/04/09/cd3319db9a8fbc65_784a78034e63d855_3031741617979942845957.jpg", "Makoto Misumi is just an ordinary high school student who lives an ordinary life, but is suddenly summoned to another world to become a \"hero\". However, the goddess of that world insulted him for being different and stripped him of his title of \"hero\", before banishing him to the wilderness at the edge of the world. As he roamed the wilderness, Makoto encountered dragons, spiders, orcs, dwarves, and all sorts of non-human tribes. Because Makoto came from another world, he was able to unleash magical powers and combat skills beyond imagination. But how will he handle when he meets many different creatures and survives in a new environment?", "Tsuki ga Michibiku Isekai Douchuu", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9556) },
                    { new Guid("f6029ede-ecbe-47e0-84ed-9e94cdcbc2b5"), new Guid("741d9d50-8d50-498d-a73a-0e5074aca3c6"), "https://static.wikia.nocookie.net/86-eighty-six/images/4/4c/Light_Novel_Volume_7_Cover.jpg", "The Republic of San Magnolia was attacked by its neighboring neighbor. It's an empire. In addition to the 85 counties of the Republic there is also \"the 86th district that does not exist\", where the elite young men and women continue to fight. Shin directs the combat of young suicide bombers, while Lena is the manager of a detachment of \"caretakers\" from the far rear. This story is about the tragic war between the two main characters above.", "Eighty Six", new DateTime(2022, 3, 23, 13, 51, 47, 235, DateTimeKind.Local).AddTicks(9546) }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "BookName", "DetailOfRequestId" },
                values: new object[] { new Guid("34995d6c-9a2f-43ee-b741-9c37442d6d0a"), new Guid("bffb1013-98ef-4d79-a040-7b0443a32dd2"), "Tsuki ga Michibiku Isekai Douchuu", new Guid("5ce6d93a-1c3e-47dc-ab74-fb401029a715") });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "BookName", "DetailOfRequestId" },
                values: new object[] { new Guid("bda7792a-225f-4847-8b06-a1adf5557070"), new Guid("1119ea52-5703-48e3-9917-76bb49612653"), "Mukushou Tensei", new Guid("c9ea1f61-6658-415d-883a-abf3a31599d5") });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "BookName", "DetailOfRequestId" },
                values: new object[] { new Guid("f47c6a96-3789-437d-bd46-b683f3321ba5"), new Guid("f6029ede-ecbe-47e0-84ed-9e94cdcbc2b5"), "Eighty Six", new Guid("eb73d5bd-c73f-448c-b6ca-f1c8e32f6443") });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequest_RequestedById",
                table: "BookBorrowingRequest",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequest_ResponseById",
                table: "BookBorrowingRequest",
                column: "ResponseById");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequestDetails_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequestDetails_DetailOfRequestId",
                table: "BookBorrowingRequestDetails",
                column: "DetailOfRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEntity_CategoryId",
                table: "BookEntity",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "BookBorrowingRequestDetails");

            migrationBuilder.DropTable(
                name: "BookBorrowingRequest");

            migrationBuilder.DropTable(
                name: "BookEntity");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "CategoryEntity");
        }
    }
}
