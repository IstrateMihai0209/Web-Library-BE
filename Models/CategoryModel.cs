using System.ComponentModel.DataAnnotations;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<BookModel> Books { get; set; }
    }
}
