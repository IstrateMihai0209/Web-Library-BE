using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.ReadBooks;
using OnlineLibrary.Models.ReadingHistory;
using OnlineLibrary.Models.Wishlist;

namespace OnlineLibrary.Models
{
    public class LibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<BookModel> Books { get; set; }
        public DbSet<LoginModel> Login { get; set; }
        public DbSet<ReadingHistoryModel> ReadingHistories { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<WishlistModel> Wishlists { get; set; }

        public DbSet<ReadBooksModel> ReadBooks { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<CategoryModel>().HasData(
            //     new CategoryModel
            //     {
            //         Id = 1,
            //         Name = "Romanian Novels XX",
            //         Description = "All romanian novels from XX century"
            //     });
            //
            // modelBuilder.Entity<RoleModel>().HasData(
            //     new RoleModel
            //     {
            //         Id = 1,
            //         Name = "Admin"
            //     });
            //
            // modelBuilder.Entity<UserModel>().HasData(
            //     new UserModel
            //     {
            //         Id = 1,
            //         Name = "Admin",
            //         Email = "admin@mail.com",
            //         Password = "secret",
            //         Permission = "Yes",
            //         RoleId = 1,
            //         CreatedAt = new DateTime(2025, 01, 01),
            //         GoogleId = 1,
            //         RememberLogin = true,
            //         ReturnUrl = ""
            //     });
            //
            // modelBuilder.Entity<UserModel>().HasData(
            //     new UserModel
            //     {
            //         Id = 2,
            //         Name = "Admin2",
            //         Email = "admin2@mail.com",
            //         Password = "secret",
            //         Permission = "Yes",
            //         RoleId = 1,
            //         CreatedAt = new DateTime(2025, 01, 01),
            //         GoogleId = 2,
            //         RememberLogin = true,
            //         ReturnUrl = ""
            //     });

            modelBuilder.Entity<LoginModel>().HasNoKey();
            modelBuilder.Entity<RegisterModel>().HasNoKey();
        }
    }
}
