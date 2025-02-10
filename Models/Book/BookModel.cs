using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models.Book
{
    public class BookModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int PublishDate { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string FilePath { get; set; }

        [Required]
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Also known as UploadedBy
        /// </summary>
        [Required]
        public int UserId { get; set; }

        public string CoverImage { get; set; }
    }
}
