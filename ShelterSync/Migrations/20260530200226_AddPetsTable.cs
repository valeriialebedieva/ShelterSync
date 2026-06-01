using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShelterSync.Migrations
{
    /// <inheritdoc />
    public partial class AddPetsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Species = table.Column<string>(type: "text", nullable: false),
                    Breed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    IsAdopted = table.Column<bool>(type: "boolean", nullable: false),
                    IsPetOfTheWeek = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Age", "Breed", "ImagePath", "IsAdopted", "IsPetOfTheWeek", "Name", "Notes", "Species" },
                values: new object[,]
                {
                    { 1, 3, "Golden Retriever", "https://images.dog.ceo/breeds/retriever-golden/n02099601_3004.jpg", false, true, "Max", "Friendly and energetic, loves to play fetch. Great with kids!", "Dog" },
                    { 2, 2, "Siamese", "https://cdn2.thecatapi.com/images/N7rlRo9Zi.jpg", false, false, "Luna", "Playful and affectionate. Loves attention and interactive toys.", "Cat" },
                    { 3, 5, "Labrador Mix", "https://images.dog.ceo/breeds/labrador/john_walker_000.jpg", false, false, "Charlie", "Calm and gentle companion. Perfect for families looking for a loyal friend.", "Dog" },
                    { 4, 1, "Tabby", "https://cdn2.thecatapi.com/images/b6yBY91Pg.jpg", false, false, "Whiskers", "Young and curious kitten. Still learning about the world around her.", "Cat" },
                    { 5, 4, "Beagle", "https://images.dog.ceo/breeds/beagle/puppy-1.jpg", false, false, "Buddy", "Sweet-natured and food-motivated. Great for training and outdoor adventures.", "Dog" },
                    { 6, 3, "Persian", "https://cdn2.thecatapi.com/images/c0f_dBlPH.jpg", false, false, "Mittens", "Elegant and serene. Prefers a quiet environment with lots of petting.", "Cat" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pets");
        }
    }
}
