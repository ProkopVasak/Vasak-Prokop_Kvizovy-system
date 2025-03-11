using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizSystem.Migrations
{
    /// <inheritdoc />
    public partial class _01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    QuizId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d43bd02-bfea-4775-90f4-3c3fc3a64ed4", null, "User", "USER" },
                    { "fae08e16-9c3d-4df4-a1ef-6f286a68c3e4", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f36fad2b-bb8b-4075-9d00-7d37172c3373", 0, "7ca8ec8d-99d7-4838-8401-7c6f0fe54725", "admin@admin.cz", true, false, null, "ADMIN@ADMIN.CZ", "ADMIN@ADMIN.CZ", "AQAAAAIAAYagAAAAEPn/02ZlWXgLzkFMMLibu8DdwgHX8fk7EqJgSSKZ2yPgdPDHqs82MpbFVhRVER0XCw==", null, false, "razitko", false, "admin@admin.cz" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "Admin", "True", "f36fad2b-bb8b-4075-9d00-7d37172c3373" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fae08e16-9c3d-4df4-a1ef-6f286a68c3e4", "f36fad2b-bb8b-4075-9d00-7d37172c3373" });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "AuthorId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("020977a4-ea0b-482d-805c-b09b119f2669"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 8", "Testovací kvíz 8" },
                    { new Guid("0655a584-e8fb-4388-8107-f2e73835fb0a"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 13", "Testovací kvíz 13" },
                    { new Guid("0c433022-83de-41c4-a048-a61ad2d65451"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 30", "Testovací kvíz 30" },
                    { new Guid("0c5b3b2f-482c-4f3e-8f14-e0e6a411a5e8"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 24", "Testovací kvíz 24" },
                    { new Guid("0f0565ed-9194-45f4-a5b5-9e89bc4014b8"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 15", "Testovací kvíz 15" },
                    { new Guid("12893648-46d6-4efe-83b6-fbe04378cd43"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 16", "Testovací kvíz 16" },
                    { new Guid("1bd4dfdd-1c86-484c-b8bf-980adfae9cfd"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 21", "Testovací kvíz 21" },
                    { new Guid("28621e42-9038-495f-a2e9-bea209cd22af"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 23", "Testovací kvíz 23" },
                    { new Guid("38b254cf-8b3e-4f20-b188-996632ca1bf7"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 18", "Testovací kvíz 18" },
                    { new Guid("48f54e81-b385-4199-a2d3-dfd84371d345"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 28", "Testovací kvíz 28" },
                    { new Guid("49374444-e54a-4dd5-b754-d3ea6dc7e52c"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 17", "Testovací kvíz 17" },
                    { new Guid("58b8b71d-97ed-47ed-8149-87a82fff952e"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 22", "Testovací kvíz 22" },
                    { new Guid("7bd21a01-5831-47b6-94a2-a995a853049c"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 12", "Testovací kvíz 12" },
                    { new Guid("8143bbc7-4ba0-46d1-bcc1-8251f188081c"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 3", "Testovací kvíz 3" },
                    { new Guid("92403420-e8a0-4dba-b0ad-3a294f9468a0"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 6", "Testovací kvíz 6" },
                    { new Guid("9d7efc9e-5151-4a27-bc84-3d0d1c7b460a"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 7", "Testovací kvíz 7" },
                    { new Guid("a9f7c3d4-dd05-40b6-a938-2a29838fed57"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 25", "Testovací kvíz 25" },
                    { new Guid("bf7874ed-c13f-44bd-b7dc-8bcaeda7f7df"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 14", "Testovací kvíz 14" },
                    { new Guid("c1559a76-9656-4cc4-9c91-85160588b31e"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 27", "Testovací kvíz 27" },
                    { new Guid("c7156f63-5de9-427d-9f55-d9b1b3c02789"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 19", "Testovací kvíz 19" },
                    { new Guid("cd74cbc3-93b2-400f-b8f0-5a537d8ee1da"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 1", "Testovací kvíz 1" },
                    { new Guid("d4392f10-3405-4d2d-817b-74a8378419d2"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 11", "Testovací kvíz 11" },
                    { new Guid("d4bec8bd-3001-4891-8ae9-7d0eab2152cd"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 26", "Testovací kvíz 26" },
                    { new Guid("dcc9b0fd-5006-4f92-ba68-739af2c9cfc0"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 5", "Testovací kvíz 5" },
                    { new Guid("e0dd2f36-55cd-44bc-8049-86b213bbb351"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 9", "Testovací kvíz 9" },
                    { new Guid("e5357654-2bf0-4148-98c0-e6c4b0ed7124"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 4", "Testovací kvíz 4" },
                    { new Guid("e6694202-2014-4f65-9c4e-a9f7951398cf"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 29", "Testovací kvíz 29" },
                    { new Guid("edfabb0f-9823-49a9-9fb2-e07c4efa8c66"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 10", "Testovací kvíz 10" },
                    { new Guid("f509d9f1-b634-4af3-b2d3-c077435ea818"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 2", "Testovací kvíz 2" },
                    { new Guid("fa308f3d-6848-4316-af94-307a1eb69709"), "f36fad2b-bb8b-4075-9d00-7d37172c3373", "Popis pro testovací kvíz 20", "Testovací kvíz 20" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "QuizId", "Text" },
                values: new object[,]
                {
                    { new Guid("032fe13f-7d6f-49e5-b3e3-58bbef3a1ab8"), new Guid("edfabb0f-9823-49a9-9fb2-e07c4efa8c66"), "Kolik je 5 + 5?" },
                    { new Guid("03b03af3-bc5c-4ec6-a9a3-399709a1f8d6"), new Guid("dcc9b0fd-5006-4f92-ba68-739af2c9cfc0"), "Kolik je 5 + 5?" },
                    { new Guid("097512f4-d565-4dda-bb49-e47e640232c2"), new Guid("fa308f3d-6848-4316-af94-307a1eb69709"), "Kolik je 10 / 2?" },
                    { new Guid("0cd03716-dc39-4812-aa16-b47ad3d16e4d"), new Guid("9d7efc9e-5151-4a27-bc84-3d0d1c7b460a"), "Kolik je 5 + 5?" },
                    { new Guid("10ac36f9-8202-4edf-b1b4-de78d38881c6"), new Guid("58b8b71d-97ed-47ed-8149-87a82fff952e"), "Kolik je 5 + 5?" },
                    { new Guid("12d8992a-eea8-4059-9369-e7b9547104bf"), new Guid("38b254cf-8b3e-4f20-b188-996632ca1bf7"), "Kolik je 5 + 5?" },
                    { new Guid("14021bd0-5773-4559-a779-d3557519f43a"), new Guid("28621e42-9038-495f-a2e9-bea209cd22af"), "Kolik je 5 + 5?" },
                    { new Guid("16467ca6-1f87-4a80-af2d-d9c681bdd9a2"), new Guid("c1559a76-9656-4cc4-9c91-85160588b31e"), "Kolik je 10 / 2?" },
                    { new Guid("17250bf3-823e-4382-9094-e76938618829"), new Guid("0c5b3b2f-482c-4f3e-8f14-e0e6a411a5e8"), "Kolik je 10 / 2?" },
                    { new Guid("1cba7e32-dd41-434d-ad53-214b13d80b45"), new Guid("9d7efc9e-5151-4a27-bc84-3d0d1c7b460a"), "Kolik je 10 / 2?" },
                    { new Guid("25fe1413-ab52-4495-98a2-318753eb00b3"), new Guid("0c433022-83de-41c4-a048-a61ad2d65451"), "Kolik je 10 / 2?" },
                    { new Guid("2ea5ae45-b5bf-4f33-88eb-4fb9600590f5"), new Guid("7bd21a01-5831-47b6-94a2-a995a853049c"), "Kolik je 10 / 2?" },
                    { new Guid("31f510c9-de5f-48ce-94a1-31d796727686"), new Guid("28621e42-9038-495f-a2e9-bea209cd22af"), "Kolik je 10 / 2?" },
                    { new Guid("347fee97-06f8-4125-8c15-684e3a3147c8"), new Guid("48f54e81-b385-4199-a2d3-dfd84371d345"), "Kolik je 10 / 2?" },
                    { new Guid("372f5dcc-6cd8-44ca-a218-89d16f7c35a2"), new Guid("d4bec8bd-3001-4891-8ae9-7d0eab2152cd"), "Kolik je 5 + 5?" },
                    { new Guid("38e38aa2-943a-4bec-8b93-7ef8302530c3"), new Guid("12893648-46d6-4efe-83b6-fbe04378cd43"), "Kolik je 10 / 2?" },
                    { new Guid("3915ec2a-e3e3-4937-855c-3f256ce02dce"), new Guid("bf7874ed-c13f-44bd-b7dc-8bcaeda7f7df"), "Kolik je 5 + 5?" },
                    { new Guid("3b4e0829-6d53-4416-9caf-207fa2c6ddcb"), new Guid("e0dd2f36-55cd-44bc-8049-86b213bbb351"), "Kolik je 10 / 2?" },
                    { new Guid("4bae6034-cc74-4249-8fad-1fa9ee99718e"), new Guid("12893648-46d6-4efe-83b6-fbe04378cd43"), "Kolik je 5 + 5?" },
                    { new Guid("5746858f-0700-4d70-bcb1-775e8adbd5d8"), new Guid("e6694202-2014-4f65-9c4e-a9f7951398cf"), "Kolik je 10 / 2?" },
                    { new Guid("58249c49-7d11-4b20-8b2f-cf007700f03d"), new Guid("edfabb0f-9823-49a9-9fb2-e07c4efa8c66"), "Kolik je 10 / 2?" },
                    { new Guid("5a94df5b-2046-41fc-978c-5426ff9bb66d"), new Guid("020977a4-ea0b-482d-805c-b09b119f2669"), "Kolik je 5 + 5?" },
                    { new Guid("66d7a06b-9481-495d-972f-5a38e2ced6b6"), new Guid("0c433022-83de-41c4-a048-a61ad2d65451"), "Kolik je 5 + 5?" },
                    { new Guid("6acd0b36-a1cf-46e9-9034-10a90e98c0ce"), new Guid("c7156f63-5de9-427d-9f55-d9b1b3c02789"), "Kolik je 10 / 2?" },
                    { new Guid("70235f7f-a493-4a55-9318-7c2c8ec6d252"), new Guid("bf7874ed-c13f-44bd-b7dc-8bcaeda7f7df"), "Kolik je 10 / 2?" },
                    { new Guid("71e7615a-95e4-4033-85b2-1bfee6e88818"), new Guid("f509d9f1-b634-4af3-b2d3-c077435ea818"), "Kolik je 10 / 2?" },
                    { new Guid("728e68e6-859b-4cb5-979d-ccf545655cd1"), new Guid("1bd4dfdd-1c86-484c-b8bf-980adfae9cfd"), "Kolik je 5 + 5?" },
                    { new Guid("7a8b2989-057e-4b37-acb8-231f4347f3cb"), new Guid("a9f7c3d4-dd05-40b6-a938-2a29838fed57"), "Kolik je 10 / 2?" },
                    { new Guid("7e1c6a98-bff4-440e-b1e2-9acbcdf8a554"), new Guid("e5357654-2bf0-4148-98c0-e6c4b0ed7124"), "Kolik je 10 / 2?" },
                    { new Guid("7ecbc820-07a1-4218-88a9-7d4ac7018796"), new Guid("0f0565ed-9194-45f4-a5b5-9e89bc4014b8"), "Kolik je 5 + 5?" },
                    { new Guid("81ae3beb-edd9-4964-bb78-d00c345b98fd"), new Guid("c7156f63-5de9-427d-9f55-d9b1b3c02789"), "Kolik je 5 + 5?" },
                    { new Guid("8202c439-63e5-448d-a152-a1265b42d475"), new Guid("49374444-e54a-4dd5-b754-d3ea6dc7e52c"), "Kolik je 5 + 5?" },
                    { new Guid("839d2942-2745-47d3-9294-14868e689566"), new Guid("d4bec8bd-3001-4891-8ae9-7d0eab2152cd"), "Kolik je 10 / 2?" },
                    { new Guid("850c63df-aede-49f4-8146-9ab4ecf7ea23"), new Guid("f509d9f1-b634-4af3-b2d3-c077435ea818"), "Kolik je 5 + 5?" },
                    { new Guid("889bef52-78fd-47fc-8181-fb1880dc676c"), new Guid("38b254cf-8b3e-4f20-b188-996632ca1bf7"), "Kolik je 10 / 2?" },
                    { new Guid("8b8f6360-3bea-404c-99dc-45375e3df53b"), new Guid("c1559a76-9656-4cc4-9c91-85160588b31e"), "Kolik je 5 + 5?" },
                    { new Guid("902db909-32dc-409e-8d85-57b7db2732c7"), new Guid("0c5b3b2f-482c-4f3e-8f14-e0e6a411a5e8"), "Kolik je 5 + 5?" },
                    { new Guid("94e860a0-52cb-430d-a455-a22f69638a34"), new Guid("fa308f3d-6848-4316-af94-307a1eb69709"), "Kolik je 5 + 5?" },
                    { new Guid("9981b2e0-f21a-4cf1-adfa-ac72c582f78f"), new Guid("0655a584-e8fb-4388-8107-f2e73835fb0a"), "Kolik je 5 + 5?" },
                    { new Guid("99fa2ebb-6a85-48bd-8e43-996c02ef1807"), new Guid("58b8b71d-97ed-47ed-8149-87a82fff952e"), "Kolik je 10 / 2?" },
                    { new Guid("a1b8cd94-1788-4f22-ab06-032d824fd707"), new Guid("e5357654-2bf0-4148-98c0-e6c4b0ed7124"), "Kolik je 5 + 5?" },
                    { new Guid("a1f62901-23ea-4fac-8b0b-e91ca2087fc9"), new Guid("020977a4-ea0b-482d-805c-b09b119f2669"), "Kolik je 10 / 2?" },
                    { new Guid("a9d49ce6-03e3-4269-9a83-54213a272326"), new Guid("1bd4dfdd-1c86-484c-b8bf-980adfae9cfd"), "Kolik je 10 / 2?" },
                    { new Guid("c6965812-c38b-403c-b5bc-b1fd553bef8f"), new Guid("e6694202-2014-4f65-9c4e-a9f7951398cf"), "Kolik je 5 + 5?" },
                    { new Guid("c6a766e3-74ca-4100-9e66-174e6cc03032"), new Guid("cd74cbc3-93b2-400f-b8f0-5a537d8ee1da"), "Kolik je 10 / 2?" },
                    { new Guid("d0f522ca-e8f2-4a24-87ec-1d26cf88a51c"), new Guid("49374444-e54a-4dd5-b754-d3ea6dc7e52c"), "Kolik je 10 / 2?" },
                    { new Guid("d1567857-4501-4f45-957e-82b4f7e66c6c"), new Guid("7bd21a01-5831-47b6-94a2-a995a853049c"), "Kolik je 5 + 5?" },
                    { new Guid("d21045b8-b2a4-447f-a20f-7846f90eeb1e"), new Guid("8143bbc7-4ba0-46d1-bcc1-8251f188081c"), "Kolik je 5 + 5?" },
                    { new Guid("d531e747-307f-4527-9bf1-a8c084d71d5a"), new Guid("0f0565ed-9194-45f4-a5b5-9e89bc4014b8"), "Kolik je 10 / 2?" },
                    { new Guid("d64ce8ba-fe76-45ca-9a0e-d3b9ab5cd752"), new Guid("92403420-e8a0-4dba-b0ad-3a294f9468a0"), "Kolik je 10 / 2?" },
                    { new Guid("d8463ab6-8483-492c-a227-54ccc93a8144"), new Guid("0655a584-e8fb-4388-8107-f2e73835fb0a"), "Kolik je 10 / 2?" },
                    { new Guid("d9fb34b7-7081-4675-9d85-1bd9a99a48b7"), new Guid("a9f7c3d4-dd05-40b6-a938-2a29838fed57"), "Kolik je 5 + 5?" },
                    { new Guid("e1bb17dd-58fb-41bd-88d1-f07e0ea100d3"), new Guid("dcc9b0fd-5006-4f92-ba68-739af2c9cfc0"), "Kolik je 10 / 2?" },
                    { new Guid("e20e7005-e306-4900-ab25-d7087dcd79be"), new Guid("cd74cbc3-93b2-400f-b8f0-5a537d8ee1da"), "Kolik je 5 + 5?" },
                    { new Guid("eb72845e-f7a2-4a99-b323-efb98228ff23"), new Guid("8143bbc7-4ba0-46d1-bcc1-8251f188081c"), "Kolik je 10 / 2?" },
                    { new Guid("edcda0ad-8054-4db9-a6bb-10542b39ff12"), new Guid("48f54e81-b385-4199-a2d3-dfd84371d345"), "Kolik je 5 + 5?" },
                    { new Guid("f1092470-f78c-4036-aaec-43288aa8b641"), new Guid("e0dd2f36-55cd-44bc-8049-86b213bbb351"), "Kolik je 5 + 5?" },
                    { new Guid("f159a70a-9876-49ad-8ac4-5e79de21a9ff"), new Guid("92403420-e8a0-4dba-b0ad-3a294f9468a0"), "Kolik je 5 + 5?" },
                    { new Guid("fbe7c04a-e723-4800-9ff1-3d38be7e20eb"), new Guid("d4392f10-3405-4d2d-817b-74a8378419d2"), "Kolik je 10 / 2?" },
                    { new Guid("fd29bd36-6aa9-4362-8365-d1f8ab2ab92e"), new Guid("d4392f10-3405-4d2d-817b-74a8378419d2"), "Kolik je 5 + 5?" }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { new Guid("000a6064-fd53-4d46-966a-29bf119646c9"), true, new Guid("347fee97-06f8-4125-8c15-684e3a3147c8"), "5" },
                    { new Guid("005a6203-76db-41a4-a112-ad54c1ed4017"), false, new Guid("d9fb34b7-7081-4675-9d85-1bd9a99a48b7"), "20" },
                    { new Guid("012374e1-a77b-4089-88e6-2aee36e59003"), false, new Guid("a9d49ce6-03e3-4269-9a83-54213a272326"), "15" },
                    { new Guid("0161ac82-3cfa-4cbc-ac93-0dd9faeb16c6"), false, new Guid("58249c49-7d11-4b20-8b2f-cf007700f03d"), "25" },
                    { new Guid("03bcf379-30df-42bb-a60a-6efe1eb0d6da"), false, new Guid("7e1c6a98-bff4-440e-b1e2-9acbcdf8a554"), "25" },
                    { new Guid("061d093d-8ee2-4b94-a3e7-e8e82032f11b"), true, new Guid("5a94df5b-2046-41fc-978c-5426ff9bb66d"), "10" },
                    { new Guid("06bbabea-45eb-4dec-ae10-198dd36f20d3"), true, new Guid("4bae6034-cc74-4249-8fad-1fa9ee99718e"), "10" },
                    { new Guid("06cf1f64-2ddc-44d3-aad7-9c9c6818eedc"), false, new Guid("d64ce8ba-fe76-45ca-9a0e-d3b9ab5cd752"), "15" },
                    { new Guid("09c97189-a31a-4dbb-9f09-cb3827f51f70"), false, new Guid("c6a766e3-74ca-4100-9e66-174e6cc03032"), "25" },
                    { new Guid("11134e8f-1d6c-478b-8e61-a69cc526420b"), true, new Guid("d9fb34b7-7081-4675-9d85-1bd9a99a48b7"), "10" },
                    { new Guid("113a1e70-03a0-44f4-8033-48513880993d"), false, new Guid("f159a70a-9876-49ad-8ac4-5e79de21a9ff"), "15" },
                    { new Guid("13015e04-a050-43aa-a522-c85760d2368c"), false, new Guid("d8463ab6-8483-492c-a227-54ccc93a8144"), "25" },
                    { new Guid("13d27647-9e23-45f2-a222-f7dc2ce26d4d"), false, new Guid("70235f7f-a493-4a55-9318-7c2c8ec6d252"), "15" },
                    { new Guid("14b27637-d123-47e1-a0ce-5bc1e751a4db"), false, new Guid("1cba7e32-dd41-434d-ad53-214b13d80b45"), "25" },
                    { new Guid("1524dc87-2d93-438b-a9f4-68d756cfe71b"), false, new Guid("032fe13f-7d6f-49e5-b3e3-58bbef3a1ab8"), "25" },
                    { new Guid("152fc818-31c4-443e-b43c-6c2758d8b216"), false, new Guid("d64ce8ba-fe76-45ca-9a0e-d3b9ab5cd752"), "25" },
                    { new Guid("193fe482-1628-4481-9572-37fe0adb2729"), false, new Guid("c6965812-c38b-403c-b5bc-b1fd553bef8f"), "15" },
                    { new Guid("1a7aca24-803a-49c5-9c94-2faba6e55624"), false, new Guid("d21045b8-b2a4-447f-a20f-7846f90eeb1e"), "25" },
                    { new Guid("1a81ae60-5628-4868-ac64-2290f07d1c2a"), false, new Guid("17250bf3-823e-4382-9094-e76938618829"), "25" },
                    { new Guid("1c9e713b-59a5-4c52-ae3f-3f8a9ebba06e"), false, new Guid("d531e747-307f-4527-9bf1-a8c084d71d5a"), "25" },
                    { new Guid("1d290b4c-b21b-4ac0-bcb2-aba80f5942d4"), true, new Guid("c6a766e3-74ca-4100-9e66-174e6cc03032"), "5" },
                    { new Guid("20590947-2588-41d0-b692-f92a1ba69f7a"), false, new Guid("12d8992a-eea8-4059-9369-e7b9547104bf"), "20" },
                    { new Guid("209c8e92-e0c9-4585-807b-0123b4f72812"), false, new Guid("e1bb17dd-58fb-41bd-88d1-f07e0ea100d3"), "25" },
                    { new Guid("20e84714-da79-4fad-a576-bc09579295de"), false, new Guid("8b8f6360-3bea-404c-99dc-45375e3df53b"), "15" },
                    { new Guid("228d8006-7d2f-4ac1-8a22-24e88b52db6f"), true, new Guid("a9d49ce6-03e3-4269-9a83-54213a272326"), "5" },
                    { new Guid("22cd5162-0090-481a-84b3-16875dec47e1"), false, new Guid("8202c439-63e5-448d-a152-a1265b42d475"), "25" },
                    { new Guid("22de7f71-48c9-46d5-9764-a583ff8a865d"), false, new Guid("f1092470-f78c-4036-aaec-43288aa8b641"), "15" },
                    { new Guid("241b4778-8dcd-447e-92a3-7a53c435a943"), false, new Guid("032fe13f-7d6f-49e5-b3e3-58bbef3a1ab8"), "15" },
                    { new Guid("2697a146-ccd7-46da-85ec-a9d1eefb8511"), false, new Guid("3b4e0829-6d53-4416-9caf-207fa2c6ddcb"), "15" },
                    { new Guid("26a78a72-a36d-41f9-b5e2-37b91af09b40"), true, new Guid("7e1c6a98-bff4-440e-b1e2-9acbcdf8a554"), "5" },
                    { new Guid("290f246b-ce04-4790-85f9-887132088d0b"), false, new Guid("99fa2ebb-6a85-48bd-8e43-996c02ef1807"), "15" },
                    { new Guid("2c0a827b-018c-4bdb-8c5f-f17bf85401d4"), false, new Guid("c6965812-c38b-403c-b5bc-b1fd553bef8f"), "20" },
                    { new Guid("2e35bc40-6c0e-4d9d-a87e-956743af45e8"), false, new Guid("66d7a06b-9481-495d-972f-5a38e2ced6b6"), "25" },
                    { new Guid("2e8820c9-9fe2-4966-ad4d-84c4f5991c59"), false, new Guid("f1092470-f78c-4036-aaec-43288aa8b641"), "25" },
                    { new Guid("2e930123-2a13-4166-984a-88c163073405"), true, new Guid("728e68e6-859b-4cb5-979d-ccf545655cd1"), "10" },
                    { new Guid("2f270e0d-5660-4262-8706-e7df10b162c6"), false, new Guid("839d2942-2745-47d3-9294-14868e689566"), "15" },
                    { new Guid("325548d9-5b92-4009-8d52-ef5d1942e0a7"), true, new Guid("d1567857-4501-4f45-957e-82b4f7e66c6c"), "10" },
                    { new Guid("33e138c3-331c-431e-aa43-2418214e3b64"), false, new Guid("58249c49-7d11-4b20-8b2f-cf007700f03d"), "20" },
                    { new Guid("3426d788-c00f-4dfe-9621-ba616a8c9471"), false, new Guid("4bae6034-cc74-4249-8fad-1fa9ee99718e"), "20" },
                    { new Guid("34335c53-0873-45f3-a306-1c119d834082"), false, new Guid("850c63df-aede-49f4-8146-9ab4ecf7ea23"), "25" },
                    { new Guid("36c0f5c8-5714-43c4-b794-a3e8908940bc"), true, new Guid("d64ce8ba-fe76-45ca-9a0e-d3b9ab5cd752"), "5" },
                    { new Guid("36ebe2f1-deff-4d48-9c8d-3d16162327cb"), false, new Guid("10ac36f9-8202-4edf-b1b4-de78d38881c6"), "20" },
                    { new Guid("3742b5b3-f469-4fbe-a81f-417467390100"), false, new Guid("e20e7005-e306-4900-ab25-d7087dcd79be"), "25" },
                    { new Guid("3948e9bd-eab2-4a98-9d16-13a78fb06fb6"), false, new Guid("58249c49-7d11-4b20-8b2f-cf007700f03d"), "15" },
                    { new Guid("3a90fc7c-1f76-433d-9cd8-b7c46dbad1ca"), false, new Guid("38e38aa2-943a-4bec-8b93-7ef8302530c3"), "25" },
                    { new Guid("3ac900fb-4ca6-464e-94d5-ac8474db2a81"), false, new Guid("0cd03716-dc39-4812-aa16-b47ad3d16e4d"), "20" },
                    { new Guid("3c962012-23d0-42c6-af1c-b582c39d49c3"), false, new Guid("372f5dcc-6cd8-44ca-a218-89d16f7c35a2"), "25" },
                    { new Guid("3c9ff93c-7855-4165-b14a-81eb584996cd"), true, new Guid("839d2942-2745-47d3-9294-14868e689566"), "5" },
                    { new Guid("3dfaef98-80a9-408a-982c-1460f4ee74a5"), true, new Guid("097512f4-d565-4dda-bb49-e47e640232c2"), "5" },
                    { new Guid("3fd40d04-415e-46ae-bb48-78b51809baa5"), true, new Guid("99fa2ebb-6a85-48bd-8e43-996c02ef1807"), "5" },
                    { new Guid("41fb2b25-166c-4262-bab0-8a0037c9105d"), true, new Guid("6acd0b36-a1cf-46e9-9034-10a90e98c0ce"), "5" },
                    { new Guid("423d8643-4c43-4bc9-b974-4d66b8514346"), true, new Guid("16467ca6-1f87-4a80-af2d-d9c681bdd9a2"), "5" },
                    { new Guid("424edd24-4089-4263-a6d7-0c2fba02996b"), false, new Guid("902db909-32dc-409e-8d85-57b7db2732c7"), "15" },
                    { new Guid("459525c5-abfd-4857-9c04-05bcd829facb"), false, new Guid("8b8f6360-3bea-404c-99dc-45375e3df53b"), "25" },
                    { new Guid("46305104-1cce-4dd8-b4d8-9141bd746f1a"), false, new Guid("3b4e0829-6d53-4416-9caf-207fa2c6ddcb"), "25" },
                    { new Guid("484ac33c-3058-45cb-843e-c8665cd7b503"), false, new Guid("3915ec2a-e3e3-4937-855c-3f256ce02dce"), "15" },
                    { new Guid("484bdafe-e52d-4b01-93aa-6b3572bd63fe"), false, new Guid("edcda0ad-8054-4db9-a6bb-10542b39ff12"), "15" },
                    { new Guid("48bdb457-be0c-4f4c-bfa9-351139eaaf58"), false, new Guid("7ecbc820-07a1-4218-88a9-7d4ac7018796"), "20" },
                    { new Guid("49180c87-62df-4ddd-87db-159c07a53afc"), false, new Guid("14021bd0-5773-4559-a779-d3557519f43a"), "25" },
                    { new Guid("49644af0-23fd-43d6-8e78-2e7cc054cf02"), false, new Guid("8b8f6360-3bea-404c-99dc-45375e3df53b"), "20" },
                    { new Guid("4c644dcb-6f20-4976-8edf-57032c523c0f"), false, new Guid("a1b8cd94-1788-4f22-ab06-032d824fd707"), "25" },
                    { new Guid("4cb38571-7c82-4209-a06d-c25755ab6f4c"), false, new Guid("6acd0b36-a1cf-46e9-9034-10a90e98c0ce"), "25" },
                    { new Guid("4ce195c1-a8f4-457b-a37c-26ce59cc9cd8"), true, new Guid("71e7615a-95e4-4033-85b2-1bfee6e88818"), "5" },
                    { new Guid("4f3b5d66-a50b-4d35-8968-9f29aa883071"), false, new Guid("889bef52-78fd-47fc-8181-fb1880dc676c"), "15" },
                    { new Guid("4f5e8735-5842-49a9-b478-ec55d890d241"), false, new Guid("03b03af3-bc5c-4ec6-a9a3-399709a1f8d6"), "25" },
                    { new Guid("4f8019b5-351b-4c91-956a-8ba223189bc4"), true, new Guid("e1bb17dd-58fb-41bd-88d1-f07e0ea100d3"), "5" },
                    { new Guid("50251a8b-1793-4a2f-8bd6-9c9d939efdf4"), false, new Guid("7e1c6a98-bff4-440e-b1e2-9acbcdf8a554"), "20" },
                    { new Guid("506aa6cc-e95d-4149-8bd6-6c7fa7144e9a"), true, new Guid("58249c49-7d11-4b20-8b2f-cf007700f03d"), "5" },
                    { new Guid("50895037-409d-4964-ad1d-19fbaf87d933"), false, new Guid("66d7a06b-9481-495d-972f-5a38e2ced6b6"), "15" },
                    { new Guid("50fc7126-f4a1-4024-9145-9a2456c5f5e3"), false, new Guid("2ea5ae45-b5bf-4f33-88eb-4fb9600590f5"), "25" },
                    { new Guid("51a278f1-4a06-4419-a13a-e57e2bd2f1ff"), true, new Guid("66d7a06b-9481-495d-972f-5a38e2ced6b6"), "10" },
                    { new Guid("51a4094f-53aa-41b8-9063-bf1b6fc76bd7"), false, new Guid("10ac36f9-8202-4edf-b1b4-de78d38881c6"), "15" },
                    { new Guid("51f7177b-b012-49be-a4b2-a85df0abc4a2"), false, new Guid("032fe13f-7d6f-49e5-b3e3-58bbef3a1ab8"), "20" },
                    { new Guid("5494e160-4be0-4fcc-adea-bde6dbd07dc6"), false, new Guid("d8463ab6-8483-492c-a227-54ccc93a8144"), "20" },
                    { new Guid("549f354e-17dc-41e5-95ed-4059a0501e44"), false, new Guid("d64ce8ba-fe76-45ca-9a0e-d3b9ab5cd752"), "20" },
                    { new Guid("54c0f89e-2d3c-4685-a910-5fc232b18346"), false, new Guid("c6a766e3-74ca-4100-9e66-174e6cc03032"), "20" },
                    { new Guid("55849b94-8eb3-4f48-9dc2-bb17dfccaa68"), false, new Guid("fbe7c04a-e723-4800-9ff1-3d38be7e20eb"), "15" },
                    { new Guid("566f1d5c-acc8-46ec-a3ae-70a2c5bef706"), true, new Guid("f1092470-f78c-4036-aaec-43288aa8b641"), "10" },
                    { new Guid("57866a57-0ca8-4c43-92ab-e3d8057521d1"), false, new Guid("372f5dcc-6cd8-44ca-a218-89d16f7c35a2"), "15" },
                    { new Guid("5821e68b-bfc5-45a7-a8bb-57d4437a598f"), false, new Guid("7a8b2989-057e-4b37-acb8-231f4347f3cb"), "15" },
                    { new Guid("58255a3a-5875-44f0-a457-ad0181b70d24"), false, new Guid("03b03af3-bc5c-4ec6-a9a3-399709a1f8d6"), "20" },
                    { new Guid("586b3202-76d5-479a-96bc-d89b42527ea6"), false, new Guid("8202c439-63e5-448d-a152-a1265b42d475"), "20" },
                    { new Guid("5c6f16c8-1015-43da-97d6-9510b8e320ee"), false, new Guid("a9d49ce6-03e3-4269-9a83-54213a272326"), "20" },
                    { new Guid("5d1c08ba-f9a9-4a66-88f2-067cec56c85e"), true, new Guid("032fe13f-7d6f-49e5-b3e3-58bbef3a1ab8"), "10" },
                    { new Guid("60ef69f7-5d23-4e2b-83f5-f82fcf90f8d0"), false, new Guid("d9fb34b7-7081-4675-9d85-1bd9a99a48b7"), "15" },
                    { new Guid("61cbf077-db39-49dc-adca-769623cae7f3"), false, new Guid("edcda0ad-8054-4db9-a6bb-10542b39ff12"), "20" },
                    { new Guid("69454b01-a5a3-4e47-b2cc-c64f77a6be84"), false, new Guid("839d2942-2745-47d3-9294-14868e689566"), "20" },
                    { new Guid("6a589c9f-26f1-4cbd-bbb7-e4b93ba355eb"), false, new Guid("8202c439-63e5-448d-a152-a1265b42d475"), "15" },
                    { new Guid("6d431414-abaa-4169-905b-c9e77814fbb3"), false, new Guid("5746858f-0700-4d70-bcb1-775e8adbd5d8"), "15" },
                    { new Guid("6ee8da0d-ad84-4c5f-9d3f-593356652be9"), false, new Guid("e1bb17dd-58fb-41bd-88d1-f07e0ea100d3"), "20" },
                    { new Guid("6ef86888-8d52-47f0-b686-16532b47220c"), false, new Guid("5746858f-0700-4d70-bcb1-775e8adbd5d8"), "20" },
                    { new Guid("6f378d8c-0727-4eb0-9efb-0d60c79f654a"), false, new Guid("5a94df5b-2046-41fc-978c-5426ff9bb66d"), "25" },
                    { new Guid("708e8f5d-cc42-4d6d-a867-124a3eaf96ed"), false, new Guid("728e68e6-859b-4cb5-979d-ccf545655cd1"), "20" },
                    { new Guid("73dfdc52-3f5e-44d0-b587-447d17120af3"), false, new Guid("99fa2ebb-6a85-48bd-8e43-996c02ef1807"), "25" },
                    { new Guid("744ff727-aa4d-4aaf-a1ef-77b1f72f01e8"), true, new Guid("850c63df-aede-49f4-8146-9ab4ecf7ea23"), "10" },
                    { new Guid("74ccda17-a322-4053-b592-d2b955e6a51d"), false, new Guid("fbe7c04a-e723-4800-9ff1-3d38be7e20eb"), "20" },
                    { new Guid("75765813-399c-4952-9fac-85622621738f"), true, new Guid("d0f522ca-e8f2-4a24-87ec-1d26cf88a51c"), "5" },
                    { new Guid("76662c66-38ad-471f-8513-737cab87e355"), false, new Guid("edcda0ad-8054-4db9-a6bb-10542b39ff12"), "25" },
                    { new Guid("77918510-4212-4346-a412-b1c2bc3f9090"), true, new Guid("e20e7005-e306-4900-ab25-d7087dcd79be"), "10" },
                    { new Guid("78956011-6020-4613-b844-a7ea59d38130"), false, new Guid("7ecbc820-07a1-4218-88a9-7d4ac7018796"), "15" },
                    { new Guid("7a46a233-f436-47f3-bc6e-cd0157a803a7"), false, new Guid("097512f4-d565-4dda-bb49-e47e640232c2"), "25" },
                    { new Guid("7b526cde-c5af-449f-b1b6-5e7294bccff3"), true, new Guid("a1b8cd94-1788-4f22-ab06-032d824fd707"), "10" },
                    { new Guid("7bf4f3fb-0c29-487f-bec1-f03f74fb8bc3"), false, new Guid("71e7615a-95e4-4033-85b2-1bfee6e88818"), "20" },
                    { new Guid("7c5de5eb-888e-4387-920b-409029c151b7"), true, new Guid("372f5dcc-6cd8-44ca-a218-89d16f7c35a2"), "10" },
                    { new Guid("7c6c3563-9358-4186-922f-836c8adc6dd3"), false, new Guid("94e860a0-52cb-430d-a455-a22f69638a34"), "20" },
                    { new Guid("7cbda163-5382-46a1-aa82-fe1a025ea48c"), false, new Guid("a1b8cd94-1788-4f22-ab06-032d824fd707"), "20" },
                    { new Guid("7d6dc306-792b-4b88-90e1-1bd0c00de961"), false, new Guid("14021bd0-5773-4559-a779-d3557519f43a"), "20" },
                    { new Guid("7e7976a1-9d52-4005-a9d0-67dedda56f3c"), true, new Guid("c6965812-c38b-403c-b5bc-b1fd553bef8f"), "10" },
                    { new Guid("7f8015db-9fb5-493e-a8e3-e43966f6e50f"), false, new Guid("850c63df-aede-49f4-8146-9ab4ecf7ea23"), "15" },
                    { new Guid("81900d10-c4bd-4f1d-a12b-8893a36d5fd6"), false, new Guid("e1bb17dd-58fb-41bd-88d1-f07e0ea100d3"), "15" },
                    { new Guid("83275f66-53ab-4292-859f-fb6d646d688b"), true, new Guid("889bef52-78fd-47fc-8181-fb1880dc676c"), "5" },
                    { new Guid("837c9a10-6524-41ad-baff-0ae8c8e8848b"), true, new Guid("8202c439-63e5-448d-a152-a1265b42d475"), "10" },
                    { new Guid("83d57671-0eb5-40f7-93fd-30bc47153c3f"), true, new Guid("31f510c9-de5f-48ce-94a1-31d796727686"), "5" },
                    { new Guid("83f9154e-9895-451d-9126-669cdd7abced"), true, new Guid("70235f7f-a493-4a55-9318-7c2c8ec6d252"), "5" },
                    { new Guid("8605e480-6313-403c-95f5-907dcc376489"), false, new Guid("f159a70a-9876-49ad-8ac4-5e79de21a9ff"), "25" },
                    { new Guid("86461d21-4526-4d64-85ac-9eda37196e97"), false, new Guid("03b03af3-bc5c-4ec6-a9a3-399709a1f8d6"), "15" },
                    { new Guid("87ae5895-691e-4bd0-8f91-62547e083d0c"), true, new Guid("d531e747-307f-4527-9bf1-a8c084d71d5a"), "5" },
                    { new Guid("8d85bd33-f771-47eb-a574-634228dbe69e"), false, new Guid("347fee97-06f8-4125-8c15-684e3a3147c8"), "20" },
                    { new Guid("8f59def9-034e-4314-8015-16b2d885a062"), false, new Guid("38e38aa2-943a-4bec-8b93-7ef8302530c3"), "15" },
                    { new Guid("9026ce16-73ab-4de6-85b6-c5e253cd05b0"), false, new Guid("66d7a06b-9481-495d-972f-5a38e2ced6b6"), "20" },
                    { new Guid("90cd5d15-01a7-49d4-a32e-30ff5a896b85"), true, new Guid("38e38aa2-943a-4bec-8b93-7ef8302530c3"), "5" },
                    { new Guid("90d71bac-1cd3-477f-bb4d-1f5a43c7c57f"), false, new Guid("372f5dcc-6cd8-44ca-a218-89d16f7c35a2"), "20" },
                    { new Guid("9476833c-34e0-4e47-972f-da50f4d291e3"), true, new Guid("d21045b8-b2a4-447f-a20f-7846f90eeb1e"), "10" },
                    { new Guid("9486620c-e813-4cbf-a480-e401bd74eaed"), false, new Guid("eb72845e-f7a2-4a99-b323-efb98228ff23"), "25" },
                    { new Guid("954f9367-f81a-4db5-90ce-e865901a4659"), false, new Guid("31f510c9-de5f-48ce-94a1-31d796727686"), "20" },
                    { new Guid("968267d8-4dd3-4935-b87e-6f81003603ba"), true, new Guid("25fe1413-ab52-4495-98a2-318753eb00b3"), "5" },
                    { new Guid("9781d5f0-8758-47c2-8042-c946a4b336e3"), false, new Guid("99fa2ebb-6a85-48bd-8e43-996c02ef1807"), "20" },
                    { new Guid("978f4669-817b-4276-90e8-df20e9512462"), false, new Guid("d531e747-307f-4527-9bf1-a8c084d71d5a"), "15" },
                    { new Guid("98955c09-040f-4f3b-bdd2-e2b5a54b960a"), false, new Guid("2ea5ae45-b5bf-4f33-88eb-4fb9600590f5"), "20" },
                    { new Guid("98b8bdcc-3da5-4cc9-8eca-43db4701a0a6"), true, new Guid("902db909-32dc-409e-8d85-57b7db2732c7"), "10" },
                    { new Guid("9a1802b3-455e-4724-8bdc-33bef51c7d54"), true, new Guid("fd29bd36-6aa9-4362-8365-d1f8ab2ab92e"), "10" },
                    { new Guid("9a6145f6-d7d2-4e5b-b499-72049237feb3"), true, new Guid("12d8992a-eea8-4059-9369-e7b9547104bf"), "10" },
                    { new Guid("9c2641f4-2164-40ec-87dd-d1dc88d5eb5b"), false, new Guid("e20e7005-e306-4900-ab25-d7087dcd79be"), "15" },
                    { new Guid("9dc6f7c4-0391-4d5a-97fb-a2b3e4c69c2e"), false, new Guid("12d8992a-eea8-4059-9369-e7b9547104bf"), "15" },
                    { new Guid("9e45597d-55f8-4d1a-b1e4-fef4dd9a8458"), false, new Guid("6acd0b36-a1cf-46e9-9034-10a90e98c0ce"), "20" },
                    { new Guid("9fb4775a-9c17-455f-96cd-f4036746e0fb"), false, new Guid("728e68e6-859b-4cb5-979d-ccf545655cd1"), "25" },
                    { new Guid("a05313d7-35c8-49b4-b215-b2ab6cd99c0c"), false, new Guid("a1f62901-23ea-4fac-8b0b-e91ca2087fc9"), "15" },
                    { new Guid("a102e6c8-71f7-4ea6-947d-a917e43bf508"), false, new Guid("12d8992a-eea8-4059-9369-e7b9547104bf"), "25" },
                    { new Guid("a15d3da3-5511-45ca-876a-bbcd6a87a623"), false, new Guid("fd29bd36-6aa9-4362-8365-d1f8ab2ab92e"), "20" },
                    { new Guid("a20d936f-7557-4db3-9696-b7ca6a7166da"), false, new Guid("fd29bd36-6aa9-4362-8365-d1f8ab2ab92e"), "25" },
                    { new Guid("a266a84b-42b0-4d2e-8a36-33e02d75ebb3"), false, new Guid("0cd03716-dc39-4812-aa16-b47ad3d16e4d"), "15" },
                    { new Guid("a2c903a6-398d-470a-b390-b7d06c6bdbc6"), false, new Guid("16467ca6-1f87-4a80-af2d-d9c681bdd9a2"), "20" },
                    { new Guid("a405b3e5-a548-4063-91db-0e10e6fb883a"), false, new Guid("3915ec2a-e3e3-4937-855c-3f256ce02dce"), "20" },
                    { new Guid("a5e1eb12-fb23-473d-9f64-da908171571b"), false, new Guid("1cba7e32-dd41-434d-ad53-214b13d80b45"), "15" },
                    { new Guid("a69e6833-b709-4a10-8eee-7797a6cb9978"), false, new Guid("17250bf3-823e-4382-9094-e76938618829"), "20" },
                    { new Guid("a91eb4dd-fd42-43ea-a6ec-36325e77f55f"), false, new Guid("c6a766e3-74ca-4100-9e66-174e6cc03032"), "15" },
                    { new Guid("a9607da2-8196-4808-ad70-fc04f28e52e1"), false, new Guid("9981b2e0-f21a-4cf1-adfa-ac72c582f78f"), "15" },
                    { new Guid("abdbb19c-5390-4b9a-b5b3-100479900860"), true, new Guid("fbe7c04a-e723-4800-9ff1-3d38be7e20eb"), "5" },
                    { new Guid("ac7fa26d-9e7c-4325-82f0-905dde7e2c66"), false, new Guid("9981b2e0-f21a-4cf1-adfa-ac72c582f78f"), "20" },
                    { new Guid("acc6d993-1d9d-451c-ace0-2deb886aef73"), true, new Guid("10ac36f9-8202-4edf-b1b4-de78d38881c6"), "10" },
                    { new Guid("ad4035ff-d7ef-4445-9fd1-6ffe87f8986b"), false, new Guid("d21045b8-b2a4-447f-a20f-7846f90eeb1e"), "20" },
                    { new Guid("add31b5b-50ba-4ac3-9c8f-7627fd5149c0"), false, new Guid("347fee97-06f8-4125-8c15-684e3a3147c8"), "25" },
                    { new Guid("ae104f5f-9707-47a4-99ac-9c21bfabf22e"), false, new Guid("81ae3beb-edd9-4964-bb78-d00c345b98fd"), "15" },
                    { new Guid("af51857f-cf10-4c02-8607-29c3b727ac26"), false, new Guid("25fe1413-ab52-4495-98a2-318753eb00b3"), "20" },
                    { new Guid("b07d72fc-817f-4b45-8cd9-ee7637d5567d"), false, new Guid("347fee97-06f8-4125-8c15-684e3a3147c8"), "15" },
                    { new Guid("b1a690e7-1eb6-4bf9-bf12-279b753ebb1a"), false, new Guid("5a94df5b-2046-41fc-978c-5426ff9bb66d"), "20" },
                    { new Guid("b2d9a2d1-fb19-4a8c-9d08-ff7eaa503435"), false, new Guid("902db909-32dc-409e-8d85-57b7db2732c7"), "20" },
                    { new Guid("b33b0a23-c471-4347-802b-d99a3b21e2e3"), false, new Guid("d21045b8-b2a4-447f-a20f-7846f90eeb1e"), "15" },
                    { new Guid("b34a3fe2-2728-4ee6-b040-e859b0f4fd57"), false, new Guid("16467ca6-1f87-4a80-af2d-d9c681bdd9a2"), "15" },
                    { new Guid("b3580c9d-63b5-4bf7-8d1f-663e5e9228f4"), true, new Guid("03b03af3-bc5c-4ec6-a9a3-399709a1f8d6"), "10" },
                    { new Guid("b4687ebd-017f-4202-bbbd-a549bf180223"), true, new Guid("9981b2e0-f21a-4cf1-adfa-ac72c582f78f"), "10" },
                    { new Guid("b4adddc1-ae3f-479c-a557-0bfeac30fac2"), false, new Guid("eb72845e-f7a2-4a99-b323-efb98228ff23"), "15" },
                    { new Guid("b70b0da3-529a-424b-8eaf-788e16c5c709"), false, new Guid("4bae6034-cc74-4249-8fad-1fa9ee99718e"), "15" },
                    { new Guid("b733663c-2b30-408c-b808-8f270e8e9a69"), false, new Guid("70235f7f-a493-4a55-9318-7c2c8ec6d252"), "20" },
                    { new Guid("b74e2551-8784-4316-8e02-91cafdc41aad"), false, new Guid("9981b2e0-f21a-4cf1-adfa-ac72c582f78f"), "25" },
                    { new Guid("b774dd69-391a-4174-b041-3b14f4f2dd1c"), false, new Guid("25fe1413-ab52-4495-98a2-318753eb00b3"), "25" },
                    { new Guid("b7b6f28d-5a64-499b-b77b-7c2ddb5bd9e5"), false, new Guid("a9d49ce6-03e3-4269-9a83-54213a272326"), "25" },
                    { new Guid("b88197ab-162f-464f-b9cc-99a18cff2140"), false, new Guid("d0f522ca-e8f2-4a24-87ec-1d26cf88a51c"), "20" },
                    { new Guid("b910380e-698f-4827-9c00-9a9a10a98e8a"), false, new Guid("7e1c6a98-bff4-440e-b1e2-9acbcdf8a554"), "15" },
                    { new Guid("b97340fa-4471-4963-bfdf-fce38b658d65"), false, new Guid("a1f62901-23ea-4fac-8b0b-e91ca2087fc9"), "25" },
                    { new Guid("bb4a60c7-3125-4ed0-b6a8-524e9b8e8572"), false, new Guid("70235f7f-a493-4a55-9318-7c2c8ec6d252"), "25" },
                    { new Guid("bc5730cb-6a17-402f-b2f9-9bbff113629e"), true, new Guid("14021bd0-5773-4559-a779-d3557519f43a"), "10" },
                    { new Guid("bd18ed66-e16b-4e0e-b35e-a085515a93cc"), false, new Guid("097512f4-d565-4dda-bb49-e47e640232c2"), "15" },
                    { new Guid("bd2bc004-2f23-4dcf-ae81-d61fa9b1b725"), false, new Guid("71e7615a-95e4-4033-85b2-1bfee6e88818"), "25" },
                    { new Guid("bdb52e9f-da03-45d5-b98a-59ff3e802d61"), true, new Guid("eb72845e-f7a2-4a99-b323-efb98228ff23"), "5" },
                    { new Guid("bf057b2d-aafb-4b33-8654-f9c2eaf4cca9"), false, new Guid("fbe7c04a-e723-4800-9ff1-3d38be7e20eb"), "25" },
                    { new Guid("c0761ab6-3ec4-451f-bc68-44b610fe7479"), false, new Guid("d1567857-4501-4f45-957e-82b4f7e66c6c"), "20" },
                    { new Guid("c1a8ab2f-322b-4ff0-b24d-4ccaf79aa863"), false, new Guid("7ecbc820-07a1-4218-88a9-7d4ac7018796"), "25" },
                    { new Guid("c326e535-1e43-4daa-b7b4-5320b2c28c6f"), false, new Guid("d0f522ca-e8f2-4a24-87ec-1d26cf88a51c"), "25" },
                    { new Guid("c404aba8-16c7-4cb1-aad0-354593df2869"), false, new Guid("7a8b2989-057e-4b37-acb8-231f4347f3cb"), "20" },
                    { new Guid("c464bb09-2071-4f4f-a837-9b30129513c1"), false, new Guid("889bef52-78fd-47fc-8181-fb1880dc676c"), "25" },
                    { new Guid("c5581bf3-5d26-4424-b025-a604e8d465c4"), true, new Guid("3b4e0829-6d53-4416-9caf-207fa2c6ddcb"), "5" },
                    { new Guid("c5c0616e-bb6c-4d1a-bf93-2b30ce352c39"), false, new Guid("7a8b2989-057e-4b37-acb8-231f4347f3cb"), "25" },
                    { new Guid("c6738c9d-dff5-4a9a-bdc3-6c23212720f5"), false, new Guid("d9fb34b7-7081-4675-9d85-1bd9a99a48b7"), "25" },
                    { new Guid("c69032e2-712d-48f0-abba-1afc21503424"), false, new Guid("f159a70a-9876-49ad-8ac4-5e79de21a9ff"), "20" },
                    { new Guid("c750f53d-f52d-4d60-ad20-ed0d1c702323"), true, new Guid("7ecbc820-07a1-4218-88a9-7d4ac7018796"), "10" },
                    { new Guid("c8b2fdba-3c39-4d53-bbee-db0da0fdc087"), false, new Guid("097512f4-d565-4dda-bb49-e47e640232c2"), "20" },
                    { new Guid("c8d1bf9f-8797-4502-8350-d4272f6eeb26"), false, new Guid("94e860a0-52cb-430d-a455-a22f69638a34"), "15" },
                    { new Guid("ca097a0c-1abf-4271-858f-cdc45d155476"), false, new Guid("3915ec2a-e3e3-4937-855c-3f256ce02dce"), "25" },
                    { new Guid("ca492e70-a9bf-4c3c-af83-7c053b8d3a6f"), false, new Guid("e20e7005-e306-4900-ab25-d7087dcd79be"), "20" },
                    { new Guid("cb1a8bf4-a36f-42b4-aa06-c9d730f81de2"), false, new Guid("a1b8cd94-1788-4f22-ab06-032d824fd707"), "15" },
                    { new Guid("cb2cceb7-fabb-4c40-bc92-296a4ff79f23"), false, new Guid("3b4e0829-6d53-4416-9caf-207fa2c6ddcb"), "20" },
                    { new Guid("cc1fbdb0-f76f-4e4b-89dd-b78b964679c4"), false, new Guid("6acd0b36-a1cf-46e9-9034-10a90e98c0ce"), "15" },
                    { new Guid("ccae4715-18d7-4a2f-9c42-d409729379ec"), false, new Guid("c6965812-c38b-403c-b5bc-b1fd553bef8f"), "25" },
                    { new Guid("cce1e0d3-c682-4003-9f31-c5c79ceb1702"), true, new Guid("d8463ab6-8483-492c-a227-54ccc93a8144"), "5" },
                    { new Guid("ce787c03-592c-4234-b942-f08b12112a76"), false, new Guid("4bae6034-cc74-4249-8fad-1fa9ee99718e"), "25" },
                    { new Guid("cefc56be-d948-4011-ae62-572c96445019"), false, new Guid("d0f522ca-e8f2-4a24-87ec-1d26cf88a51c"), "15" },
                    { new Guid("cf560077-3476-4a48-81f1-44ca75bb2391"), false, new Guid("d1567857-4501-4f45-957e-82b4f7e66c6c"), "15" },
                    { new Guid("cfd92d82-ccbf-4376-89ca-b39b3510fa34"), true, new Guid("17250bf3-823e-4382-9094-e76938618829"), "5" },
                    { new Guid("d14f4d57-6d1c-4918-bbce-ff2d70a3aea3"), false, new Guid("a1f62901-23ea-4fac-8b0b-e91ca2087fc9"), "20" },
                    { new Guid("d1b85b47-7d63-432f-ab4b-139ba314cf7c"), false, new Guid("31f510c9-de5f-48ce-94a1-31d796727686"), "25" },
                    { new Guid("d207a063-d019-4119-8d8a-213fbb94aa62"), false, new Guid("17250bf3-823e-4382-9094-e76938618829"), "15" },
                    { new Guid("d2d2d34e-48a5-4ed6-9088-71df80889996"), false, new Guid("839d2942-2745-47d3-9294-14868e689566"), "25" },
                    { new Guid("d353f857-2516-4aed-ba92-809f4b41f9f0"), false, new Guid("71e7615a-95e4-4033-85b2-1bfee6e88818"), "15" },
                    { new Guid("d3e6fb44-6f32-4f54-a9aa-18cc04d75e7d"), false, new Guid("d1567857-4501-4f45-957e-82b4f7e66c6c"), "25" },
                    { new Guid("d663727b-1a85-45be-9e18-e711e30806f1"), false, new Guid("0cd03716-dc39-4812-aa16-b47ad3d16e4d"), "25" },
                    { new Guid("d75d9143-8cc2-44fe-916d-7c8c5cbc9206"), false, new Guid("94e860a0-52cb-430d-a455-a22f69638a34"), "25" },
                    { new Guid("d9000570-6635-4276-82ea-21bd8a3cf18d"), false, new Guid("eb72845e-f7a2-4a99-b323-efb98228ff23"), "20" },
                    { new Guid("da3c7455-6155-40ee-a67a-ad2066a5610a"), false, new Guid("38e38aa2-943a-4bec-8b93-7ef8302530c3"), "20" },
                    { new Guid("da9cc5ce-18cd-4b50-94e6-edd9d80f2774"), false, new Guid("5a94df5b-2046-41fc-978c-5426ff9bb66d"), "15" },
                    { new Guid("dd915b9e-2bab-4621-9e54-78182ee48fe4"), false, new Guid("2ea5ae45-b5bf-4f33-88eb-4fb9600590f5"), "15" },
                    { new Guid("ddba4299-77ee-4e9b-ac4d-c707911eeb3d"), false, new Guid("fd29bd36-6aa9-4362-8365-d1f8ab2ab92e"), "15" },
                    { new Guid("de469d39-5433-4440-af69-563923bff126"), true, new Guid("2ea5ae45-b5bf-4f33-88eb-4fb9600590f5"), "5" },
                    { new Guid("e067fe39-18da-4069-90df-f8d5ef54f32e"), true, new Guid("a1f62901-23ea-4fac-8b0b-e91ca2087fc9"), "5" },
                    { new Guid("e0aeaf69-efb2-421d-a5a5-37d9f046d68c"), false, new Guid("902db909-32dc-409e-8d85-57b7db2732c7"), "25" },
                    { new Guid("e1333b4d-c66b-4227-a818-867ab126b95c"), false, new Guid("5746858f-0700-4d70-bcb1-775e8adbd5d8"), "25" },
                    { new Guid("e1957bbd-43f3-4980-b269-d8f72f1b010b"), true, new Guid("3915ec2a-e3e3-4937-855c-3f256ce02dce"), "10" },
                    { new Guid("e206b45e-1d1f-493f-9f07-4b9639fb3eed"), true, new Guid("f159a70a-9876-49ad-8ac4-5e79de21a9ff"), "10" },
                    { new Guid("e3316cef-090f-4b18-915d-f4ed62978500"), false, new Guid("850c63df-aede-49f4-8146-9ab4ecf7ea23"), "20" },
                    { new Guid("e488f8aa-c9d1-4886-96e4-850e2fa87a8c"), false, new Guid("81ae3beb-edd9-4964-bb78-d00c345b98fd"), "25" },
                    { new Guid("e5536a36-b595-463c-af20-cac04cdb186a"), false, new Guid("31f510c9-de5f-48ce-94a1-31d796727686"), "15" },
                    { new Guid("e69a213c-ad84-405e-b087-a60f14a4f50a"), true, new Guid("94e860a0-52cb-430d-a455-a22f69638a34"), "10" },
                    { new Guid("e6c6cfae-16aa-4f10-92d0-96296175c917"), true, new Guid("5746858f-0700-4d70-bcb1-775e8adbd5d8"), "5" },
                    { new Guid("e92e0f3b-59dd-4177-943f-3e820c86f101"), false, new Guid("728e68e6-859b-4cb5-979d-ccf545655cd1"), "15" },
                    { new Guid("ea2d8c7b-2a0f-4381-94f5-93337326ee07"), false, new Guid("14021bd0-5773-4559-a779-d3557519f43a"), "15" },
                    { new Guid("eb5f5713-e67d-4d7c-aac4-443755b68320"), true, new Guid("edcda0ad-8054-4db9-a6bb-10542b39ff12"), "10" },
                    { new Guid("ec3d6d31-ecb4-49d4-9d83-cc235657327c"), false, new Guid("81ae3beb-edd9-4964-bb78-d00c345b98fd"), "20" },
                    { new Guid("ef129f72-f9f0-4c68-8d66-093f9f292a44"), true, new Guid("8b8f6360-3bea-404c-99dc-45375e3df53b"), "10" },
                    { new Guid("f1d60229-c88a-484c-8cb2-ba18269d669f"), false, new Guid("25fe1413-ab52-4495-98a2-318753eb00b3"), "15" },
                    { new Guid("f35dd548-cc52-418b-a2c5-919c12299264"), true, new Guid("81ae3beb-edd9-4964-bb78-d00c345b98fd"), "10" },
                    { new Guid("f5054155-2f25-45f3-81d1-32a9bc64bbf3"), true, new Guid("7a8b2989-057e-4b37-acb8-231f4347f3cb"), "5" },
                    { new Guid("f6dd7d5c-522e-47c4-809f-d72dcf669a3d"), false, new Guid("889bef52-78fd-47fc-8181-fb1880dc676c"), "20" },
                    { new Guid("f865fff0-0065-4859-9567-51866999652d"), false, new Guid("1cba7e32-dd41-434d-ad53-214b13d80b45"), "20" },
                    { new Guid("f8acce17-770e-485f-a3f8-587b29bb668e"), false, new Guid("f1092470-f78c-4036-aaec-43288aa8b641"), "20" },
                    { new Guid("f9e90b79-3b24-4414-bd99-042293eec4bc"), true, new Guid("1cba7e32-dd41-434d-ad53-214b13d80b45"), "5" },
                    { new Guid("fa8327ba-82a4-44b8-a402-4fc78d988a7e"), false, new Guid("d531e747-307f-4527-9bf1-a8c084d71d5a"), "20" },
                    { new Guid("fd5b5195-8e45-40df-8422-9ebff0b22073"), false, new Guid("16467ca6-1f87-4a80-af2d-d9c681bdd9a2"), "25" },
                    { new Guid("fd6c48a9-8994-41ab-b19a-dbe513462b82"), true, new Guid("0cd03716-dc39-4812-aa16-b47ad3d16e4d"), "10" },
                    { new Guid("fd7bd984-cf0c-429f-bdcd-db5bae05d5a2"), false, new Guid("d8463ab6-8483-492c-a227-54ccc93a8144"), "15" },
                    { new Guid("fe4b5176-afae-477d-b900-cb2c01f17a62"), false, new Guid("10ac36f9-8202-4edf-b1b4-de78d38881c6"), "25" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
