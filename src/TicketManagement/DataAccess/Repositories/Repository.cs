using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal  class Repository<T> : IRepository<T> where T : class, new()
    {
		public string ConnectionString { get; private set; }

		public Repository(string connStr)
        {
			ConnectionString = connStr;
        }

		public virtual int Create(T entity)
		{
			int id;
			if (entity == null)
				throw new NullReferenceException();

			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				using (var command = conn.CreateCommand())
				{
					command.Connection = conn;

					Reflection(entity, command, true);

					var sb = new StringBuilder("INSERT INTO [").Append(typeof(T).Name).Append("] VALUES(");
					Reflection(sb, entity, true);
					sb.Remove(sb.Length - 1, 1).Append(") SELECT SCOPE_IDENTITY()");

					command.CommandText = sb.ToString();

					id = Convert.ToInt32(command.ExecuteScalar());
				}
			}

			return id;
		}

		public virtual int Delete(int id)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				using (var command = conn.CreateCommand())
				{
					command.CommandText = "DELETE FROM [" + typeof(T).Name + "] WHERE [" + typeof(T).Name + "].[Id] = @Id";
					command.Connection = conn;

					command.Parameters.Add(new SqlParameter() { ParameterName = "@Id", Value = id });

					command.ExecuteNonQuery();
				}
			}

			return 1;
		}

        public virtual T Get(int id)
        {
			T entity = null;
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                { 
					command.CommandText = "SELECT * FROM [" + typeof(T).Name + "] WHERE [" + typeof(T).Name + "].[Id] = @Id";
                    command.Connection = conn;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Id", Value = id });

                    using (var reader = command.ExecuteReader())
                    {
						while (reader.Read())
                        {
							if(entity == null)
								entity = new T();

							Reflection(entity, reader);
                        }
                    }                 
                }
            }

			return entity;
		}

        public virtual IEnumerable<T> GetList()
        {
			List<T> entity = null;
			using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                   
                    command.CommandText = "SELECT * FROM " + typeof(T).Name+" option (fast 500)";
                    command.Connection = conn;

                    using (var reader = command.ExecuteReader())
                    {
						entity = new List<T>();
                        while (reader.Read())
                        {
							var row = new T();
							Reflection(row, reader);
							entity.Add(row);
                        }
                    }                   
                }
            }

			return entity;
		}

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new NullReferenceException();

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.Connection = conn;

					Reflection(entity, command, false);

					var sb = new StringBuilder("UPDATE [").Append(typeof(T).Name).Append("] SET ");
					Reflection(sb, entity, false);
					sb.Remove(sb.Length - 1, 1).Append(" WHERE [").Append(typeof(T).Name).Append("].[Id] = @Id");

					command.CommandText = sb.ToString();

					command.ExecuteNonQuery();
                }
            }
        }

        protected void Reflection(object entity, IDataReader reader)
        {
            foreach (var property in entity.GetType().GetProperties().
                        Where(x => !typeof(IEnumerable).IsAssignableFrom(x.PropertyType) || x.PropertyType == typeof(string)))
            {
                entity.GetType().GetProperty(property.Name).SetValue(entity, reader[property.Name]);
            }
        }

        protected void Reflection(object entity, IDbCommand command, bool addRow)
        {
            var properties = entity.GetType().GetProperties().
                        Where(x => !typeof(IEnumerable).IsAssignableFrom(x.PropertyType) || x.PropertyType == typeof(string));

            if (addRow)
                properties = properties.Skip(1);

            foreach (var property in properties)
            {
                command.Parameters.Add(new SqlParameter()
                { ParameterName = "@" + property.Name, Value = property.GetValue(entity) });
            }
        }

		private void Reflection(StringBuilder sb, object entity, bool addRow)
		{
			var properties = entity.GetType().GetProperties().
						Where(x => !typeof(IEnumerable).IsAssignableFrom(x.PropertyType) || x.PropertyType == typeof(string)).Skip(1);

			foreach (var property in properties)
			{
				if (addRow)
					sb.Append("@").Append(property.Name).Append(",");
				else
					sb.Append(property.Name).Append(" = @").Append(property.Name).Append(",");
			}
		}

	}
}
