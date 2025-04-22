using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Repositories.Book
{
    public class BookRepository : Repository<BookModel>, IBookRepository, IReturnsBookList
    {
        public int Count => 40;
        public long PeriodDifference => 1628640000000000;

        public BookRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync(int userId, int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(b => b.UserId == userId)
                .Skip(skip)
                .Take(Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetTopPopularBooksAsync(int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .OrderByDescending(b => b.Popularity)
                .Skip(skip)
                .Take(Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetSimilarBooksAsync(BookModel bookModel)
        {
            return await _dbSet
                .Where(book => book.Title != bookModel.Title && (book.Genre == bookModel.Genre || Math.Abs(bookModel.PublishDate.Ticks - book.PublishDate.Ticks) <= PeriodDifference))
                .Take(12)
                .OrderBy(book => Math.Abs(bookModel.PublishDate.Ticks - book.PublishDate.Ticks))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<BookModel>> SearchBooks(string searchQuery, Dictionary<string, List<string>> filters, int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;
            searchQuery = string.IsNullOrEmpty(searchQuery) ? "" : searchQuery;

            var query = _dbSet.AsQueryable();
            var lowerSearch = searchQuery.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(lowerSearch));

            foreach (var filter in filters)
            {
                var propertyName = filter.Key.ToLower();
                var values = filter.Value.ConvertAll(v => v.ToLower());

                query = propertyName switch
                {
                    "author" => query.Where(b => values.Any(v => b.Author.ToLower().Contains(v))),
                    "publisher" => query.Where(b => values.Any(v => b.Publisher.ToLower().Contains(v))),
                    "genre" => query.Where(b => values.Any(v => b.Genre.ToLower().Contains(v))),
                    //"language" => query.Where(b => values.Any(v => b.Language.ToLower().Contains(v))),
                    _ => query
                };
            }
            
            var dbSet = await query
                .Skip(skip)
                .Take(Count)
                .ToListAsync();

            return dbSet;
        }

        private static Expression<Func<BookModel, bool>> ApplyFilters(Dictionary<string, string> filters)
        {
            var parameter = Expression.Parameter(typeof(BookModel), "s");
            Expression combinedFilter = null;

            var filterGroups = filters
                .GroupBy(kvp =>
                    {
                        var key = kvp.Key.ToLower();
                        if (key.StartsWith("author")) return "Author";
                        if (key.StartsWith("publisher")) return "Publisher";
                        if (key.StartsWith("genre")) return "Genre";
                        if (key.StartsWith("language")) return "Language";
                        return null;
                    }
                )
                .Where(g => g.Key != null)
                .ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Value.ToLower()).ToList());

            foreach (var group in filterGroups)
            {
                var propertyName = group.Key;
                var values = group.Value;

                foreach (var value in values)
                {
                    var property = Expression.Property(parameter, propertyName);
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Array.Empty<Type>());
                    var propertyToLower = Expression.Call(property, toLowerMethod);
                    var containsValue = Expression.Constant(value, typeof(string));
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var containsCall = Expression.Call(propertyToLower, containsMethod, containsValue);

                    combinedFilter = combinedFilter == null
                        ? containsCall
                        : Expression.Or(combinedFilter, containsCall);
                }
            }

            return combinedFilter == null
                ? s => true
                : Expression.Lambda<Func<BookModel, bool>>(combinedFilter, parameter);
        }
    }
}



















