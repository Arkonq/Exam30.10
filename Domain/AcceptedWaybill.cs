using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public class AcceptedWaybill : Entity
	{
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public Guid StoreId { get; set; }
		public Guid StorekeeperId { get; set; }
		public Guid WaybillId { get; set; }
	}
}
