namespace BusinessLogic.Services.Tests
{
	using BusinessLogic.Services;
	using DataAccess.Entities;
	using System;
	using System.Linq;
	using DataAccess.Repositories;
	using System.Configuration;
	using NUnit.Framework;
	using BusinessLogic.Exceptions;

    internal class EventApi
	{
		/*private IService<Entities.Event> eventService;
		private int insertedId;
		private DeployDb dateBase;

		[OneTimeSetUp]
		public void Init()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;

			dateBase = new DeployDb();
			dateBase.Deploy();

			eventService = null;

			eventService.Find(x => x.LayoutId > 0);
		}

		[Test, Order(1)]
		public void Create_event()
		{
			//Arrange
			var add = new Entities.Event
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
		public void Create_event_with_invalid_date()
		{
			//Arrange
			var add = new Entities.Event
			{
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Name = "Parsifal"
			};

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(add));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Invalid date"));
		}

		[Test, Order(3)]
		public void Create_event_which_is_null()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => eventService.Create(null));
		}

		[Test, Order(4)]
		public void Create_event_without_layout()
		{
			//Arrange
			var add = new Entities.Event
			{
				LayoutId = 0,
				Date = DateTime.Today.AddMonths(1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Name = "Parsifal"
			};

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(add));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("LayoutID equals zero"));
		}

		[Test, Order(5)]
		public void Create_event_in_the_past_throws_exception()
		{
			//Arrange
			var add = new Entities.Event
			{
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(-1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Name = "Parsifal"
			};

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(add));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Attempt of creating event with a date in the past"));
		}

		[Test, Order(6)]
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

		[Test, Order(7)]
		public void Update_event_which_is_null()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => eventService.Update(null));
		}

		[Test, Order(8)]
		public void Update_event_throws_exception()
		{
			//Arrange
			var update = eventService.Get(insertedId);

			//Act
			update.Date = DateTime.Today.AddMonths(-2).AddHours(15);
			var exception = Assert.Catch<EventException>(() => eventService.Update(update));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Attempt of updating event with a date in the past"));
		}

		[Test, Order(9)]
		public void Update_event_without_layout()
		{
			//Arrange
			var update = eventService.Get(insertedId);

			//Act
			update.LayoutId = 0;
			var exception = Assert.Catch<EventException>(() => eventService.Update(update));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Layout equals zero"));
		}

		[Test, Order(10)]
		public void Events_get_list()
		{
			//Act
			var events = eventService.GetList();

			//Assert
			Assert.IsTrue(events.Any(x => x.Id == insertedId));
		}

		[Test, Order(11)]
		public void Layout_change()
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
				//Assert.IsFalse(update.EventAreaList.SequenceEqual(updated.EventAreaList));
				Assert.AreNotEqual(update, updated);
			});
		}

		[Test, Order(12)]
		public void Delete_event()
		{
			//Assert
			Assert.IsTrue(eventService.Delete(insertedId));
		}

		[OneTimeTearDown]
		public void CleanUp()
		{
			dateBase.Drop();
		}*/
	}
}
