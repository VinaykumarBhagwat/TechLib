using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.Contracts.Requests
{
    public class AddBookRequest
    {
        // adding model binding
        [Required(ErrorMessage = "Title is requied")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Isbn is requied")]
        public string Isbn { get; set; }
        [Required(ErrorMessage = "Published date is requied")]
        public string PublishedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescr { get; set; }
    }
}
