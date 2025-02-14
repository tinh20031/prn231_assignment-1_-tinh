using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class eStoreDbContext : DbContext

	{
		public eStoreDbContext() { }

        public eStoreDbContext(DbContextOptions<eStoreDbContext> options)
       : base(options) // Truyền option vào DbContext
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			IConfigurationRoot configuration = builder.Build();
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("EStoreDB"));


		}
		public virtual DbSet<Category> categories { get; set; }
		public virtual DbSet<CartItem> CartItems { get; set; }
		public virtual DbSet<Member> members { get; set; }
		public virtual DbSet<Order> orders { get; set; }
		public virtual DbSet<OrderDetail> orderDetails { get; set; }
		public virtual DbSet<Products> products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => od.OrderDetailId); // Đảm bảo OrderDetailId là khóa chính

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Products)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);

            base.OnModelCreating(modelBuilder);
        }



    }

}
