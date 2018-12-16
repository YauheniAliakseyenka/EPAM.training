using DataAccess.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	internal class EventRepository : Repository<Event>
	{
		public EventRepository(string connStr) : base(connStr) { }

		public override int Create(Event entity)
		{
			int output = 0;
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				using (var command = conn.CreateCommand())
				{
					command.CommandText = "AddEvent";
					command.Connection = conn;
					command.CommandType = System.Data.CommandType.StoredProcedure;

					Reflection(entity, command, true);

					output =  Convert.ToInt32(command.ExecuteScalar());
					entity.Id = output;
				}
			}

			return output;
		}

		public override void Update(Event entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				using (var command = conn.CreateCommand())
				{
					command.CommandText = "UpdateEvent";
					command.Connection = conn;
					command.CommandType = System.Data.CommandType.StoredProcedure;

					Reflection(entity, command, false);
					command.ExecuteNonQuery();
				}
			}
		}

		public override int Delete(int id)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				using (var command = conn.CreateCommand())
				{
					command.CommandText = "DeleteEvent";
					command.Connection = conn;
					command.CommandType = System.Data.CommandType.StoredProcedure;

					command.Parameters.Add(new SqlParameter() { ParameterName = "@Id", Value = id });

					command.ExecuteNonQuery();

					return 1;
				}
			}
		}
	}
}

