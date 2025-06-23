using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Collections.Generic;

namespace Sample01.Pages
{
	public class IndexModel : PageModel
	{
		private readonly Service1 service1;
		private readonly Service2 service2;
		private readonly IService service;
		private readonly IService2 service22;
		private readonly IService3 service33;
		private readonly IService4 service44;
		private readonly IFoo foo;
		private readonly IBar bar;

		public IndexModel(Service1 service1, Service2 service2, IService service, IService2 service22,
			IService3 service33, IService4 service44, IFoo foo, IBar bar)
		{
			this.service1 = service1;
			this.service2 = service2;
			this.service = service;
			this.service22 = service22;
			this.service33 = service33;
			this.service44 = service44;
			this.foo = foo;
			this.bar = bar;
		}

		public List<string> ResultListString { get; set; }

		public void OnGet()
		{
			//var i00 = (service as Service).TestIService("service");
			//var i01 = (service as Service1).TestIService("Service1");
			//var i02 = (service as Service2).TestIService("Service2");

			var is00 = service.TestIService("service 00");
			var is002 = service22.TestIService("service 22");
			var is003 = service33.TestIService("service 33");
			var is004 = service44.TestIService("service 44");
			var is01 = service1.TestIService("service 001");
			var is02 = service2.TestIService("service 02");
			var if01 = foo.TestIFoo("foo");
			var ib01 = bar.TestIBar("bar");
			var ls = new List<string> { is002, is003, is004, is00, is01, is02, if01, ib01 };
			ResultListString = ls;
		}
	}
}
