using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_HospitalDatabase.Data.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Medicaments_MedicamentId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prescriptions",
                table: "Prescriptions");

            migrationBuilder.RenameTable(
                name: "Prescriptions",
                newName: "PatientMedicaments");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_MedicamentId",
                table: "PatientMedicaments",
                newName: "IX_PatientMedicaments_MedicamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientMedicaments",
                table: "PatientMedicaments",
                columns: new[] { "PatientId", "MedicamentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicaments_Medicaments_MedicamentId",
                table: "PatientMedicaments",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "MedicamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicaments_Patients_PatientId",
                table: "PatientMedicaments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicaments_Medicaments_MedicamentId",
                table: "PatientMedicaments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicaments_Patients_PatientId",
                table: "PatientMedicaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientMedicaments",
                table: "PatientMedicaments");

            migrationBuilder.RenameTable(
                name: "PatientMedicaments",
                newName: "Prescriptions");

            migrationBuilder.RenameIndex(
                name: "IX_PatientMedicaments_MedicamentId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_MedicamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prescriptions",
                table: "Prescriptions",
                columns: new[] { "PatientId", "MedicamentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Medicaments_MedicamentId",
                table: "Prescriptions",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "MedicamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
