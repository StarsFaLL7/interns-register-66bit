using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "probation_courses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_probation_courses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "probation_projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_probation_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "interns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    is_male = table.Column<bool>(type: "boolean", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    probation_course_id = table.Column<Guid>(type: "uuid", nullable: true),
                    probation_project_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interns", x => x.id);
                    table.ForeignKey(
                        name: "fk_interns_probation_courses_probation_course_id",
                        column: x => x.probation_course_id,
                        principalTable: "probation_courses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_interns_probation_projects_probation_project_id",
                        column: x => x.probation_project_id,
                        principalTable: "probation_projects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_interns_probation_course_id",
                table: "interns",
                column: "probation_course_id");

            migrationBuilder.CreateIndex(
                name: "ix_interns_probation_project_id",
                table: "interns",
                column: "probation_project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interns");

            migrationBuilder.DropTable(
                name: "probation_courses");

            migrationBuilder.DropTable(
                name: "probation_projects");
        }
    }
}
