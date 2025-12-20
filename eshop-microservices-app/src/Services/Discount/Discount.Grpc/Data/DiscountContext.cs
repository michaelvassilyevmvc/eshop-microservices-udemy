using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>()
            .HasData(
                new Coupon { Id = 1, ProductName = "IPhone X", Amount = 150, Description = "IPhone Discount" },
                new Coupon { Id = 2, ProductName = "Samsung 10", Amount = 100, Description = "Samsung Discount" }
            );
    }
}