using System;
using System.Data;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TypeRacerAPI.DesignPatterns.Command
{
	public class DatabaseReceiver
	{
		private readonly string _connectionString = "Server=SHARKSTOP;Database=TypeRacer;User Id=root;Password=root123;Encrypt=True;TrustServerCertificate=True;";
		public System.Data.DataTable? _results = null;

		public virtual void Select(string table)
		{
			var dataTable = new DataTable();
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = new SqlCommand($"SELECT * FROM {table}", connection))
				using (var adapter = new SqlDataAdapter(command))
				{
					adapter.Fill(dataTable);
				}
			}
			_results = dataTable;
		}

		public virtual void Update(string table, int id, string data, string column)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = new SqlCommand($"UPDATE {table} SET {column} = @Data WHERE Id = @Id", connection))
				{
					command.Parameters.AddWithValue($"@Data", data);
					command.Parameters.AddWithValue("@Id", id);
					command.ExecuteNonQuery();
				}
			}
		}

		public virtual void Delete(int id, string table)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = new SqlCommand($"DELETE FROM {table} WHERE Id = @Id", connection))
				{
					command.Parameters.AddWithValue("@Id", id);
					command.ExecuteNonQuery();
				}
			}
		}
	}

}
