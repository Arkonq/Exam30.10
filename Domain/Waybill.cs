using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public class Waybill : Entity
	{
		public Guid StoreId { get; set; }
		public int Apples { get; set; }
		public int Bananas { get; set; }
		public int Pears { get; set; }
		public string Action { get; set; }
	}
}
