using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {

        public HospitalContext()
        {

        }

        public HospitalContext(DbContextOptions options)
            : base(options) { }


        //DbSet<> here !!!
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Doctor> Doctors { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        //TODO:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>(entity =>
            {

                entity.HasKey(m => m.MedicamentId);
                entity.Property(m => m.Name).IsRequired()
                .IsUnicode().HasMaxLength(50);

            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(pm => new { pm.PatientId, pm.MedicamentId });

                entity
                .HasOne(p => p.Patient)
                .WithMany(pr => pr.Prescriptions)
                .HasForeignKey(p => p.PatientId);

                entity
                .HasOne(p => p.Medicament)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.MedicamentId);

            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(d => d.DiagnoseId);

                entity.Property(d => d.Name).IsRequired()
                .IsUnicode().HasMaxLength(50);

                entity.Property(d => d.Comments).IsRequired()
              .IsUnicode().HasMaxLength(250);

                entity.HasOne(d => d.Patient)
                .WithMany(d => d.Diagnoses)
                .HasForeignKey(p => p.PatientId);

            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);

                entity.Property(p => p.FirstName).IsRequired()
             .IsUnicode().HasMaxLength(50);

                entity.Property(p => p.LastName).IsRequired()
             .IsUnicode().HasMaxLength(50);
                entity.Property(p => p.Address).IsRequired()
             .IsUnicode().HasMaxLength(250);

                entity.Property(p => p.Email).IsRequired()
             .IsUnicode(false).HasMaxLength(80);

                entity.Property(p => p.HasInsurance).IsRequired();


            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.HasKey(v => v.VisitationId);

                entity.Property(v => v.Date).IsRequired();
                entity.Property(v => v.Comments).IsRequired()
                .IsUnicode().HasMaxLength(250);


                entity.HasOne(v => v.Patient)
                .WithMany(v => v.Visitations)
                .HasForeignKey(v => v.PatientId);

                entity
                .HasOne(v => v.Doctor)
                .WithMany(v => v.Visitations)
                .HasForeignKey(d => d.DoctorId);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.DoctorId);

                entity.Property(d => d.Name).IsRequired()
                .IsUnicode().HasMaxLength(100);


                entity.Property(d => d.Specialty).IsRequired()
                .IsUnicode().HasMaxLength(100);

               
            });
            
        }
    }
}
