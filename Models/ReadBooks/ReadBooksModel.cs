using OnlineLibrary.Models.Book;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models.ReadBooks
{
    public class ReadBooksModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public ICollection<BookModel> Books { get; set; } = new List<BookModel>();
    }
}
