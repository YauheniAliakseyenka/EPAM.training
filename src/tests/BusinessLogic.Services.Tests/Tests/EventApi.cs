namespace BusinessLogic.Services.Tests
{
	using BusinessLogic.Services;
	using DataAccess.Entities;
	using System;
	using System.Linq;
	using System.Configuration;
	using NUnit.Framework;
    using BusinessLogic.DTO;
    using BusinessLogic.Exceptions.EventExceptions;
    using System.Threading.Tasks;
	using BusinessLogic.Services.Tests.DiContainer;
	using Autofac;

	internal class EventApi
	{
		private IEventService eventService;
		private int insertedId;
		private DeployDb dateBase;

		[OneTimeSetUp]
		public void Init()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;

			dateBase = new DeployDb();
			dateBase.Deploy();

			eventService = Container.GetContainer().Resolve<IEventService>();
		}

		[Test, Order(1)]
		public async Task Create_event()
		{
			//Arrange
			var add = new EventDto
			{
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Title = "Parsifal",
                ImageURL = "url",
                CreatedBy = 1
			};

			//Act
			await eventService.Create(add);
			insertedId = add.Id;
			var insertedRow = await eventService.Get(insertedId);

            //Assert
            Assert.AreEqual(add, insertedRow);
		}

		[Test, Order(2)]
		public void Create_event_with_invalid_date()
		{
			//Arrange
			var add = new EventDto
            {
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Title = "Parsifal",
                ImageURL = "url",
				CreatedBy = 1
			};

			//Act
			var exception = Assert.CatchAsync<EventException>(async () => await eventService.Create(add));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Invalid date"));
		}

		[Test, Order(3)]
		public void Create_event_which_is_null()
		{
			//Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await eventService.Create(null));
		}

		[Test, Order(4)]
		public void Create_event_without_layout()
		{
			//Arrange
			var add = new EventDto
            {
				LayoutId = 0,
				Date = DateTime.Today.AddMonths(1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Title = "Parsifal",
                ImageURL = "url",
				CreatedBy = 1
            };

			//Act
			var exception = Assert.CatchAsync<EventException>(async () => await eventService.Create(add));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("LayoutId is invalid"));
		}

		[Test, Order(5)]
		public void Create_event_in_the_past_throws_exception()
		{
			//Arrange
			var add = new EventDto
            {
				LayoutId = 2,
				Date = DateTime.Today.AddMonths(-1).AddHours(21).AddMinutes(30),
				Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
				Title = "Parsifal",
                ImageURL = "url",
				CreatedBy = 1
            };

			//Act
			var exception = Assert.CatchAsync<EventException>(async () => await eventService.Create(add));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Attempt of creating event with a date in the past"));
		}

		[Test, Order(6)]
		public async Task Update_event()
		{
			//Arrange
			var update = await eventService.Get(insertedId);

			//Act
			update.Description = "changing of description";
			update.Title = "Test name";
			update.Date = DateTime.Today.AddMonths(2).AddHours(15);
			await eventService.Update(update);

			//Assert
			Assert.AreEqual(update, await eventService.Get(insertedId));
		}

		[Test, Order(7)]
		public void Update_event_which_is_null()
		{
			//Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await eventService.Update(null));
		}

		[Test, Order(8)]
		public async Task Update_event_throws_exception()
		{
			//Arrange
			var update = await eventService.Get(insertedId);

			//Act
			update.Date = DateTime.Today.AddMonths(-2).AddHours(15);
			var exception = Assert.CatchAsync<EventException>(async () => await eventService.Update(update));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Attempt of updating event with a date in the past"));
		}

		[Test, Order(9)]
		public async Task Update_event_without_layout()
		{
			//Arrange
			var update = await eventService.Get(insertedId);

			//Act
			update.LayoutId = 0;
			var exception = Assert.CatchAsync<EventException>(async () => await eventService.Update(update));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("LayoutId is invalid"));
		}

		[Test, Order(10)]
		public async Task Events_get_list()
		{
			//Act
			var events = await eventService.GetList();

			//Assert
			Assert.IsTrue(events.Any(x => x.Id == insertedId));
		}

		[Test, Order(11)]
		public async Task Layout_change()
		{
			//Arrange
			var update = await eventService.Get(insertedId);
			var data = await eventService.GetEventInformation(insertedId);
			var areasBeforeUpdate = data.Areas;

			//Act
			update.LayoutId = 3;
			await eventService.Update(update);
			data = await eventService.GetEventInformation(insertedId);
			var areasAfterUpdate = data.Areas;

			//Assert
			Assert.AreNotEqual(areasAfterUpdate, areasBeforeUpdate);
		}

        [Test, Order(12)]
        public async Task Delete_event_with_locked_seats_expected_exception()
        {
			//Arrange
			var random = new Random();
			var seatService = Container.GetContainer().Resolve<IStoreService<EventSeatDto, int>>();
			var data = await eventService.GetEventInformation(insertedId);
			var area = data.Areas[random.Next(data.Areas.Count)];
			var seat = area.Seats[random.Next(area.Seats.Count)];
			seat.State = SeatState.Ordered;

			//Act
			await seatService.Update(seat);
			var exception = Assert.CatchAsync<EventException>(async () => await eventService.Delete(insertedId));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Not allowed to delete. Event has locked seat"));
		}

        [Test, Order(13)]
		public async Task Delete_event()
		{
			//Arrange
			var eventList = await eventService.GetList();
			var delete = eventList.First();

			//Act
			await eventService.Delete(delete.Id);

			//Assert
			Assert.IsNull(await eventService.Get(delete.Id));
		}

		[OneTimeTearDown]
		public void CleanUp()
		{
			dateBase.Drop();
		}
	}
}
