using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateAppointmentUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointment_ScheduleId_StartDate_EndDate",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ScheduleId_StartDate_EndDate_PatientId",
                table: "Appointment",
                columns: new[] { "ScheduleId", "StartDate", "EndDate", "PatientId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointment_ScheduleId_StartDate_EndDate_PatientId",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ScheduleId_StartDate_EndDate",
                table: "Appointment",
                columns: new[] { "ScheduleId", "StartDate", "EndDate" },
                unique: true);
        }
    }
}
