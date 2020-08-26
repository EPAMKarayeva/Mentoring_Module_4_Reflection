using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyIoC;

namespace IoCSample
{
	[ImportConstructor]
	public class CustomerBLL
	{
		public CustomerBLL(ICustomerDAL dal, Logger logger)
		{ }
	}
	public class CustomerBLL2
	{
		[Import]
		public ICustomerDAL CustomerDAL { get; set; }
		[Import]
		public Logger Logger { get; set; }
	}
}
