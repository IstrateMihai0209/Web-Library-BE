using OnlineLibrary.Models.Book;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models.ReadingHistory
{
    public class ReadingHistoryDto
    {
        public ICollection<BookModel> Books { get; set; }

        public DateTime AccessDate { get; set; }
    }
}
