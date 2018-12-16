using Microsoft.SqlServer.Dac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.EventApi.Tests
{
	internal class DeployDb
	{
		private string _connectionString;
		private string _dacPacPath;

		public DeployDb(string connectionString, string dacPacPath)
		{
			_connectionString = connectionString;
			_dacPacPath = dacPacPath;
		}

		public void Deploy()
		{
			DacServices ds = new DacServices(_connectionString);
			using (DacPackage dp = DacPackage.Load(_dacPacPath))
			{
				ds.Deploy(dp, @"TicketManagementTest", upgradeExisting: false, options: null, cancellationToken: null);
			}
		}

		public void Drop()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = "USE master; " +
						"ALTER DATABASE [" + connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
						"DROP DATABASE [" + connection.Database + "]";
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
