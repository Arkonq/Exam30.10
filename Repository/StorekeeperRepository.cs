using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository
{
	public class StorekeeperRepository
	{
		private readonly string connectionString;

		public StorekeeperRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public bool Authorization(string name, string pass)
		{
			var sql = "Select * from Storekeepers " +
								"where Name = @Name AND Pass = @Pass;";
			using (var connection = new SqlConnection(connectionString))
			{
				if (connection.Query(sql, new { Name = name, Pass = pass }).AsList().Count == 1)
				{
					return true;
				}
				return false;
			}
		}

		public Storekeeper GetStorekeeper(string name)
		{
			var sql = "Select * from Storekeepers " +
								"where Name = @Name";
			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Storekeeper>(sql, new { Name = name });				
			}
		}
	}
}
