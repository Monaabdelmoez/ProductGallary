using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductGallary.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Gallary> Gallaries { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }


        public Context():base()
        {

        }

        public Context(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = WebApplication.CreateBuilder();

            string connectionString = builder.Configuration.GetConnectionString("AhmedAlaa");
            optionsBuilder.UseSqlServer(connectionString);
            
        }


    }
   
}
