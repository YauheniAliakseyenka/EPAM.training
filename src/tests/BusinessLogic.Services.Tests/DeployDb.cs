namespace BusinessLogic.Services.Tests
{
	using Microsoft.SqlServer.Dac;
	using System.Configuration;
	using System.Data.SqlClient;

	internal class DeployDb
	{
		private string _connectionString;
		private string _dacPacPath;

		public DeployDb()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;
			_dacPacPath = ConfigurationManager.AppSettings["DacPacPath"];
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
