using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Domain;

namespace Repository
{
	public class StoreRepository
	{
		private readonly string connectionString;

		public StoreRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public Store GetStore()
		{
			var sql = "Select * from Stores;";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Store>(sql);
			}
		}

		public void Add(Store store)
		{
			var sql = "Insert into Stores (Id, Apples, Bananas, Pears) Values (@Id, @Apples, @Bananas, @Pears);";

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Execute(sql, store);	
			}
		}

		public void Update(Store store)
		{
			string sql = "UPDATE Stores " +
				"SET Apples = @Apples," +
				"Bananas = @Bananas, " +
				"Pears = @Pears " +
				"WHERE Id = @Id;";
			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, store);
				if (rowAffected != 1)
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}
	}
}
