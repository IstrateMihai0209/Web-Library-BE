namespace OnlineLibrary.Models.Repositories.Category
{
    public class CategoryRepository : Repository<CategoryModel>, ICategoryRepository
    {
        public CategoryRepository(LibraryDbContext dbContext) : base(dbContext) { }
    }
}
