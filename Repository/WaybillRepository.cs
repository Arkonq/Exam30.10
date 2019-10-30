using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Repository
{
	public class WaybillRepository
	{
		private readonly string connectionString;

		public WaybillRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void Add(Waybill waybill)
		{
			var sql = "Insert into Waybills (Id, StoreId, Apples, Bananas, Pears, Action) Values (@Id, @StoreId, @Apples, @Bananas, @Pears, @Action);";

			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, waybill);
				if (rowAffected != 1)   // так как вставка всего на 1 строку
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public ICollection<Waybill> GetAll()
		{
			var sql = "Select * From Waybills";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Waybill>(sql).ToList();
			}
		}

		public Waybill GetByNum(int num)
		{
			var sql = "Select * From Waybills " +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT 1 ROWS ONLY ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Waybill>(sql, new { Skip = num - 1});
			}
		}

		public void Delete(Waybill wayBill)
		{
			string sql = "DELETE FROM Waybills " +
									"WHERE Id = @Id; ";
			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, wayBill);
				if (rowAffected != 1)
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}
	}
}
