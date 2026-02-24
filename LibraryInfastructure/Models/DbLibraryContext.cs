using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryInfrastructure.Models;

public partial class DbLibraryContext : DbContext
{
    public DbLibraryContext()
    {
    }

    public DbLibraryContext(DbContextOptions<DbLibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Salaryfund> Salaryfunds { get; set; }

    public virtual DbSet<Salaryhistory> Salaryhistories { get; set; }

    public virtual DbSet<Scientist> Scientists { get; set; }

    public virtual DbSet<Scientistposition> Scientistpositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=solomiapu411070");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");

            entity.ToTable("department");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Facultyid).HasColumnName("facultyid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Departments)
                .HasForeignKey(d => d.Facultyid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("department_facultyid_fkey");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("faculty_pkey");

            entity.ToTable("faculty");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("position_pkey");

            entity.ToTable("position");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Basesalary)
                .HasPrecision(10, 2)
                .HasColumnName("basesalary");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Salaryfund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("salaryfund_pkey");

            entity.ToTable("salaryfund");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Calculationdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("calculationdate");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Facultyid).HasColumnName("facultyid");
            entity.Property(e => e.Periodend).HasColumnName("periodend");
            entity.Property(e => e.Periodstart).HasColumnName("periodstart");
            entity.Property(e => e.Totalamount)
                .HasPrecision(15, 2)
                .HasColumnName("totalamount");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Department).WithMany(p => p.Salaryfunds)
                .HasForeignKey(d => d.Departmentid)
                .HasConstraintName("salaryfund_departmentid_fkey");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Salaryfunds)
                .HasForeignKey(d => d.Facultyid)
                .HasConstraintName("salaryfund_facultyid_fkey");
        });

        modelBuilder.Entity<Salaryhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("salaryhistory_pkey");

            entity.ToTable("salaryhistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Changedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedate");
            entity.Property(e => e.Newsalary)
                .HasPrecision(10, 2)
                .HasColumnName("newsalary");
            entity.Property(e => e.Oldsalary)
                .HasPrecision(10, 2)
                .HasColumnName("oldsalary");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Scientistid).HasColumnName("scientistid");

            entity.HasOne(d => d.Scientist).WithMany(p => p.Salaryhistories)
                .HasForeignKey(d => d.Scientistid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("salaryhistory_scientistid_fkey");
        });

        modelBuilder.Entity<Scientist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("scientist_pkey");

            entity.ToTable("scientist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Department).WithMany(p => p.Scientists)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("scientist_departmentid_fkey");
        });

        modelBuilder.Entity<Scientistposition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("scientistposition_pkey");

            entity.ToTable("scientistposition");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Employmentrate)
                .HasPrecision(3, 2)
                .HasDefaultValue(1.0m)
                .HasColumnName("employmentrate");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Positionid).HasColumnName("positionid");
            entity.Property(e => e.Scientistid).HasColumnName("scientistid");
            entity.Property(e => e.Startdate).HasColumnName("startdate");

            entity.HasOne(d => d.Department).WithMany(p => p.Scientistpositions)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("scientistposition_departmentid_fkey");

            entity.HasOne(d => d.Position).WithMany(p => p.Scientistpositions)
                .HasForeignKey(d => d.Positionid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("scientistposition_positionid_fkey");

            entity.HasOne(d => d.Scientist).WithMany(p => p.Scientistpositions)
                .HasForeignKey(d => d.Scientistid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("scientistposition_scientistid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
