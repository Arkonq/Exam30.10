using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository
{
	public class AcceptedWaybillRepository
	{
		private readonly string connectionString;

		public AcceptedWaybillRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void Add(Store store, Waybill waybill, Storekeeper storekeeper)
		{
			var sql = "Insert into AcceptedWaybills (Id, StoreId, WaybillId, StorekeeperId) Values (@Id, @StoreId, @WaybillId, @StorekeeperId);";

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Execute(sql, new { Id = Guid.NewGuid(), StoreId = store.Id, WaybillId = waybill.Id, StorekeeperId = storekeeper.Id});
			}
		}
	}
}
