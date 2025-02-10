namespace OnlineLibrary.Models.Book
{
    public class BookDto
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public int PublishDate { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedAt { get; set; }

        public string CoverImage { get; set; }
    }
}
