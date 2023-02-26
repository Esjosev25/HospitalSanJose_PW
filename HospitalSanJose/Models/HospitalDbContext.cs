using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanJose.Models;

public partial class HospitalDbContext : DbContext
{
  public HospitalDbContext()
  {
  }

  public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Appointment> Appointments { get; set; }

  public virtual DbSet<Consultation> Consultations { get; set; }

  public virtual DbSet<Department> Departments { get; set; }

  public virtual DbSet<Doctor> Doctors { get; set; }

  public virtual DbSet<DoctorsInfo> DoctorsInfos { get; set; }

  public virtual DbSet<Function> Functions { get; set; }

  public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

  public virtual DbSet<PersonalInfo> PersonalInfos { get; set; }

  public virtual DbSet<Prescription> Prescriptions { get; set; }

  public virtual DbSet<Role> Roles { get; set; }

  public virtual DbSet<User> Users { get; set; }

  public virtual DbSet<RoleFunction> RoleFunctions { get; set; }

  public virtual DbSet<UserRole> UserRoles { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)

    {

      IConfigurationRoot configuration = new ConfigurationBuilder()

      .SetBasePath(Directory.GetCurrentDirectory())

      .AddJsonFile("appsettings.json")

      .Build();

      var connectionString = configuration.GetConnectionString("HospitalDB");

      optionsBuilder.UseMySQL(connectionString);

    }
  }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Appointment>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("appointments");

      entity.HasIndex(e => e.DoctorId, "doctor_id");

      entity.HasIndex(e => e.UserId, "user_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.AppointmentDate)
              .HasColumnType("date")
              .HasColumnName("appointment_date");
      entity.Property(e => e.AppointmentTime)
              .HasColumnType("time")
              .HasColumnName("appointment_time");
      entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
              .HasForeignKey(d => d.DoctorId)
              .HasConstraintName("appointments_ibfk_1");

      entity.HasOne(d => d.User).WithMany(p => p.Appointments)
              .HasForeignKey(d => d.UserId)
              .HasConstraintName("appointments_ibfk_2");
    });

    modelBuilder.Entity<Consultation>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("consultations");

      entity.HasIndex(e => e.AppointmentId, "FK_consultations_appointment");

      entity.HasIndex(e => e.DoctorId, "FK_consultations_doctor_id");

      entity.HasIndex(e => e.MedicalRecordsId, "FK_consultations_medical_records");

      entity.HasIndex(e => e.UserId, "FK_consultations_patient_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
      entity.Property(e => e.ConsultationDate)
              .HasColumnType("datetime")
              .HasColumnName("consultation_date");
      entity.Property(e => e.Diagnosis)
              .HasMaxLength(255)
              .HasColumnName("diagnosis");
      entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
      entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes");
      entity.Property(e => e.MedicalRecordsId).HasColumnName("medical_records_id");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.Appointment).WithMany(p => p.Consultations)
              .HasForeignKey(d => d.AppointmentId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_consultations_appointment");

      entity.HasOne(d => d.Doctor).WithMany(p => p.Consultations)
              .HasForeignKey(d => d.DoctorId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_consultations_doctor_id");

      entity.HasOne(d => d.MedicalRecords).WithMany(p => p.Consultations)
              .HasForeignKey(d => d.MedicalRecordsId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_consultations_medical_records");

      entity.HasOne(d => d.User).WithMany(p => p.Consultations)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_consultations_patient_id");
    });

    modelBuilder.Entity<Department>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("departments");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.DepartmentName)
              .HasMaxLength(50)
              .HasColumnName("department_name");
      entity.Property(e => e.Description)
              .HasMaxLength(255)
              .HasColumnName("description");
    });

    modelBuilder.Entity<Doctor>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("doctors");

      entity.HasIndex(e => e.DepartmentId, "FK_doctors_departments");

      entity.HasIndex(e => e.UserId, "FK_doctors_users");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.DepartmentId).HasColumnName("department_id");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.Department).WithMany(p => p.Doctors)
              .HasForeignKey(d => d.DepartmentId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_doctors_departments");

      entity.HasOne(d => d.User).WithMany(p => p.Doctors)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_doctors_users");
    });

    modelBuilder.Entity<DoctorsInfo>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("doctors_info");

      entity.HasIndex(e => e.DoctorId, "FK_doctors_info_doctor_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
      entity.Property(e => e.Qualification)
              .HasMaxLength(255)
              .HasColumnName("qualification");
      entity.Property(e => e.Specialty)
              .HasMaxLength(50)
              .HasColumnName("specialty");
      entity.Property(e => e.YearsOfExperience).HasColumnName("yearsOfExperience");

      entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorsInfos)
              .HasForeignKey(d => d.DoctorId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_doctors_info_doctor_id");
    });

    modelBuilder.Entity<Function>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("functions");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Description)
              .HasMaxLength(255)
              .HasColumnName("description");
      entity.Property(e => e.Name)
              .HasMaxLength(50)
              .HasColumnName("name");
      entity.Property(e => e.Type)
              .HasMaxLength(50)
              .HasColumnName("type");
    });

    modelBuilder.Entity<MedicalRecord>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("medical_records");

      entity.HasIndex(e => e.UserId, "FK_medical_records_patient_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Gender)
              .HasMaxLength(10)
              .HasColumnName("gender");
      entity.Property(e => e.NumberOfChildren).HasColumnName("number_of_children");
      entity.Property(e => e.RecordDate)
              .HasColumnType("date")
              .HasColumnName("record_date");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.User).WithMany(p => p.MedicalRecords)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_medical_records_patient_id");
    });

    modelBuilder.Entity<PersonalInfo>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("personal_info");

      entity.HasIndex(e => e.UserId, "user_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.AddressLine1)
              .HasMaxLength(100)
              .HasColumnName("address_line1");
      entity.Property(e => e.AddressLine2)
              .HasMaxLength(100)
              .HasColumnName("address_line2");
      entity.Property(e => e.Birthdate)
              .HasColumnType("date")
              .HasColumnName("birthdate");
      entity.Property(e => e.City)
              .HasMaxLength(50)
              .HasColumnName("city");
      entity.Property(e => e.Dpi)
              .HasMaxLength(13)
              .IsFixedLength()
              .HasColumnName("dpi");
      entity.Property(e => e.MaritalStatus)
              .HasMaxLength(20)
              .HasColumnName("marital_status");
      entity.Property(e => e.PhoneNumber1)
              .HasMaxLength(20)
              .HasColumnName("phone_number1");
      entity.Property(e => e.PhoneNumber2)
              .HasMaxLength(20)
              .HasColumnName("phone_number2");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.User).WithMany(p => p.PersonalInfos)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("personal_info_ibfk_1");
    });

    modelBuilder.Entity<Prescription>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("prescriptions");

      entity.HasIndex(e => e.ConsultationId, "FK_prescriptions_consultation_id");

      entity.HasIndex(e => e.DoctorId, "FK_prescriptions_doctor_id");

      entity.HasIndex(e => e.UserId, "FK_prescriptions_patient_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
      entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
      entity.Property(e => e.Instructions)
              .HasColumnType("text")
              .HasColumnName("instructions");
      entity.Property(e => e.PrescriptionDate)
              .HasColumnType("date")
              .HasColumnName("prescription_date");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.Consultation).WithMany(p => p.Prescriptions)
              .HasForeignKey(d => d.ConsultationId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_prescriptions_consultation_id");

      entity.HasOne(d => d.Doctor).WithMany(p => p.Prescriptions)
              .HasForeignKey(d => d.DoctorId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_prescriptions_doctor_id");

      entity.HasOne(d => d.User).WithMany(p => p.Prescriptions)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_prescriptions_patient_id");
    });

    modelBuilder.Entity<Role>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("roles");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Description)
              .HasMaxLength(255)
              .HasColumnName("description");
      entity.Property(e => e.Name)
              .HasMaxLength(50)
              .HasColumnName("name");
    });

    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("users");

      entity.HasIndex(e => e.Username, "username").IsUnique();

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Activated).HasColumnName("activated");
      entity.Property(e => e.Deleted).HasColumnName("deleted");
      entity.Property(e => e.Email)
              .HasMaxLength(100)
              .HasColumnName("email");
      entity.Property(e => e.FirstName)
              .HasMaxLength(50)
              .HasColumnName("first_name");
      entity.Property(e => e.Image)
              .HasColumnType("blob")
              .HasColumnName("image");
      entity.Property(e => e.IsLocked).HasColumnName("isLocked");
      entity.Property(e => e.LastName)
              .HasMaxLength(50)
              .HasColumnName("last_name");
      entity.Property(e => e.Password)
              .HasMaxLength(255)
              .HasColumnName("password");
      entity.Property(e => e.Username)
              .HasMaxLength(50)
              .HasColumnName("username");
    });

    modelBuilder.Entity<RoleFunction>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("role_functions");

      entity.HasIndex(e => e.FunctionId, "function_id");

      entity.HasIndex(e => e.RoleId, "role_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.FunctionId).HasColumnName("function_id");
      entity.Property(e => e.RoleId).HasColumnName("role_id");

      entity.HasOne(d => d.Function).WithMany(p => p.RoleFunctions)
              .HasForeignKey(d => d.FunctionId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("user_functions_ibfk_1");

      entity.HasOne(d => d.Role).WithMany(p => p.RoleFunctions)
              .HasForeignKey(d => d.RoleId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("user_functions_ibfk_2");
    });

    modelBuilder.Entity<UserRole>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PRIMARY");

      entity.ToTable("user_roles");

      entity.HasIndex(e => e.RoleId, "role_id");

      entity.HasIndex(e => e.UserId, "user_id");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.RoleId).HasColumnName("role_id");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
              .HasForeignKey(d => d.RoleId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("user_roles_ibfk_2");

      entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("user_roles_ibfk_1");
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
