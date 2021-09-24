using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcodomusTest
{
	public class DbOperation
	{

		public int? OldValue { get; set; }

		public int? Value { get; set; }

		public string Key { get; set; }

		public Guid Transasction { get; set; }
	}
}
