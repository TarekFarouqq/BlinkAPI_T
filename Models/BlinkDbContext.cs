using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Models
{
    public class BlinkDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryTransactionHeader> InventoryTransactionHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewComment> ReviewComments { get; set; }
        public DbSet<StockProductInventory> StockProductInventories { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<TransactionProduct> TransactionProducts { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<FilterAttributes> FilterAttributes { get; set; }
        public DbSet<DefaultAttributes> DefaultAttributes { get; set; }
        public DbSet<ProductAttributes> ProductAttributes { get; set; }
        public DbSet<ReviewSuppliedProduct> ReviewSuppliedProducts { get; set; }
        public DbSet<ReviewSuppliedProductImages> ReviewSuppliedProductImages { get; set; }
        public DbSet<Advertisment> Advertisments { get; set; }
        public BlinkDbContext() : base() 
        {

        }

        public BlinkDbContext(DbContextOptions<BlinkDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }

    }
}
