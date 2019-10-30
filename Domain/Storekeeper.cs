using System;

namespace Domain
{
	public class Storekeeper : Entity	 // Принимает, отпускает накладные
	{
		public Guid StoreId { get; set; }
		public string Name { get; set; }
		public string Pass { get; set; }
	}
}
