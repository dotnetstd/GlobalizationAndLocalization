using System.ComponentModel.DataAnnotations;

namespace JsonLocalizer.Models
{
	public class FormModel
	{
		[Required(ErrorMessage = "Name_Required_MSG")]
		[Display(Name = "Name")]
		public string Name { get; set; }
	}
}
