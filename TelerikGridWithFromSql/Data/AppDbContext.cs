using Microsoft.EntityFrameworkCore;

namespace TelerikGridWithFromSql.Data;

public partial class AppDbContext : DbContext {
  public AppDbContext() {
  }

  public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) {
  }

  public virtual DbSet<Product> Products { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Northwind;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Product>(entity => {
      entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

      entity.HasIndex(e => e.CategoryId, "CategoryID");

      entity.HasIndex(e => e.ProductName, "ProductName");

      entity.HasIndex(e => e.SupplierId, "SupplierID");

      entity.HasIndex(e => e.SupplierId, "SuppliersProducts");

      entity.Property(e => e.ProductId).HasColumnName("ProductID");
      entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
      entity.Property(e => e.ProductName).HasMaxLength(40);
      entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
      entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
      entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
      entity.Property(e => e.UnitPrice)
        .HasDefaultValue(0m)
        .HasColumnType("money");
      entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
      entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}