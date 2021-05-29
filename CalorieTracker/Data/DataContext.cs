using System.Reflection;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CalorieTracker.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, string,
        IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IConfiguration _configuration;
        public virtual DbSet<UserInformation> UserInformation { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<MealFood> MealFoods { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var connectionString = _configuration.GetConnectionString("DbConnectionString");
            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsHistoryTable("__EFMigrationsHistory", "CalorieTracker"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("CalorieTracker");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .Property(user => user.EnrolledAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Meal>(mealEntity =>
            {
                mealEntity.HasOne(meal => meal.User)
                    .WithMany(user => user.Meals)
                    .HasForeignKey(meal => meal.UserId);
            });

            modelBuilder.Entity<MealFood>(mealFoodEntity =>
            {
                mealFoodEntity.HasOne(mealFood => mealFood.Meal)
                    .WithMany(meal => meal.MealFoods)
                    .HasForeignKey(mealFood => mealFood.MealId);
                mealFoodEntity.Property(m => m.TotalFat).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.SaturatedFat).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.Cholesterol).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.Sodium).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.Carbohydrate).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.DietaryFiber).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.Sugars).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.Protein).HasDefaultValue(0);
                mealFoodEntity.Property(m => m.Potassium).HasDefaultValue(0);
            });
            
            modelBuilder.Entity<UserInformation>(userInformationEntity =>
            {
                userInformationEntity.HasOne(userInformation => userInformation.User)
                    .WithOne(user => user.UserInformation);
            });
        }
    }
}