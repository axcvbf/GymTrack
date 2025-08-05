using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTrack.Migrations
{
    /// <inheritdoc />
    public partial class SetsPropertyRemovedFromExerciseDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sets",
                table: "ExerciseDatas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "ExerciseDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
