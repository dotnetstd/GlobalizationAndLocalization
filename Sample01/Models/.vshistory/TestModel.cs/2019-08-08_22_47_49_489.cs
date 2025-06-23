namespace Sample01.Models
{
	public class TestModel
	{
		/// <summary>
		/// Value set by the controller.
		/// </summary>
		public string TestString1 { get; set; }

		/// <summary>
		/// Value resolved internally.
		/// </summary>
		public string TestString2
		{
			get
			{
				return Resources.Resource.Test;
			}
		}
	}
}
