using Microsoft.EntityFrameworkCore;

namespace OnlineLibrary.Models
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<BookModel> Books { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<LoginModel> Login { get; set; }
        public DbSet<ReadingHistoryModel> ReadingHistories { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
