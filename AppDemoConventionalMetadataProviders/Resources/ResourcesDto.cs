using System.ComponentModel.DataAnnotations;

namespace AppDemoConventionalMetadataProviders.Resources
{
	public class ResourcesDto
	{
		[DataType(DataType.Text)]
		[Display(Name = nameof(NameAndMessage.Name), ResourceType = typeof(NameAndMessage))]
		[StringLength(100, ErrorMessageResourceName = nameof(NameAndMessage.MaxLengthError), ErrorMessageResourceType = typeof(NameAndMessage))]
		public string Name { get; set; }

		[Display(Name = nameof(NameAndMessage.Email), ResourceType = typeof(NameAndMessage))]
		[MaxLength(256, ErrorMessageResourceName = nameof(NameAndMessage.MaxLengthError), ErrorMessageResourceType = typeof(NameAndMessage))]
		[EmailAddress(ErrorMessageResourceName = nameof(NameAndMessage.InvalidEmail), ErrorMessageResourceType = typeof(NameAndMessage))]
		public string Email { get; set; }

		[Display(Name = nameof(NameAndMessage.WebSite), ResourceType = typeof(NameAndMessage))]
		[MaxLength(256, ErrorMessageResourceName = nameof(NameAndMessage.MaxLengthError), ErrorMessageResourceType = typeof(NameAndMessage))]
		[Url(ErrorMessageResourceName = nameof(NameAndMessage.MaxLengthError), ErrorMessageResourceType = typeof(NameAndMessage))]
		public string WebSite { get; set; }

		[DataType(DataType.MultilineText)]
		[Display(Name = nameof(NameAndMessage.Message), ResourceType = typeof(NameAndMessage))]
		[Required(ErrorMessageResourceName = nameof(NameAndMessage.Required), ErrorMessageResourceType = typeof(NameAndMessage))]
		[MaxLength(256, ErrorMessageResourceName = nameof(NameAndMessage.MaxLengthError), ErrorMessageResourceType = typeof(NameAndMessage))]
		public string Message { get; set; }
	}
}


