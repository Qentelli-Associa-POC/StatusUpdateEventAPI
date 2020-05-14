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

        public virtual DbSet<EventTransactionMapping> EventTransactionMapping { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Hoa> Hoa { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceTracker> InvoiceTracker { get; set; }
        public virtual DbSet<InvoiceType> InvoiceType { get; set; }
        public virtual DbSet<InvoiceWorkFlow> InvoiceWorkFlow { get; set; }
        public virtual DbSet<InvoiceWorkflowStatus> InvoiceWorkflowStatus { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonHoaMapping> PersonHoaMapping { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<TemplateMapping> TemplateMapping { get; set; }
        public virtual DbSet<TemplateStep> TemplateStep { get; set; }
        public virtual DbSet<TemplateStore> TemplateStore { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<VendorHoaMapping> VendorHoaMapping { get; set; }
        public virtual DbSet<WorkFlowMaster> WorkFlowMaster { get; set; }
        public virtual DbSet<WorkFlowStatus> WorkFlowStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=a66c589266d2e4247bfa6cca967e4ebd-795134233.us-east-2.elb.amazonaws.com;Username=invoiceadmin;Password=Login@123;Database=Associa;Port=31541");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<EventTransactionMapping>(entity =>
            {
                entity.ToTable("EventTransactionMapping", "InvoiceApproval");

                entity.HasIndex(e => e.EventId)
                    .HasName("fki_FK_Events_Id");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Parameter).HasColumnType("character varying");

                entity.Property(e => e.SegmentCondition).HasColumnType("character varying");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventTransactionMapping)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EventTransactionMappingEventIdFkey");

                entity.HasOne(d => d.Hoa)
                    .WithMany(p => p.EventTransactionMapping)
                    .HasForeignKey(d => d.HoaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EventTransactionMappingHoaIdFkey");

                entity.HasOne(d => d.WorkflowCategoryNavigation)
                    .WithMany(p => p.EventTransactionMapping)
                    .HasForeignKey(d => d.WorkflowCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EventTransactionMappingInvoiceTypeFkey");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.ToTable("Events", "InvoiceApproval");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Procedure).IsRequired();
            });

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

                entity.Property(e => e.InvoiceNumber).ValueGeneratedOnAdd();

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
                    .HasConstraintName("InvoiceVendorIdWithPersonIdFkey");
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

            modelBuilder.Entity<InvoiceWorkFlow>(entity =>
            {
                entity.ToTable("InvoiceWorkFlow", "InvoiceApproval");

                entity.HasIndex(e => e.InvoiceId)
                    .HasName("fki_fk_InvoiceWorkFlow_invoiceId");

                entity.HasIndex(e => e.WorkFlowMasterId)
                    .HasName("fki_fk_InvoiceWorkFlow_WorkFlowMasterId");

                entity.Property(e => e.InvoiceWorkFlowId).ValueGeneratedNever();

                entity.Property(e => e.Sequence).HasColumnType("numeric");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceWorkFlow)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_InvoiceWorkFlow_invoiceId");

                entity.HasOne(d => d.WorkFlowMaster)
                    .WithMany(p => p.InvoiceWorkFlow)
                    .HasForeignKey(d => d.WorkFlowMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_InvoiceWorkFlow_WorkFlowMasterId");
            });

            modelBuilder.Entity<InvoiceWorkflowStatus>(entity =>
            {
                entity.ToTable("InvoiceWorkflowStatus", "InvoiceApproval");

                entity.Property(e => e.Id).ValueGeneratedNever();
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

            modelBuilder.Entity<PersonHoaMapping>(entity =>
            {
                entity.ToTable("PersonHoaMapping", "InvoiceApproval");

                entity.Property(e => e.PersonHoaMappingId).ValueGeneratedNever();

                entity.HasOne(d => d.Hoa)
                    .WithMany(p => p.PersonHoaMapping)
                    .HasForeignKey(d => d.HoaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HoaMappingHoaFkey");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonHoaMapping)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PersonHoaMappingHoaIdFkey");
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

                entity.HasIndex(e => e.RoleId)
                    .HasName("fki_RoleId_TemplateStep_Role");

                entity.Property(e => e.TemplateStepId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedBy).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TemplateStep)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("TemplateStepOwnerIdFkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TemplateStep)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("RoleId_TemplateStep_Role");
            });

            modelBuilder.Entity<TemplateStore>(entity =>
            {
                entity.ToTable("TemplateStore", "InvoiceApproval");

                entity.HasIndex(e => e.RoleId)
                    .HasName("fki_RoleId_TemplateStore_Role");

                entity.Property(e => e.TemplateStoreId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.InvoiceType)
                    .WithMany(p => p.TemplateStore)
                    .HasForeignKey(d => d.InvoiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TemplateStoreInvoiceTypeFkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TemplateStore)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("RoleId_TemplateStore_Role");
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

            modelBuilder.Entity<WorkFlowMaster>(entity =>
            {
                entity.ToTable("WorkFlowMaster", "InvoiceApproval");

                entity.Property(e => e.WorkFlowMasterId).ValueGeneratedNever();
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
