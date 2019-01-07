using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.Services.EventServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogin.Unit.Tests
{
	internal class EventServicesValidation
	{
		[TestCase("The area #4", 1)]
		[TestCase("The area #5", 1)]
		[TestCase("The area #1", 3)]
		public void Event_area_description_is_unique(string description, int eventId)
		{
			//Arrange
			var store = MockWorkUnit.GetUnit();
			var eventSeatService = new EventSeatService(store);
			var eventAreaService = new EventAreaService(store, eventSeatService);
			var eventArea = new EventAreaDto { Description = description, EventId = eventId };

			//Act
			var exception = Assert.Catch<EventAreaException>(() => eventAreaService.Create(eventArea));

			//Assert
			Assert.That(exception.Message, Is.Not.EqualTo("Area description isn't unique"));
		}

		[TestCase("The area #1", 1)]
		[TestCase("The area #3", 1)]
		[TestCase("The first area", 3)]
		public void Event_area_is_unique_throws_exception(string description, int eventId)
		{
			//Arrange
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), null);
			var eventArea = new EventAreaDto { Description = description, EventId = eventId };

			//Act
			var exception = Assert.Catch<EventAreaException>(() => eventAreaService.Create(eventArea));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Area description isn't unique");
		}

		[Test]
		public void Event_area_is_null_throws_exception()
		{
			//Arrange
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), null);

			//Assert
			Assert.Throws<ArgumentNullException>(() => eventAreaService.Create(null));
		}

		[Test, TestCaseSource(typeof(TestingData), "EventsValid")]
		public void Event_is_valid(EventDto e)
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());

			//Assert
			Assert.DoesNotThrow(() => eventService.Create(e));
		}

		[Test, TestCaseSource(typeof(TestingData), "EventsDateIsNotValid")]
		public void Events_date_is_valid_expected_exception(EventDto e)
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(e));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Invalid date");
		}

		[Test, TestCaseSource(typeof(TestingData), "EventInThePast")]
		public void Events_in_the_past_expected_exception(EventDto e)
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(e));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Attempt of creating event with a date in the past");
		}

		[Test]
		public void Event_is_null_throws_exception()
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());

			//Assert
			Assert.Throws<ArgumentNullException>(() => eventService.Create(null));
		}

		[Test]
		public void Delete_event_with_locked_seats_expected_exception()
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Delete(1));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Not allowed to delete. Event has locked seat");
		}

		[Test]
		public void Delete_event_without_locked_seats()
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());

			//Assert
			Assert.DoesNotThrow(() => eventService.Delete(3));
		}

		[Test]
		public void Change_layout_of_event_with_locked_seats_expected_exception()
		{
			//Arrange
			var eventService = new EventService(MockWorkUnit.GetUnit());
			var update = eventService.Get(1);
			update.LayoutId = 2;

			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Update(update));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Not allowed to update layout. Event has locked seats");
		}

		[Test]
		public void Create_event_area_without_event_seats_expected_exception()
		{
			//Arrange
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), null);
			var create = new EventAreaDto
			{
				Seats = new List<EventSeatDto>(),
				CoordX = 1,
				CoordY = 2,
				Description = "Area #2",
				EventId = 1,
				Price = 240.25M
			};

			//Act
			var exception = Assert.Catch<EventAreaException>(() => eventAreaService.Create(create));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Invalid state of event area. Seat list is empty");
		}

		[Test]
		public void Create_event_area()
		{
			//Arrange
			var store = MockWorkUnit.GetUnit();
			var eventSeatService = new EventSeatService(store);
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), eventSeatService);
			var create = new EventAreaDto
			{
				Seats = new List<EventSeatDto>
				{
					new EventSeatDto{State = 0, Number = 1, Row = 1},
					new EventSeatDto{State = 0, Number = 2, Row = 1},
					new EventSeatDto{State = 0, Number = 3, Row = 1},
					new EventSeatDto{State = 0, Number = 1, Row = 2}
				},
				CoordX = 1,
				CoordY = 2,
				Description = "Area #2",
				EventId = 1,
				Price = 155.35M,
				Id = 10
			};

			Assert.DoesNotThrow(() => eventAreaService.Create(create));
		}

		[Test]
		public void Delete_event_area_expected_exception()
		{
			//Arrange
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), null);

			//Act
			var exception = Assert.Catch<EventAreaException>(() => eventAreaService.Delete(1));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Not allowed to delete. Area has locked seat");
		}

		[Test]
		public void Delete_event_area()
		{
			//Arrange
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), null);

			//Assert
			Assert.DoesNotThrow(() => eventAreaService.Delete(3));
		}

		[Test]
		public void Update_event_area_without_event_seats_expected_exception()
		{
			//Arrange
			var eventAreaService = new EventAreaService(MockWorkUnit.GetUnit(), null);
			var update = eventAreaService.Get(1);
			update.Seats = new List<EventSeatDto>();

			//Act
			var exception = Assert.Catch<EventAreaException>(() => eventAreaService.Update(update));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Invalid state of event area. Seat list is empty");
		}

		[Test]
		public void Update_event_area_with_one_added_seat()
		{
			//Arrange
			var store = MockWorkUnit.GetUnit();
			var eventSeatService = new EventSeatService(store);
			var eventAreaService = new EventAreaService(store, eventSeatService);
			var update = eventAreaService.Get(1);
			update.Seats = eventSeatService.FindBy(x => x.EventAreaId == update.Id).ToList();
			update.Seats.Add(new EventSeatDto { Row = 3, Number = 1, State = 0, EventAreaId = 1 });

			//Assert
			Assert.DoesNotThrow(() => eventAreaService.Update(update));
		}

		[Test]
		public void Update_event_area_with_one_deleted_seat()
		{
			//Arrange
			var store = MockWorkUnit.GetUnit();
			var eventSeatService = new EventSeatService(store);
			var eventAreaService = new EventAreaService(store, eventSeatService);
			var update = eventAreaService.Get(1);
			update.Seats = eventSeatService.FindBy(x => x.EventAreaId == update.Id).ToList();
			update.Seats.RemoveAt(1);

			//Assert
			Assert.DoesNotThrow(() => eventAreaService.Update(update));
		}
	}
}
