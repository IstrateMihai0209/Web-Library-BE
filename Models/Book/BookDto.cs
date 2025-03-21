namespace OnlineLibrary.Models.Book
{
    public class BookDto
    {
        public string Title { get; set; }

        public string Author { get; set; }
        
        public string Publisher { get; set; }
        
        public string Genre { get; set; }

        public string Description { get; set; }

        public int PublishYear { get; set; }

        public string MoreAboutAuthor { get; set; }
        
        public int UploaderId { get; set; }
        
        public IFormFile CoverImage { get; set; }
        
        public IFormFile TextFile { get; set; }
    }
}
