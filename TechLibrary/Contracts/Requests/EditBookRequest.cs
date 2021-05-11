using System.ComponentModel.DataAnnotations;

namespace TechLibrary.Contracts.Requests
{
    public class EditBookRequest
    {
        // adding model binding
        [Required(ErrorMessage = "Book id is requied")]
        public int BookId { get; set; }
        [Required(ErrorMessage = "Title is requied")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Published date is requied")]
        public string PublishedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescr { get; set; }
    }
}
