using System.ComponentModel.DataAnnotations;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.ReadingHistory
{
    public class ReadingHistoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public List<BookModel> Books { get; set; } = new List<BookModel>();

        [Required]
        public DateTime AccessDate { get; set; }
    }
}
