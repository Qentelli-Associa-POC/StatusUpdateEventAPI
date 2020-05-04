using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Associa.Service.DAL.Models
{
    public partial class AssociaSqlContext : DbContext
    {
        public AssociaSqlContext()
        {
        }

        public AssociaSqlContext(DbContextOptions<AssociaSqlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hoa> Hoa { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceTracker> InvoiceTracker { get; set; }
        public virtual DbSet<InvoiceType> InvoiceType { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<TemplateMapping> TemplateMapping { get; set; }
        public virtual DbSet<TemplateStep> TemplateStep { get; set; }
        public virtual DbSet<TemplateStore> TemplateStore { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<VendorHoaMapping> VendorHoaMapping { get; set; }
        public virtual DbSet<WorkFlowStatus> WorkFlowStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("a3c21f756ae3043e0a22a9d4036448c5-2126734940.us-east-2.elb.amazonaws.com;Username=invoiceadmin;Password=Asso1234cia;Database=Associa;Port=31541");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Hoa>(entity =>
            {
                entity.ToTable("Hoa", "InvoiceApproval");

                entity.Property(e => e.HoaId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice", "InvoiceApproval");

                entity.Property(e => e.InvoiceId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Hoa)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.HoaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceHoaFkey");

                entity.HasOne(d => d.InvoiceType)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.InvoiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceInvoiceTypeFkey");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceVendorIdFkey");
            });

            modelBuilder.Entity<InvoiceTracker>(entity =>
            {
                entity.ToTable("InvoiceTracker", "InvoiceApproval");

                entity.Property(e => e.InvoiceTrackerId).ValueGeneratedNever();

                entity.Property(e => e.CompleteTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.StartTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceTracker)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceTrackerInvoiceIdFkey");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.InvoiceTracker)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceTrackerPersonIdFkey");
            });

            modelBuilder.Entity<InvoiceType>(entity =>
            {
                entity.ToTable("InvoiceType", "InvoiceApproval");

                entity.Property(e => e.InvoiceTypeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.InvoiceCode).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "InvoiceApproval");

                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PersonRoleFkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "InvoiceApproval");

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<TemplateMapping>(entity =>
            {
                entity.ToTable("TemplateMapping", "InvoiceApproval");

                entity.Property(e => e.TemplateMappingId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.TemplateStep)
                    .WithMany(p => p.TemplateMapping)
                    .HasForeignKey(d => d.TemplateStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TemplateMappingTemplateStepIdFkey");

                entity.HasOne(d => d.TemplateStore)
                    .WithMany(p => p.TemplateMapping)
                    .HasForeignKey(d => d.TemplateStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TemplateStoreIdFkey");
            });

            modelBuilder.Entity<TemplateStep>(entity =>
            {
                entity.ToTable("TemplateStep", "InvoiceApproval");

                entity.Property(e => e.TemplateStepId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TemplateStep)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("TemplateStepOwnerIdFkey");
            });

            modelBuilder.Entity<TemplateStore>(entity =>
            {
                entity.ToTable("TemplateStore", "InvoiceApproval");

                entity.Property(e => e.TemplateStoreId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.InvoiceType)
                    .WithMany(p => p.TemplateStore)
                    .HasForeignKey(d => d.InvoiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TemplateStoreInvoiceTypeFkey");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.ToTable("Vendor", "InvoiceApproval");

                entity.Property(e => e.VendorId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.VendorNavigation)
                    .WithOne(p => p.Vendor)
                    .HasForeignKey<Vendor>(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VendorpersonIdFkey");
            });

            modelBuilder.Entity<VendorHoaMapping>(entity =>
            {
                entity.ToTable("VendorHoaMapping", "InvoiceApproval");

                entity.Property(e => e.VendorHoaMappingId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Hoa)
                    .WithMany(p => p.VendorHoaMapping)
                    .HasForeignKey(d => d.HoaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VendorHoaMappingHoaIdFkey");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.VendorHoaMapping)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VendorHoaMappingVendorIdFkey");
            });

            modelBuilder.Entity<WorkFlowStatus>(entity =>
            {
                entity.ToTable("WorkFlowStatus", "InvoiceApproval");

                entity.Property(e => e.WorkFlowStatusId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Status).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.WorkFlowStatus)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkFlowStatusInvoiceIdFkey");

                entity.HasOne(d => d.InvoiceType)
                    .WithMany(p => p.WorkFlowStatus)
                    .HasForeignKey(d => d.InvoiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkFlowStatusInvoiceTypeIdFkey");

                entity.HasOne(d => d.TemplateStep)
                    .WithMany(p => p.WorkFlowStatus)
                    .HasForeignKey(d => d.TemplateStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkFlowStatusInvoiceStepIdFkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
