
using Jewellery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<AppraisalReport> AppraisalReports { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);

            // Certificate Decimal Precision

            modelBuilder.Entity<Certificate>()
                .Property(c => c.Weight)
                .HasColumnType("decimal(18,3)");

            modelBuilder.Entity<Certificate>()
                .Property(c => c.GoldPercentage)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Certificate>()
                .Property(c => c.SilverPercentage)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Certificate>()
                .Property(c => c.CopperPercentage)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Certificate>()
                .Property(c => c.OtherPercentage)
                .HasColumnType("decimal(5,2)");

            // Cart Configuration

            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.SessionId)
                .IsUnique();

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem Configuration

            modelBuilder.Entity<CartItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            // Order Configuration

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice Configuration

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Items)
                .WithOne(i => i.Invoice)
                .HasForeignKey(i => i.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);


            // Invoice Decimal Precision

            modelBuilder.Entity<Invoice>()
                .Property(i => i.SubTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TaxAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);

            // InvoiceItem Configuration

            modelBuilder.Entity<InvoiceItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceItem>()
                .Property(i => i.TotalPrice)
                .HasPrecision(18, 2);

            /////
            ///
            modelBuilder.Entity<AppraisalReport>()
    .HasOne(a => a.Product)
    .WithMany(p => p.AppraisalReports)
    .HasForeignKey(a => a.ProductId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppraisalReport>()
                .Property(a => a.Weight)
                .HasPrecision(18, 3);

            modelBuilder.Entity<AppraisalReport>()
                .Property(a => a.EstimatedValue)
                .HasPrecision(18, 2);

            // OrderItem Configuration

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);
        }
    }
}
