using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace TypeRacerAPI.DesignPatterns.Command
{
	public interface IDatabaseCommand
	{
		void Execute();
	}
	public class SelectCommand : IDatabaseCommand
	{
		private readonly DatabaseReceiver _database;
		private readonly string _table;

		public SelectCommand(DatabaseReceiver database, string table)
		{
			_database = database;
			_table = table;
		}

		public void Execute()
		{
			_database.Select(_table);
		}
	}

	public class UpdateCommand : IDatabaseCommand
	{
		private readonly DatabaseReceiver _database;
		private readonly string _data;
		private readonly string _table;
		private readonly int _id;
		private readonly string _column;

		public UpdateCommand(DatabaseReceiver database, string table, int id, string data, string column)
		{
			_database = database;
			_id = id;
			_data = data;
			_table = table;
			_column = column;
		}

		public void Execute()
		{
			_database.Update(_table, _id, _data, _column);
		}
	}

	public class DeleteCommand : IDatabaseCommand
	{
		private readonly DatabaseReceiver _database;
		private readonly int _id;
		private readonly string _table;

		public DeleteCommand(DatabaseReceiver database, int id, string table)
		{
			_database = database;
			_id = id;
			_table = table;
		}

		public void Execute()
		{
			_database.Delete(_id, _table);
		}
	}

}
