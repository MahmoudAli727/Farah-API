using Application.Helpers;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<BeautyCenter> BeautyCenters { get; set; }
        public DbSet<ServiceForBeautyCenter> servicesForBeautyCenter { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<HallPicture> HallPicture {get; set;}
        public DbSet<ImagesBeautyCenter> BeautyCenterPicture { get; set; }

        public DbSet<Photography> Photographies { get; set; }
        public DbSet<Portfolio> ImagePhotography { get; set; }
        public DbSet<ShopDresses> ShopDresses { get; set; }
        public DbSet<Dress> Dresses { get; set; }

        public DbSet<ImagesBeautyCenter> ImagesBeautyCenter { get; set; }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarPicture> CarPictures { get; set; }

        public DbSet<FavoriteService> FavoriteService { get; set; }

        public DbSet<UserOTP> userOTPs { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BeautyCenter>()
                        .HasMany(bc => bc.ServicesForBeautyCenter)
                        .WithOne(s => s.BeautyCenter)
                        .HasForeignKey(s => s.BeautyCenterId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BeautyCenter>()
                    .HasMany(bc => bc.ImagesBeautyCenter)
                    .WithOne(i => i.beautyCenter)
                    .HasForeignKey(i => i.BeautyCenterId)
                    .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Chat>()
	                .HasOne(c => c.Customer)
	                .WithMany()
	                .HasForeignKey(c => c.CustomerId)
	                .OnDelete(DeleteBehavior.Restrict); // أو NoAction

			modelBuilder.Entity<Chat>()
			    	.HasOne(c => c.Owner)
				    .WithMany()
		     		.HasForeignKey(c => c.OwnerId)
			     	.OnDelete(DeleteBehavior.Cascade);
			
            modelBuilder.Entity<Service>()
                        .HasMany(s => s.FavoriteServices)
                        .WithOne(f => f.Service)
                        .HasForeignKey(f => f.ServiceId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Car>()
                        .HasMany(c => c.Pictures)
                        .WithOne(p => p.Car)
                        .HasForeignKey(p => p.CarID);

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.Property(o => o.IDFrontImage).IsRequired();
                entity.Property(o => o.IDBackImage).IsRequired();
                entity.Property(o => o.UserType).IsRequired();
                entity.Property(o => o.AccountStatus).HasDefaultValue(OwnerAccountStatus.Pending);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                // configuration for Customer can go here
            });

            modelBuilder.Entity<Photography>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Photographer)
                .HasForeignKey(i => i.PhotographerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BeautyCenter>()
           .HasMany(b => b.ServicesForBeautyCenter)
           .WithOne(s => s.BeautyCenter)
           .HasForeignKey(s => s.BeautyCenterId);

            modelBuilder.Entity<Hall>()
                .ToTable("Halls")
                .HasBaseType<Service>();

            modelBuilder.Entity<Photography>()
                        .ToTable("Photograph")
                        .HasBaseType<Service>();

            modelBuilder.Entity<Car>()
                        .ToTable("Cars")
                        .HasBaseType<Service>();
			
            modelBuilder.Entity<ChatMessage>()
	                    .HasOne(cm => cm.Receiver)
	                    .WithMany()
	                    .HasForeignKey(cm => cm.ReceiverId)
	                    .OnDelete(DeleteBehavior.Restrict); // أو NoAction

			modelBuilder.Entity<ChatMessage>()
				        .HasOne(cm => cm.Sender)
				        .WithMany()
				        .HasForeignKey(cm => cm.SenderId)
				        .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ChatMessage>()
				        .HasOne(cm => cm.Chat)
				        .WithMany()
				        .HasForeignKey(cm => cm.ChatId)
				        .OnDelete(DeleteBehavior.NoAction); // أو Restrict
		    
            modelBuilder.Entity<BeautyCenter>()
                        .ToTable("BeautyCenters")
                        .HasBaseType<Service>();

            modelBuilder.Entity<ShopDresses>()
                        .ToTable("ShopDresses")
                        .HasBaseType<Service>();

            // Configure Owner
            modelBuilder.Entity<Owner>()
                .ToTable("Owners") // Table for Owner entity
                .HasBaseType<ApplicationUser>();

            // Configure Customer
            modelBuilder.Entity<Customer>()
                .ToTable("Customers") // Table for Customer entity
                .HasBaseType<ApplicationUser>();

            modelBuilder.Entity<BeautyCenter>()
                .HasMany(b => b.Reviews)
                .WithOne(r => r.BeautyCenter)
                .HasForeignKey(r => r.BeautyCenterId);

            modelBuilder.Entity<BeautyCenter>()
          .HasMany(b => b.ServicesForBeautyCenter)
          .WithOne(s => s.BeautyCenter)
          .HasForeignKey(s => s.BeautyCenterId);



            modelBuilder.Entity<Service>()
                        .HasOne(s => s.Owner)
                        .WithMany(o => o.Services)
                        .HasForeignKey(s => s.OwnerID)
                        .OnDelete(DeleteBehavior.Restrict);
        }

    }

}

