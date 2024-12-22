using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Heros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceOffers",
                columns: table => new
                {
                    PriceOfferId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NewPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceOffers", x => x.PriceOfferId);
                    table.ForeignKey(
                        name: "FK_PriceOffers_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReviewText = table.Column<string>(type: "TEXT", nullable: false),
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookTags",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TagId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTags", x => new { x.BookId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BookTags_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Name" },
                values: new object[,]
                {
                    { new Guid("27242797-5280-4b98-868b-5da2d461d67f"), "Amy Nicolas" },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), "Cory Raynor" },
                    { new Guid("5b7d6d74-9105-4d02-bd8e-d33ca8dc684e"), "Berenice Rowe" },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), "Emely Champlin" },
                    { new Guid("78fb66ae-78ee-4937-86f4-f6c9f143dd24"), "Houston Pfannerstill" },
                    { new Guid("79bf9afb-25f4-456c-9d32-daa7e1e1e601"), "Vesta Wuckert" },
                    { new Guid("9bb0741c-9d12-4aae-9dc3-b6305e15372f"), "Garnet Blanda" },
                    { new Guid("9d4bb1d5-2bb8-42e7-9c31-675da455ee25"), "Kyleigh Feil" },
                    { new Guid("a9eba03e-af51-4331-a0be-d28c5443281d"), "Melyna Leuschke" },
                    { new Guid("beda3cb8-f89a-4d17-84bd-79c697dedab5"), "Ashlee King" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Title" },
                values: new object[,]
                {
                    { new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986"), "Dolores itaque nam." },
                    { new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), "Nobis iusto consectetur." },
                    { new Guid("1d958088-e117-4e7f-a32e-84e2680c6b42"), "Facilis dolorem temporibus." },
                    { new Guid("289bf20b-f67c-426a-880b-35f13313445a"), "Placeat vero aut." },
                    { new Guid("314681c9-15d3-4f25-b133-492277e5b658"), "Velit sit ut." },
                    { new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), "Qui explicabo ea." },
                    { new Guid("34999596-d404-422a-8e98-9459821a2feb"), "Qui sequi atque." },
                    { new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), "Eligendi doloremque nulla." },
                    { new Guid("6dfa710c-a5b7-4643-902a-392e471b9238"), "Eveniet ipsam odit." },
                    { new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3"), "Sit ratione eaque." },
                    { new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8"), "Molestiae vel ut." },
                    { new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), "Doloremque est qui." },
                    { new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), "Non quis cupiditate." },
                    { new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c"), "Asperiores consequatur et." },
                    { new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), "Magnam deleniti molestiae." },
                    { new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), "Repudiandae impedit consectetur." },
                    { new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3"), "Quasi est odio." },
                    { new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9"), "Ullam autem officiis." },
                    { new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "Iure nulla nobis." },
                    { new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), "Qui in nemo." }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "TagId", "Name" },
                values: new object[,]
                {
                    { new Guid("392a40c5-e2ce-4c0c-aac0-5f6fba473b66"), "Shoes" },
                    { new Guid("7d9ce0ce-c03b-4b96-8ba3-86f86b1fe069"), "Health" },
                    { new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a"), "Sports" },
                    { new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec"), "Computers" },
                    { new Guid("f48e58ba-1765-44cf-aeeb-70f180432994"), "Tools" }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("1581e381-a665-4750-9cef-af769e1ea7cd") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("1581e381-a665-4750-9cef-af769e1ea7cd") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("1d958088-e117-4e7f-a32e-84e2680c6b42") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("1d958088-e117-4e7f-a32e-84e2680c6b42") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("289bf20b-f67c-426a-880b-35f13313445a") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("289bf20b-f67c-426a-880b-35f13313445a") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("314681c9-15d3-4f25-b133-492277e5b658") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("314681c9-15d3-4f25-b133-492277e5b658") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("34999596-d404-422a-8e98-9459821a2feb") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("34999596-d404-422a-8e98-9459821a2feb") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("582d107b-28c9-4642-8a92-9d282a38cd32") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("582d107b-28c9-4642-8a92-9d282a38cd32") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("6dfa710c-a5b7-4643-902a-392e471b9238") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("6dfa710c-a5b7-4643-902a-392e471b9238") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("b385f057-66e7-4ae6-b941-27b6880d269a") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("b385f057-66e7-4ae6-b941-27b6880d269a") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e") },
                    { new Guid("4c4564df-43c5-4961-8d96-5f9415e92b04"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478") },
                    { new Guid("766a932d-c2ec-4e7f-b451-8fe525365f72"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478") }
                });

            migrationBuilder.InsertData(
                table: "BookTags",
                columns: new[] { "BookId", "TagId" },
                values: new object[,]
                {
                    { new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("1d958088-e117-4e7f-a32e-84e2680c6b42"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("1d958088-e117-4e7f-a32e-84e2680c6b42"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("289bf20b-f67c-426a-880b-35f13313445a"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("289bf20b-f67c-426a-880b-35f13313445a"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("314681c9-15d3-4f25-b133-492277e5b658"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("314681c9-15d3-4f25-b133-492277e5b658"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("34999596-d404-422a-8e98-9459821a2feb"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("34999596-d404-422a-8e98-9459821a2feb"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("6dfa710c-a5b7-4643-902a-392e471b9238"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("6dfa710c-a5b7-4643-902a-392e471b9238"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") },
                    { new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), new Guid("e80e41d0-726c-4bed-9321-534e535f4f4a") },
                    { new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), new Guid("ee81f3ce-8d45-4514-8b03-a0d43a3b55ec") }
                });

            migrationBuilder.InsertData(
                table: "PriceOffers",
                columns: new[] { "PriceOfferId", "BookId", "NewPrice" },
                values: new object[,]
                {
                    { new Guid("0ab8a020-cc70-4d5b-9818-3f59d404cde9"), new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), 71.79m },
                    { new Guid("0d26323d-76db-4344-9065-0c38f6472243"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), 97.01m },
                    { new Guid("20d48357-1b94-42d3-9e18-d19022e2edd3"), new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), 69.05m },
                    { new Guid("384543f6-c515-4d9a-8014-f4513a625c11"), new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986"), 10.92m },
                    { new Guid("3bd7348c-6224-4ca1-9c99-d5a4a8cffe99"), new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3"), 47.55m },
                    { new Guid("56a1ba81-55f9-406a-bbc2-d97478b93747"), new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), 62.55m },
                    { new Guid("56eb4835-5798-45a4-b024-ae68926041aa"), new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c"), 70.82m },
                    { new Guid("5b0f8194-fa5b-4199-825f-7e2cf1aac582"), new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9"), 15.30m },
                    { new Guid("6183e94f-3dc3-408c-a01f-17c281ba58cf"), new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), 40.90m },
                    { new Guid("6f237de4-65f3-4ac4-bb37-75c9c72a5bdb"), new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), 60.21m },
                    { new Guid("7e1a3559-fc13-4391-98a0-e9478060af1c"), new Guid("6dfa710c-a5b7-4643-902a-392e471b9238"), 51.03m },
                    { new Guid("95e4a007-04b0-46a7-ae3d-f3c98cf65ed4"), new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3"), 10.28m },
                    { new Guid("96d2ebde-48a8-4846-b4e5-851f2f6eef1d"), new Guid("34999596-d404-422a-8e98-9459821a2feb"), 21.07m },
                    { new Guid("cea1ad80-f741-4955-aec5-ebabe986d703"), new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8"), 80.20m },
                    { new Guid("ee5cf4a2-ee6b-4369-afb3-ff83bb655d17"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), 96.31m }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookId", "ReviewText" },
                values: new object[,]
                {
                    { new Guid("007d1a00-2a33-4c42-aca8-8492c9d31605"), new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), "Animi reprehenderit ea sapiente deleniti sint laudantium. Illum nam autem rerum facilis et et. Ipsum esse dolor nihil minus. Sequi consequatur ut ex illo corporis consequatur. Et possimus ex qui consequatur officiis et. Harum iusto sed qui." },
                    { new Guid("0358b572-bcad-40e5-81a8-4e65622fb3b4"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), "Sed ea nam commodi nulla tempora aut. Sed dolore est ab et corrupti minima nemo. Totam voluptas quia consequatur perferendis saepe architecto ut. Suscipit id libero aut nisi ut autem laudantium. Architecto pariatur reiciendis. Ex architecto sint aut aliquid nihil dolor et assumenda deserunt." },
                    { new Guid("0d04f8e0-22b7-4b4f-b99a-d8001b4a138e"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "In ex ipsa qui voluptatem iure quibusdam voluptatem rem hic. Porro maxime ut. Soluta ab quos." },
                    { new Guid("16e79f72-eb67-47ba-b755-3cb12e00d113"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "Dolores nam ratione aut in. Distinctio quidem consequatur quis excepturi quos explicabo sit. Minus eligendi voluptates enim ducimus dolor possimus fugit quibusdam eligendi. Error dicta et qui et odio corrupti enim. Voluptatem omnis nihil sit iusto aut nesciunt." },
                    { new Guid("1af21959-f26a-40fe-ba7c-e0f2c2566226"), new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), "Eos aut id ex sint. Sapiente consequuntur adipisci nulla enim nisi qui. Consequatur velit ad optio non expedita dignissimos ut ea. Molestias minima libero quo earum est voluptate impedit exercitationem. Et ipsa iste libero ut nisi tenetur rerum. Iure molestias voluptatem sed iusto." },
                    { new Guid("2a649082-b605-4989-86f7-def884345964"), new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), "Velit odit dolorem placeat in fugit est voluptatem maxime et. Qui voluptatem deleniti consequatur mollitia tempore aut ut. Eligendi amet sit consectetur. Modi nisi itaque libero id quae harum." },
                    { new Guid("2b61b9a4-d7c6-4bc6-b8e1-6639bbe9b7e1"), new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), "Quo aliquam quia vel. Nisi quis ea. Ea vel mollitia. Sint aspernatur iusto sunt velit praesentium quia iste. Sit et eos exercitationem alias odit voluptates dolores sint voluptas." },
                    { new Guid("30f2d868-e0ac-4a7a-b93d-e7a7a3e9ddf5"), new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3"), "Possimus eligendi dolorem. Quis modi voluptas. Molestiae consectetur eaque et ducimus vel praesentium. Quidem sit quia voluptatum suscipit perspiciatis placeat architecto. Sed culpa et dolorem enim architecto non. Inventore atque incidunt repellat voluptatum ipsam voluptas ut et voluptatum." },
                    { new Guid("33cd6cf6-af9d-4203-ba0c-c05ffa3268a9"), new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), "Quidem consequatur ipsa veritatis culpa dolorem sed fuga. Id hic cum et illum aut laborum est veritatis. Nesciunt sit nisi quibusdam accusantium mollitia suscipit voluptatibus neque mollitia." },
                    { new Guid("3694a085-beb6-4088-a927-0aefcfd8f3e1"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), "Accusantium enim quod aut tempore. Saepe doloribus necessitatibus reiciendis autem consequatur. Error odit nisi voluptatem iure exercitationem. Natus rem aliquid repellat qui in officia quo. Architecto sint beatae doloribus. Possimus ut et officia." },
                    { new Guid("385a2c02-ce3d-4e85-afac-f55abca95bef"), new Guid("314681c9-15d3-4f25-b133-492277e5b658"), "Quam non maiores sed pariatur ut qui enim vel esse. In nostrum rerum voluptatibus qui. In harum quos fugit sed reprehenderit alias sit laudantium dicta." },
                    { new Guid("3adae02f-f0d5-41ca-90f8-c08891e9baf0"), new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), "Aperiam quibusdam quis voluptatem. Ut eius quasi doloribus. Sunt et vitae modi." },
                    { new Guid("3b05203d-77b5-451f-8e30-5d402e49c085"), new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8"), "Ipsam praesentium dolorum provident natus officia consequatur. Aliquid id recusandae. Et corrupti perferendis architecto laudantium. Neque occaecati nisi quia quam." },
                    { new Guid("510a873a-caa4-4040-aef2-4502ec804cdc"), new Guid("314681c9-15d3-4f25-b133-492277e5b658"), "Dolorum quibusdam natus placeat minus vel sint aliquid. Omnis laborum asperiores quis aperiam distinctio corporis dolores. Ipsa voluptas consectetur neque rem. Voluptatem quis at accusamus doloremque est quod. Ad rem quasi." },
                    { new Guid("53ba88b0-5351-4549-a59e-d867746a079e"), new Guid("6dfa710c-a5b7-4643-902a-392e471b9238"), "Quis praesentium cupiditate alias recusandae praesentium. Voluptates optio et. Consectetur dignissimos omnis quia labore voluptas culpa." },
                    { new Guid("543dfe8d-914d-4c4f-b263-d8b4d58114b5"), new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), "Quia repellat reiciendis et. Quibusdam officia vel quia rem voluptatibus et quia facere. Consectetur totam est temporibus rem voluptates in iure rerum veniam. Voluptatem necessitatibus ea occaecati modi. Consequuntur est laborum exercitationem suscipit. Sequi deserunt fuga rerum est excepturi occaecati." },
                    { new Guid("55e8ee1a-06b8-402e-b29a-60f14d979b79"), new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), "Repellat voluptas dolores. Eveniet est fugiat enim est sed magnam corporis dolores at. Tempora sit deserunt. Aut reprehenderit dolor quidem. Pariatur debitis pariatur. Consequuntur necessitatibus quo quia autem dignissimos." },
                    { new Guid("62c1ed97-9975-4d06-8ef6-22db05704cec"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "Doloribus voluptates aperiam consectetur error accusamus autem doloremque culpa. Aliquam quasi nam nesciunt numquam commodi aliquam non ut at. Eos laudantium natus sunt a. Laborum modi ex. Corrupti esse commodi repellat voluptas fugit aut accusamus. Ab eos et ducimus et quos velit debitis." },
                    { new Guid("64e10d38-dbcb-45ee-938b-5bcac1675bcd"), new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9"), "Qui maiores placeat. Suscipit eius aperiam recusandae consequatur provident autem velit et. Nihil qui nisi ex laudantium neque ex." },
                    { new Guid("68bb3eb9-32c9-41cc-84a9-6c6640cd8649"), new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), "Quos qui ipsam distinctio qui voluptates laborum consectetur et magni. Est voluptatem libero voluptate nobis mollitia laborum tempora est. Repudiandae et rerum." },
                    { new Guid("69c41120-8ab7-48d8-bc8f-3dffe9c67868"), new Guid("a9836a73-63d9-4167-bd62-8a0a5f24ea38"), "Fugiat dolor ratione esse id consequuntur alias ut. Rem amet quo aut quis nihil tempore perferendis sit perspiciatis. Aut eos sunt sed magni aut possimus. Autem sequi neque soluta est adipisci. Quo et eum omnis et. Incidunt consequuntur est officia quia fugit laborum molestias similique omnis." },
                    { new Guid("6e6b2f3b-3e38-4071-8a39-b29ba4c2ef3f"), new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), "Aliquid magni inventore quasi et ea sint. Fugiat error enim autem ea commodi fuga. Repellendus voluptatem ut perferendis molestias voluptas perspiciatis nobis illum. Veritatis consequatur voluptatibus adipisci nobis velit ut necessitatibus dolorem et. Nihil est voluptas quia nobis." },
                    { new Guid("6efafc16-79c1-4f9b-9cca-c878d723ed45"), new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986"), "Veniam delectus eius accusamus maxime id molestiae. Recusandae impedit assumenda ipsam sed. Mollitia explicabo aut rem voluptas aspernatur." },
                    { new Guid("7f0a8a51-90d2-4c4d-a3d2-1f0dee097114"), new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), "Illum ipsum quia. Atque culpa voluptatum. Sed ratione provident dolorem aut corporis accusantium ut dolor aut." },
                    { new Guid("80dab418-9492-467f-ad1e-e8ccf284c6be"), new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c"), "Et nisi quo ea fugit omnis et aperiam ab. Officiis et pariatur ratione inventore molestiae id delectus velit. Repellendus animi ut dolore qui porro et beatae. Tenetur tempora expedita ut unde enim. Minima magni in placeat aliquam illum et voluptatibus et non." },
                    { new Guid("878de66a-dc0f-4336-92fb-f396e12e668c"), new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), "Omnis iusto fugit reiciendis autem aut vitae. Magni porro sequi nihil nesciunt. Natus magni dolorum laudantium est aut laudantium ratione quia. Odio repudiandae architecto totam est numquam." },
                    { new Guid("8818725f-3dbc-4c5b-ac32-cd402661aa1f"), new Guid("6dfa710c-a5b7-4643-902a-392e471b9238"), "Vel sit fugit inventore quos deleniti quaerat minus et quos. Et autem et aut quia odit maxime dolorum perspiciatis. Ducimus nisi ut dolores quia doloremque. Sunt officiis id voluptas et similique et omnis ratione. Architecto debitis et consequatur blanditiis quo pariatur." },
                    { new Guid("8c534757-0f8e-4e86-ba81-ece30f71678d"), new Guid("98cf276e-e728-4aa9-b2e6-e791df2dece8"), "Et praesentium qui. Esse at tempora aut sunt ut ipsam. Animi maxime consequatur ipsam sequi perspiciatis natus. Qui voluptatibus voluptatem aut enim alias hic magni velit. Aut culpa porro nihil eveniet." },
                    { new Guid("8dcd20e9-6fb3-4767-b52e-0a73cbe6e145"), new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), "Pariatur autem molestiae molestiae repellat. Et quo magni sint sunt tenetur. Ab aperiam omnis modi explicabo. Excepturi distinctio earum autem harum ipsum et quia. Libero voluptatum harum earum et sequi occaecati." },
                    { new Guid("8e9452cb-7037-47e0-95ea-13330458a4fa"), new Guid("ae1f715b-42b9-423d-bf94-59eea5af556c"), "Voluptatibus consequatur rem. Voluptatem quidem distinctio. Amet atque necessitatibus numquam alias inventore omnis est." },
                    { new Guid("90484440-48f5-40dd-96b7-71fd17a67597"), new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), "Aut ut nam. Est quis quod mollitia sapiente quaerat dignissimos porro fugiat et. Deserunt adipisci adipisci animi eius adipisci. Officia accusamus minus esse id illo facere recusandae quis delectus. Error modi et sequi. Nihil provident qui voluptas ex qui voluptate expedita consequatur velit." },
                    { new Guid("95d36255-13f0-4ed0-9d82-f4c2e2ef4a2b"), new Guid("7661b60d-ffc7-4114-9ff3-b185e0ba9eb3"), "A ea sed laboriosam perferendis qui sit aut aliquam id. Vitae eaque officia sunt quia ducimus harum qui. Eaque saepe sunt sed id qui. Rerum ratione temporibus fugiat aut impedit. Delectus reprehenderit eaque sit ullam. Laborum laborum autem eum." },
                    { new Guid("9edae360-b8c7-4ab3-a620-7ac9beea9f5b"), new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), "Tenetur temporibus nam sequi nostrum minima deserunt et deleniti ipsa. Eveniet illo aut. Minus ea laudantium debitis excepturi." },
                    { new Guid("a1b4f2e7-cb6a-4e60-8068-1a35e13f138e"), new Guid("582d107b-28c9-4642-8a92-9d282a38cd32"), "Ea porro facere aut molestiae similique repellendus explicabo velit. Eos aspernatur ab velit in aut fugiat. Quia velit odit numquam temporibus assumenda magni iste. Repellendus dignissimos beatae sint non sapiente pariatur qui." },
                    { new Guid("a9054ca0-6935-4f5e-b255-e1fc691ef6bd"), new Guid("b385f057-66e7-4ae6-b941-27b6880d269a"), "Nesciunt facere eum dolore animi est. Est odit voluptate adipisci aut reiciendis sunt facere ut. Inventore et cupiditate et odit. Quia animi voluptas velit ipsam. Sit laborum est sint. Architecto quia dolor minus est voluptatem earum." },
                    { new Guid("ac1c7ac6-44ba-4acd-bb02-4418f8054440"), new Guid("314681c9-15d3-4f25-b133-492277e5b658"), "Aut amet sed qui labore consequatur. Sit vel quo. Reprehenderit in mollitia nesciunt maiores perferendis voluptatem. Non voluptas itaque quia velit dolores." },
                    { new Guid("acf0a926-5685-4bee-b156-2c8cad27ac02"), new Guid("1d958088-e117-4e7f-a32e-84e2680c6b42"), "Laudantium optio ut consequatur omnis. Nesciunt consequatur aut maiores sunt. Quis dolore fugiat dolores doloremque animi maiores numquam. Neque id dolor nesciunt temporibus culpa deleniti iste non non. Enim sed error dolores quia iure et reprehenderit ut vel. Quam impedit et exercitationem dolore eligendi provident eos accusantium." },
                    { new Guid("ad3f2750-14fa-47c4-9a89-efef4ff72561"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "Dolore ut doloremque ut eligendi quia pariatur vitae. Molestias et mollitia impedit totam error illum placeat autem. Totam cupiditate reprehenderit neque provident eum reprehenderit. Porro et ipsum repellendus repellendus eum aut." },
                    { new Guid("b6e5e7a0-f2c0-4ed6-bc02-cdcf43e3680a"), new Guid("efcf7a24-710b-4e75-b1e3-668c8bc9a2e9"), "Labore illo dolor velit occaecati omnis exercitationem molestiae. Et qui nostrum eligendi est accusamus dolores. Minima magni sit laborum officiis illum vitae debitis. Et animi aut." },
                    { new Guid("c7baaa71-8d25-4994-b7ef-a1315cd776a9"), new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), "Sint minima cumque nemo repudiandae nisi. Molestiae sit perspiciatis molestiae. Quis qui ut commodi et repellendus sit. Voluptatem consectetur dolore nemo voluptas placeat iure dicta quia." },
                    { new Guid("c924af72-6462-4c61-a46d-a954257a5f30"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), "Laudantium qui mollitia illum quo magnam hic repellat aut. Illo velit sunt maxime voluptate quia. Sit excepturi vero tempore facilis necessitatibus molestias molestiae. Ut et impedit numquam voluptas eum autem voluptatum voluptas. Sunt nulla deleniti natus." },
                    { new Guid("cc731914-b3b3-4543-b242-36499cb1225a"), new Guid("f687c768-f595-4c37-92fd-7a4e8d6db478"), "Et sed distinctio sequi soluta qui commodi esse sit. Nostrum deserunt aut eos rerum molestiae voluptatem. Ducimus sit ut dolorem quia debitis vitae." },
                    { new Guid("dbd34c98-7779-47ce-a613-0b5373f3a7f2"), new Guid("c2c01868-48a9-464a-8025-6dc7d13606a3"), "Eveniet et dicta at esse aut fugit voluptas. Autem omnis et et eos dicta enim mollitia accusamus dolorum. Amet quia nostrum ad qui et placeat." },
                    { new Guid("dfd02e1f-eea4-40eb-9876-f7f3c7fa4049"), new Guid("ad4d5d5c-9337-45aa-a98a-8cf38aa7ac8e"), "Omnis eum ea et aut laborum non. Commodi eum molestiae dolor aut aspernatur. Repellendus voluptas repudiandae. Voluptatem provident et consequatur et quo rerum consequatur nulla qui. Provident ea et autem quos. Alias cum voluptate consequatur et." },
                    { new Guid("e3b9189a-eeac-4190-9934-4560d106e7f0"), new Guid("3222d8aa-a9cd-4bbe-841a-ffd0beedfa6f"), "Deleniti rerum doloremque sunt quidem et repudiandae ipsam voluptatem assumenda. Dolor ratione nam unde distinctio. Et in a quas." },
                    { new Guid("e472925a-6517-4288-a036-3efe2b4cdeee"), new Guid("1581e381-a665-4750-9cef-af769e1ea7cd"), "Neque ipsum tempora qui neque debitis. Voluptas perspiciatis sequi dolores nesciunt quia. Recusandae illo necessitatibus nihil voluptas." },
                    { new Guid("ebd3a687-9648-44ef-9032-4b13cb159d6a"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "Officia saepe voluptatum facilis. Error sequi dolore. Nam cumque repudiandae voluptatibus quis nemo a repellendus autem. Et ducimus nostrum accusantium id velit id." },
                    { new Guid("f16a9cae-62cc-4c45-af9b-29e6125bc46d"), new Guid("c106eebd-2b6e-482e-b890-adfa8ab78d85"), "Eligendi voluptates non ducimus incidunt. Earum in iure. Sint consequatur quod possimus quia deleniti est. Nihil rerum eligendi deserunt. Commodi at quia." },
                    { new Guid("f7d14686-5dea-4377-9ffb-14faca0e719f"), new Guid("f1165cb9-0433-4352-b9b8-5edd714e0f2e"), "Eligendi eos consequatur autem. Aut dolore sapiente aut est veritatis et sunt perspiciatis adipisci. Omnis culpa unde dolor ex." },
                    { new Guid("fab2c9fd-819c-46eb-8a59-41e4715b6887"), new Guid("092c5f0b-85f1-4eaf-9053-1f9d5fee2986"), "Itaque qui et eaque doloribus dolorem. Enim sint pariatur ipsa. Molestiae architecto aut veniam molestiae esse est ab adipisci molestiae. Et omnis earum ipsam quidem et enim voluptas perspiciatis." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTags_TagId",
                table: "BookTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceOffers_BookId",
                table: "PriceOffers",
                column: "BookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookTags");

            migrationBuilder.DropTable(
                name: "Heros");

            migrationBuilder.DropTable(
                name: "PriceOffers");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
