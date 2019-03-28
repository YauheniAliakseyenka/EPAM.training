namespace BusinessLogic.Services.Tests
{
	using Microsoft.SqlServer.Dac;
    using System;
    using System.Configuration;
	using System.Data.SqlClient;
    using System.IO;

    internal class DeployDb
	{
		private string _connectionString;
		private string _dacPacFileName;

		public DeployDb()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;
            _dacPacFileName = ConfigurationManager.AppSettings["DacPacFileName"];
		}

		public void Deploy()
		{
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\", _dacPacFileName));
            DacServices ds = new DacServices(_connectionString);
			using (DacPackage dp = DacPackage.Load(path))
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
