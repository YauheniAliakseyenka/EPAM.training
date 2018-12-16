namespace BusinessLogic.EventApi.Tests
{
	using BusinessLogic;
	using BusinessLogic.Services;
	using DataAccess.Entities;
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.SqlServer.Dac;
	using System.Reflection;
	using System.IO;
	using BusinessLogic.ViewEntities;
	using DataAccess.Repositories;
	using System.Configuration;
	using BusinessLogic.Exceptions;
	using DataAccess;

	internal class TestEventApi
	{
		private IService<EventView> eventService;
		private int insertedId;
		private DeployDb dateBase;

		[OneTimeSetUp]
		public void Init()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;
			var dacPath = ConfigurationManager.AppSettings["DacPacPath"];
		
			dateBase = new DeployDb(connectionString, dacPath);
			dateBase.Deploy();

			eventService = new EventService(new EventRepository(connectionString), new Repository<EventArea>(connectionString));
		}

		[Test, Order(1)]
		public void Create_event()
		{
			//Arrange
			var add = new EventView()
			{
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Name = "Parsifal"
			};

			//Act
			eventService.Create(add);
			insertedId = add.Id;
			var insertedRow = eventService.Get(insertedId);

			//Assert
			Assert.IsNotNull(insertedRow);
		}

		[Test, Order(2)]
		public void Create_event_throws_exception()
		{
			//Arrange
			var add = new EventView()
			{
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(-1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Name = "Parsifal"
			};

			//Assert
			Assert.Throws<EventException>(() => eventService.Create(add));
		}

		[Test, Order(3)]
		public void Update_event()
		{
			//Arrange
			var update = eventService.Get(insertedId);

			//Act
			update.Description = "changing of description";
			update.Name = "Test name";
			update.Date = DateTime.Today.AddMonths(2).AddHours(15);
			eventService.Update(update);

			//Assert
			Assert.AreEqual(update, eventService.Get(insertedId));
		}

		[Test, Order(4)]
		public void Events_get_list()
		{
			//Act
			var events = eventService.GetList();

			//Assert
			Assert.IsTrue(events.Count() > 0);
		}

		[Test, Order(5)]
		public void Layout_Change()
		{
			//Arrange
			var update = eventService.Get(insertedId);

			//Act
			update.LayoutId = 3;
			eventService.Update(update);
			var updated = eventService.Get(insertedId);

			//Assert
			Assert.Multiple(() =>
			{
				Assert.IsTrue(update.EventAreaList.Count() > 0);
				Assert.AreNotEqual(update, updated);
			});
		}

		[Test, Order(6)]
		public void Delete_Event()
		{
			//Act
			var deleted = eventService.Delete(insertedId);

			//Assert
			Assert.IsNull(eventService.Get(insertedId));
		}
		
		[OneTimeTearDown]
		public void CleanUp()
		{
			dateBase.Drop();
		}
	}
}
