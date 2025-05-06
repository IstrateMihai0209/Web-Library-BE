using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineLibrary.Models.Book
{
    public class BookModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("title")]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        [JsonPropertyName("author")]
        [StringLength(50)]
        public string Author { get; set; }
        
        [Required]
        [JsonPropertyName("publisher")]
        [StringLength(50)]
        public string Publisher { get; set; }

        [JsonPropertyName("moreAboutAuthor")]
        [StringLength(200)]
        public string MoreAboutAuthor { get; set; }
        
        [JsonPropertyName("genre")]
        [StringLength(50)]
        public string Genre { get; set; }

        [JsonPropertyName("description")]
        [StringLength(200)]
        public string Description { get; set; }

        [JsonPropertyName("publishDate")]
        public DateTime PublishDate { get; set; }

        [Required]
        [JsonPropertyName("filePath")]
        [StringLength(200)]
        public string FilePath { get; set; }

        [Required]
        [JsonPropertyName("uploadedAt")]
        public DateTime UploadedAt { get; set; }

        [Required]
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("coverImage")]
        public string CoverImage { get; set; }

        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }
    }
}
