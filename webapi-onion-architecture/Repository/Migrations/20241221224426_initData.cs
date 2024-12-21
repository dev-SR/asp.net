using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class initData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "44089 Bogan Junctions, Jakaylaborough, Dominica", "Cyprus", "Dibbert LLC" },
                    { new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"), "7772 Kamron Parkways, Breitenberghaven, Lesotho", "Liechtenstein", "Casper Inc" },
                    { new Guid("47680e25-cda2-4e85-919e-7d723d3dfcc2"), "2677 Osinski Meadow, West Adriel, Equatorial Guinea", "Saint Helena", "Halvorson, Jenkins and Gibson" },
                    { new Guid("4904ae42-f898-4bac-b795-645fd90519cc"), "4085 Little Rest, New Doriantown, Guadeloupe", "Guadeloupe", "Heaney, Ondricka and Pouros" },
                    { new Guid("617feaa2-08b0-4433-9de0-88983525e371"), "942 Krystal Villages, West Rosemarychester, Comoros", "Cote d'Ivoire", "Ziemann, Predovic and West" },
                    { new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "381 Goldner Circles, Port Alessandra, Cyprus", "Greece", "Konopelski and Sons" },
                    { new Guid("7fdd53f7-34aa-4093-8ff6-ad2fb157cd22"), "522 Goldner Pike, East Laurianetown, Congo", "Ukraine", "Kihn - Haag" },
                    { new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "2616 Wuckert Extension, Raquelview, Indonesia", "France", "Gaylord Inc" },
                    { new Guid("d5f6e332-c6e0-4c59-a148-6372b5599ae6"), "61437 Casandra Creek, Lake Magali, Saint Vincent and the Grenadines", "Trinidad and Tobago", "Ortiz and Sons" },
                    { new Guid("dff2b225-e75a-48cf-8a06-19e89f25d140"), "677 Swift Shore, Sanfordshire, Slovakia (Slovak Republic)", "Gabon", "Mayer Group" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("0d9df22a-4087-42c1-8df6-04e69c8a9e0f"), 55, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Miracle Hand", "District Optimization Coordinator" },
                    { new Guid("134f255a-e7b8-49f4-ab0c-b6d7c6cbe4ec"), 56, new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "Dusty Muller", "Dynamic Accountability Architect" },
                    { new Guid("1afb9570-9431-4104-8592-16cc6069f821"), 54, new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "Anabelle Monahan", "Lead Infrastructure Specialist" },
                    { new Guid("1e08e9c0-77b7-4b35-9c91-41d06276a080"), 50, new Guid("617feaa2-08b0-4433-9de0-88983525e371"), "Brandt Gutmann", "Lead Solutions Architect" },
                    { new Guid("20e3d58e-6e3d-4dbc-b28a-108b1f515fc6"), 29, new Guid("4904ae42-f898-4bac-b795-645fd90519cc"), "Devonte Ankunding", "Legacy Metrics Consultant" },
                    { new Guid("256d64f6-6153-4968-8f8c-02fe5facb496"), 59, new Guid("dff2b225-e75a-48cf-8a06-19e89f25d140"), "Lawson Medhurst", "Dynamic Mobility Administrator" },
                    { new Guid("2641473c-a1a8-415a-8228-a76e370b211b"), 55, new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "Alva Vandervort", "Legacy Optimization Designer" },
                    { new Guid("2aa8952b-91c0-44f5-9e5d-8209ea3515db"), 31, new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "Cameron Rolfson", "Senior Implementation Facilitator" },
                    { new Guid("2b1fb753-e0e4-4ad4-b398-60833d50a38c"), 29, new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"), "Bernita Kuhlman", "Central Quality Engineer" },
                    { new Guid("326c0964-8e59-45a4-a7a1-24e5c6c13e46"), 43, new Guid("d5f6e332-c6e0-4c59-a148-6372b5599ae6"), "Craig Rodriguez", "Corporate Quality Strategist" },
                    { new Guid("33c870dc-41eb-4d15-8c25-97a572b3df32"), 59, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Gillian Ruecker", "Chief Creative Coordinator" },
                    { new Guid("3b2a2647-bc9c-4eb3-a3f3-277edd4a1a22"), 47, new Guid("47680e25-cda2-4e85-919e-7d723d3dfcc2"), "Porter Miller", "Dynamic Security Supervisor" },
                    { new Guid("40ae07b4-3b59-4812-8f67-3243a10b8f5e"), 59, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Torrance Kutch", "Future Response Planner" },
                    { new Guid("485d7512-8f42-404c-98c5-cf444660160f"), 26, new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"), "Jett Marvin", "Central Program Liaison" },
                    { new Guid("4dc1aa2c-6f0f-47d1-ab90-c8f48df02fb5"), 56, new Guid("47680e25-cda2-4e85-919e-7d723d3dfcc2"), "Ana Lang", "Product Program Supervisor" },
                    { new Guid("51c4a842-ab27-4992-ab72-5b001476b271"), 23, new Guid("4904ae42-f898-4bac-b795-645fd90519cc"), "Zachariah Stokes", "International Data Architect" },
                    { new Guid("56a547bd-962c-459e-92c5-7f21b9622d8e"), 41, new Guid("617feaa2-08b0-4433-9de0-88983525e371"), "Keith Gibson", "Chief Intranet Engineer" },
                    { new Guid("58e021f0-9aab-453c-8ecd-6c319caa5d4f"), 40, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Marcus Mohr", "Product Solutions Director" },
                    { new Guid("58fd7707-df28-4e24-b867-1d7761679632"), 27, new Guid("47680e25-cda2-4e85-919e-7d723d3dfcc2"), "Elvis Schroeder", "Corporate Division Facilitator" },
                    { new Guid("648d1566-4f4e-467e-a672-afbb68dd9985"), 25, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Carolanne Farrell", "Central Integration Engineer" },
                    { new Guid("6551a838-3ed9-45c5-b372-33ce27d6ef40"), 24, new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "Denis Mertz", "Forward Web Liaison" },
                    { new Guid("6b90df1e-13a7-4669-8675-fc47e96d5a18"), 25, new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "Priscilla Rempel", "Chief Quality Developer" },
                    { new Guid("6c4d4728-065e-4127-953a-9c669ad9b183"), 56, new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "Alice Rohan", "Corporate Implementation Executive" },
                    { new Guid("6ebba1c1-5d8b-4a5c-9a7f-780eaeaaf37b"), 47, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Elouise Sanford", "District Configuration Planner" },
                    { new Guid("7b49411c-18d7-476e-b178-346582c7e133"), 60, new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "Harmon Oberbrunner", "Central Operations Supervisor" },
                    { new Guid("7e4e69ac-8bb0-40ca-b7d3-c84db4e6f0ec"), 41, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Anibal Glover", "Principal Assurance Coordinator" },
                    { new Guid("8068c55a-095c-4351-bc37-d40eaec0e76f"), 50, new Guid("4904ae42-f898-4bac-b795-645fd90519cc"), "Marley Funk", "Internal Directives Manager" },
                    { new Guid("82ed97fa-7cfa-4f31-8de7-95d8049bd746"), 35, new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"), "Travon O'Reilly", "International Markets Designer" },
                    { new Guid("8923603d-1238-4c32-afcd-a0ca35382c2e"), 35, new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "Sarai Roberts", "District Paradigm Liaison" },
                    { new Guid("89c3c0f2-b20e-4d72-939e-d3cb4abcb5ff"), 38, new Guid("d5f6e332-c6e0-4c59-a148-6372b5599ae6"), "Ladarius Daugherty", "International Operations Developer" },
                    { new Guid("8c87fb67-1c1c-413a-a528-c12302348587"), 25, new Guid("d5f6e332-c6e0-4c59-a148-6372b5599ae6"), "Simone Swift", "National Configuration Specialist" },
                    { new Guid("8fabcfe5-abfc-4760-84f4-73faea003d1e"), 55, new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "Roberta Corwin", "Global Division Engineer" },
                    { new Guid("96b71d80-f99f-49bb-9b72-d963e1e3fa13"), 57, new Guid("d5f6e332-c6e0-4c59-a148-6372b5599ae6"), "Joannie Zemlak", "Future Intranet Facilitator" },
                    { new Guid("a799933d-beb9-476d-a545-1c7dd9c3603e"), 27, new Guid("4904ae42-f898-4bac-b795-645fd90519cc"), "Lucinda Barton", "Dynamic Paradigm Analyst" },
                    { new Guid("b21e7c78-29eb-42f2-b4ee-846c76da6fa1"), 28, new Guid("7fdd53f7-34aa-4093-8ff6-ad2fb157cd22"), "Emerson Barrows", "Senior Security Agent" },
                    { new Guid("b6e77138-a8a0-402c-842c-7c42f08fecae"), 31, new Guid("dff2b225-e75a-48cf-8a06-19e89f25d140"), "Sheila Ferry", "Central Accountability Planner" },
                    { new Guid("bb90070a-d48d-4ec5-a5b4-b0418b63299c"), 22, new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"), "Bryana Shields", "Lead Creative Engineer" },
                    { new Guid("d0157999-7376-4583-9c74-4416262432a0"), 41, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Penelope Beatty", "Product Operations Assistant" },
                    { new Guid("d2702671-cc66-42f9-910d-c2497187d645"), 35, new Guid("617feaa2-08b0-4433-9de0-88983525e371"), "Raymond Kris", "Senior Interactions Officer" },
                    { new Guid("d34cc149-a417-486f-b625-3b668a99cc6c"), 53, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Bobby Rohan", "Customer Solutions Administrator" },
                    { new Guid("d3b4f8d2-0ff2-466a-88b5-886b494f317a"), 24, new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"), "Delaney Mohr", "Legacy Factors Executive" },
                    { new Guid("d91466cd-9700-4b74-8ac1-6ef2ce42e38c"), 55, new Guid("47680e25-cda2-4e85-919e-7d723d3dfcc2"), "Frieda Lakin", "Principal Quality Developer" },
                    { new Guid("db1d27e0-1bde-4d8e-be7d-8c6d8b3562fd"), 22, new Guid("dff2b225-e75a-48cf-8a06-19e89f25d140"), "Uriel Turcotte", "Product Identity Analyst" },
                    { new Guid("de59d77b-7bff-496f-baf0-db28d55e5b44"), 28, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Monty Dickens", "Regional Markets Associate" },
                    { new Guid("e366b5b8-2ec3-471d-a5c2-8020849b1c49"), 32, new Guid("617feaa2-08b0-4433-9de0-88983525e371"), "Karlie Schiller", "Central Functionality Administrator" },
                    { new Guid("e48749df-e54c-47d0-98c0-2fcb7c6dd0e3"), 32, new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"), "Coy Rogahn", "Customer Operations Specialist" },
                    { new Guid("e5fdbeb1-0515-45b2-b005-56fd8c12c264"), 57, new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"), "Eldred Tremblay", "Regional Factors Coordinator" },
                    { new Guid("e91fa170-0e16-4aaa-91c7-4f31a4289710"), 29, new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"), "Wayne Wehner", "Customer Security Director" },
                    { new Guid("fad528ce-8b3e-445c-96b8-532c194f19fb"), 45, new Guid("7fdd53f7-34aa-4093-8ff6-ad2fb157cd22"), "Isadore Satterfield", "Human Implementation Architect" },
                    { new Guid("fdd73d81-dec4-4db8-ba29-1f6f3a83bb11"), 28, new Guid("4904ae42-f898-4bac-b795-645fd90519cc"), "Jon Roob", "Forward Security Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("0d9df22a-4087-42c1-8df6-04e69c8a9e0f"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("134f255a-e7b8-49f4-ab0c-b6d7c6cbe4ec"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("1afb9570-9431-4104-8592-16cc6069f821"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("1e08e9c0-77b7-4b35-9c91-41d06276a080"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("20e3d58e-6e3d-4dbc-b28a-108b1f515fc6"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("256d64f6-6153-4968-8f8c-02fe5facb496"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2641473c-a1a8-415a-8228-a76e370b211b"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2aa8952b-91c0-44f5-9e5d-8209ea3515db"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2b1fb753-e0e4-4ad4-b398-60833d50a38c"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("326c0964-8e59-45a4-a7a1-24e5c6c13e46"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("33c870dc-41eb-4d15-8c25-97a572b3df32"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("3b2a2647-bc9c-4eb3-a3f3-277edd4a1a22"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("40ae07b4-3b59-4812-8f67-3243a10b8f5e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("485d7512-8f42-404c-98c5-cf444660160f"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("4dc1aa2c-6f0f-47d1-ab90-c8f48df02fb5"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("51c4a842-ab27-4992-ab72-5b001476b271"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("56a547bd-962c-459e-92c5-7f21b9622d8e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("58e021f0-9aab-453c-8ecd-6c319caa5d4f"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("58fd7707-df28-4e24-b867-1d7761679632"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("648d1566-4f4e-467e-a672-afbb68dd9985"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("6551a838-3ed9-45c5-b372-33ce27d6ef40"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("6b90df1e-13a7-4669-8675-fc47e96d5a18"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("6c4d4728-065e-4127-953a-9c669ad9b183"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("6ebba1c1-5d8b-4a5c-9a7f-780eaeaaf37b"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("7b49411c-18d7-476e-b178-346582c7e133"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("7e4e69ac-8bb0-40ca-b7d3-c84db4e6f0ec"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("8068c55a-095c-4351-bc37-d40eaec0e76f"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("82ed97fa-7cfa-4f31-8de7-95d8049bd746"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("8923603d-1238-4c32-afcd-a0ca35382c2e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("89c3c0f2-b20e-4d72-939e-d3cb4abcb5ff"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("8c87fb67-1c1c-413a-a528-c12302348587"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("8fabcfe5-abfc-4760-84f4-73faea003d1e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("96b71d80-f99f-49bb-9b72-d963e1e3fa13"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("a799933d-beb9-476d-a545-1c7dd9c3603e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("b21e7c78-29eb-42f2-b4ee-846c76da6fa1"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("b6e77138-a8a0-402c-842c-7c42f08fecae"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("bb90070a-d48d-4ec5-a5b4-b0418b63299c"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d0157999-7376-4583-9c74-4416262432a0"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d2702671-cc66-42f9-910d-c2497187d645"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d34cc149-a417-486f-b625-3b668a99cc6c"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d3b4f8d2-0ff2-466a-88b5-886b494f317a"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d91466cd-9700-4b74-8ac1-6ef2ce42e38c"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("db1d27e0-1bde-4d8e-be7d-8c6d8b3562fd"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("de59d77b-7bff-496f-baf0-db28d55e5b44"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e366b5b8-2ec3-471d-a5c2-8020849b1c49"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e48749df-e54c-47d0-98c0-2fcb7c6dd0e3"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e5fdbeb1-0515-45b2-b005-56fd8c12c264"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e91fa170-0e16-4aaa-91c7-4f31a4289710"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("fad528ce-8b3e-445c-96b8-532c194f19fb"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("fdd73d81-dec4-4db8-ba29-1f6f3a83bb11"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("159ebb19-771d-4cef-996d-a7a974c598b3"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("3a102b47-5906-4d49-a4c3-19e18d7e60fb"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("47680e25-cda2-4e85-919e-7d723d3dfcc2"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("4904ae42-f898-4bac-b795-645fd90519cc"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("617feaa2-08b0-4433-9de0-88983525e371"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("66cbbac2-ff53-40f4-88e8-d8c182223b7e"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("7fdd53f7-34aa-4093-8ff6-ad2fb157cd22"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("b1d28805-064b-4ca1-9bb4-5adaae8f8867"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("d5f6e332-c6e0-4c59-a148-6372b5599ae6"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("dff2b225-e75a-48cf-8a06-19e89f25d140"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "312 Forest Avenue, BF 923", "USA", "Admin_Solutions Ltd" },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "583 Wall Dr. Gwynn Oak, MD 21207", "USA", "IT_Solutions Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), 35, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Kane Miller", "Administrator" },
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), 26, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Sam Raiden", "Software developer" },
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), 30, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Jana McLeaf", "Software developer" }
                });
        }
    }
}
