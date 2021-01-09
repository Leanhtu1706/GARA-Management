using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GaraManagement.Models
{
    public partial class GaraContext : DbContext
    {
        public GaraContext()
        {
        }

        public GaraContext(DbContextOptions<GaraContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DetailGoodsDeliveryNote> DetailGoodsDeliveryNotes { get; set; }
        public virtual DbSet<DetailGoodsReceivedNote> DetailGoodsReceivedNotes { get; set; }
        public virtual DbSet<DetailRepair> DetailRepairs { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<GoodsDeliveryNote> GoodsDeliveryNotes { get; set; }
        public virtual DbSet<GoodsReceivedNote> GoodsReceivedNotes { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Pay> Pays { get; set; }
        public virtual DbSet<Repair> Repairs { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<TypeOfSupply> TypeOfSupplies { get; set; }
        public virtual DbSet<Work> Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Gara");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK_User");

                entity.ToTable("Account");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.IdEmployee)
                    .HasConstraintName("FK_Account_Employee");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.CarName).HasMaxLength(50);

                entity.Property(e => e.Color).HasMaxLength(10);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LicensePlates)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer).HasMaxLength(20);

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.IdCustomer)
                    .HasConstraintName("FK_Car_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Address).HasMaxLength(80);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.IdentityCardNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DetailGoodsDeliveryNote>(entity =>
            {
                entity.HasKey(e => new { e.IdGoodsDeliveryNote, e.IdMaterial });

                entity.ToTable("DetailGoodsDeliveryNote");

                entity.HasOne(d => d.IdGoodsDeliveryNoteNavigation)
                    .WithMany(p => p.DetailGoodsDeliveryNotes)
                    .HasForeignKey(d => d.IdGoodsDeliveryNote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailGoodsDeliveryNote_GoodsDeliveryNote");

                entity.HasOne(d => d.IdMaterialNavigation)
                    .WithMany(p => p.DetailGoodsDeliveryNotes)
                    .HasForeignKey(d => d.IdMaterial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailGoodsDeliveryNote_Material");
            });

            modelBuilder.Entity<DetailGoodsReceivedNote>(entity =>
            {
                entity.HasKey(e => new { e.IdGoodsReceivedNote, e.IdMaterial });

                entity.ToTable("DetailGoodsReceivedNote");

                entity.HasOne(d => d.IdGoodsReceivedNoteNavigation)
                    .WithMany(p => p.DetailGoodsReceivedNotes)
                    .HasForeignKey(d => d.IdGoodsReceivedNote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailGoodsReceivedNote_GoodsReceivedNote");

                entity.HasOne(d => d.IdMaterialNavigation)
                    .WithMany(p => p.DetailGoodsReceivedNotes)
                    .HasForeignKey(d => d.IdMaterial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailGoodsReceivedNote_Material");
            });

            modelBuilder.Entity<DetailRepair>(entity =>
            {
                entity.HasKey(e => new { e.IdRepair, e.IdWork })
                    .HasName("PK_DetailRepair_1");

                entity.ToTable("DetailRepair");

                entity.HasOne(d => d.IdRepairNavigation)
                    .WithMany(p => p.DetailRepairs)
                    .HasForeignKey(d => d.IdRepair)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailRepair_Repair");

                entity.HasOne(d => d.IdWorkNavigation)
                    .WithMany(p => p.DetailRepairs)
                    .HasForeignKey(d => d.IdWork)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailRepair_Work");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(80);

                entity.Property(e => e.ContractStartDate).HasColumnType("date");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.IdentityCardNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<GoodsDeliveryNote>(entity =>
            {
                entity.ToTable("GoodsDeliveryNote");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ExportDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_at");

                entity.HasOne(d => d.IdRepairNavigation)
                    .WithMany(p => p.GoodsDeliveryNotes)
                    .HasForeignKey(d => d.IdRepair)
                    .HasConstraintName("FK_GoodsDeliveryNote_Repair");
                entity.HasOne(e => e.IdEmployeeNavigation)
                    .WithMany(p => p.GoodsDeliveryNotes)
                    .HasForeignKey(e => e.IdEmployee);
            });

            modelBuilder.Entity<GoodsReceivedNote>(entity =>
            {
                entity.ToTable("GoodsReceivedNote");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ImportDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_at");

                entity.HasOne(d => d.IdSupplierNavigation)
                    .WithMany(p => p.GoodsReceivedNotes)
                    .HasForeignKey(d => d.IdSupplier)
                    .HasConstraintName("FK_GoodsReceivedNote_Supplier");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("Material");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Create_at");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(10);

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_at");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(d => d.IdType)
                    .HasConstraintName("FK_Material_TypeOfSupplies");
            });
            modelBuilder.Entity<Pay>(entity =>
            {
                entity.ToTable("Pay");
                entity.HasOne(r => r.IdRepairNavigation)
                    .WithMany(p => p.Pays)
                    .HasForeignKey(r => r.IdRepair);
            });
                modelBuilder.Entity<Repair>(entity =>
            {
                entity.ToTable("Repair");

                entity.Property(e => e.DateFinished).HasColumnType("datetime");

                entity.Property(e => e.DateOfFactoryEntry).HasColumnType("datetime");

                entity.HasOne(d => d.IdCarNavigation)
                    .WithMany(p => p.Repairs)
                    .HasForeignKey(d => d.IdCar)
                    .HasConstraintName("FK_Repair_Car");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.Address).HasMaxLength(80);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TypeOfSupply>(entity =>
            {
                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Create_at");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_at");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.ToTable("Work");

                entity.Property(e => e.Description)
                    .HasMaxLength(50);

                entity.Property(e => e.WorkName)
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdServiceNavigation)
                    .WithMany(p => p.Works)
                    .HasForeignKey(d => d.IdService)
                    .HasConstraintName("FK_Work_Service");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
