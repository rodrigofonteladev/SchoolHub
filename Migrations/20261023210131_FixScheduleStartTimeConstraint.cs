using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolHub.Migrations
{
    /// <inheritdoc />
    public partial class FixScheduleStartTimeConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Schedule_StartTime",
                table: "Schedules");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Schedule_StartTime",
                table: "Schedules",
                sql: "StartTime < EndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Schedule_StartTime",
                table: "Schedules");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Schedule_StartTime",
                table: "Schedules",
                sql: "StartTime > EndTime");
        }
    }
}
