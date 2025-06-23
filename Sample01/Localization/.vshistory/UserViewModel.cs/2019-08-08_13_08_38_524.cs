using System;
using System.ComponentModel.DataAnnotations;

namespace Sample01.Localization
{
	public class UserViewModel
	{
		public UserViewModel(String pName)
		{
			Name = pName;
		}

		[Display(Name = "MsgId")]
		[Required(ErrorMessage = "MsgId")]
		public String Name { get; set; }
	}
}
