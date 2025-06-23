using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MultiLanguageCore.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        public int Lang_Id { get; set; }

        [Required(ErrorMessage = "Required")]

        [Display(Name = "Title")]
        [MaxLength(300)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }

        [Display(Name = "ImageName")]
        [MaxLength(50)]
        public string ImageName { get; set; }

        [Display(Name = "CreateDate")]
        public DateTime CreateDate { get; set; }


        public Language Language { get; set; }
    }
}
