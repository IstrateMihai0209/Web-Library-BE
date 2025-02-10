using System.ComponentModel.DataAnnotations;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models
{
    public class ReadingHistoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public UserModel User { get; set; }

        public ICollection<BookModel> Books { get; set; }

        [Required]
        public DateTime AccessDate { get; set; }
    }
}
