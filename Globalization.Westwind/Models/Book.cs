using System.ComponentModel.DataAnnotations;

namespace Globalization.Westwind.Models
{
    public class Book
    {
        public int BookId { get; set; }
        [Required(ErrorMessage = "ErrorMail")]
        public string Title { get; set; }
    }
}
