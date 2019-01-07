namespace BusinessLogic.Validation.Tests
{
	using DataAccess.Entities;
	using Moq;
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using BusinessLogic.Services;
	using BusinessLogic.Exceptions;
	using DataAccess.Repositories;
	using DataAccess;

	internal class Validation
	{

		/*private IService<BusinessLogic.Entities.Venue> venueService;
		private IService<BusinessLogic.Entities.Event> eventService;
		private IService<BusinessLogic.Entities.EventArea> eventAreaService;
		private IService<BusinessLogic.Entities.EventSeat> eventSeatService;

		[OneTimeSetUp]
		public void Init()
		{
			var fakeVenueRepo = new Mock<IRepository<Venue>>();
			fakeVenueRepo.Setup(x => x.GetList()).Returns(TestData.VenueList());
            fakeVenueRepo.Setup(x => x.Create(It.IsAny<Venue>())).Verifiable();

			var fakeAreaRepo = new Mock<IRepository<Area>>();
			fakeAreaRepo.Setup(x => x.GetList()).Returns(TestData.AreaList());
			fakeAreaRepo.Setup(x => x.Create(It.IsAny<Area>())).Verifiable();

            var fakeSeatRepo = new Mock<IRepository<Seat>>();
			fakeSeatRepo.Setup(x => x.GetList()).Returns(TestData.SeatList());
			fakeSeatRepo.Setup(x => x.Create(It.IsAny<Seat>())).Verifiable();

            var fakeLayoutRepo = new Mock<IRepository<Layout>>();
			fakeLayoutRepo.Setup(x => x.GetList()).Returns(TestData.LayoutList());
			fakeLayoutRepo.Setup(x => x.Create(It.IsAny<Layout>())).Verifiable();

            var fakeEventRepo = new Mock<IEventRepository>();
			fakeEventRepo.Setup(x => x.GetList()).Returns(TestData.EventList());
			fakeEventRepo.Setup(x => x.Create(It.IsAny<Event>())).Verifiable();

            var fakeEventAreaRepo = new Mock<IEntityFrameworkRepository<EventArea>>();
			fakeEventAreaRepo.Setup(x => x.GetList()).Returns(TestData.EventAreaList());
			fakeEventAreaRepo.Setup(x => x.Create(It.IsAny<EventArea>())).Verifiable();

            var fakeEventSeatRepo = new Mock<IEntityFrameworkRepository<EventSeat>>();
			fakeEventSeatRepo.Setup(x => x.GetList()).Returns(TestData.EventSeatList());
			fakeEventSeatRepo.Setup(x => x.Create(It.IsAny<EventSeat>())).Verifiable();

            venueService = new VenueService(fakeVenueRepo.Object, fakeLayoutRepo.Object, fakeAreaRepo.Object, fakeSeatRepo.Object);

			eventService = new EventService(fakeEventRepo.Object);
			eventAreaService  = new EventAreaService(fakeEventAreaRepo.Object);
			eventSeatService = new EventSeatService(fakeEventSeatRepo.Object);
		}

		[TestCase("Symphony House")]
		[TestCase("ENO, Super Coliseum")]
		[TestCase("Colston Hall")]
		[TestCase("Club Koko")]
		public void Venue_name_is_unique(string venueName)
		{
			//Arrange
			var venue = new Entities.Venue { Name = venueName };

			//Act
			var exception = Assert.Catch(() => venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.Not.EqualTo("Such venue already exists"));
		}

		[Test]
		public void Venue_is_valid()
		{
			//Arrange
			var venue = new Entities.Venue { Name = "Colston Hall", LayoutList = new List<Entities.Layout>(), Id = 2 };
			venue.LayoutList.Add(new Entities.Layout() { AreaList = new List<Entities.Area>(), Description = "any layout description", Id = 4 });
			venue.LayoutList.First().AreaList.Add(new Entities.Area { Description = "any area description", SeatList = new List<Entities.Seat>(), Id = 4 });
			venue.LayoutList.First().AreaList.First().SeatList.Add(new Entities.Seat() { Number = 2, Row = 1 });

			//Assert
			Assert.DoesNotThrow(() => venueService.Create(venue));
		}

		[Test]
		public void Inserting_of_venue_without_layout_expected_exception()
		{
			//Arrange
			var venue = new Entities.Venue { Name = "Club Koko", LayoutList = new List<Entities.Layout>() };

			//Assert
			Assert.Throws<VenueException>(() => venueService.Create(venue));
		}

		[TestCase("Symphony Hall")]
		[TestCase("ENO, Coliseum")]
		[TestCase("Royal Albert Hall")]
		public void Venue_name__is_unique_throws_exception(string venueName)
		{
			//Arrange
			var venue = new Entities.Venue { Name = venueName };
			var exception = Assert.Catch<VenueException>(() => venueService.Create(venue));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Such venue already exists");
		}

		[Test]
		public void Venue_is_null_retun_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => venueService.Create(null));
		}

		[TestCase("The Hall 2", 1)]
		[TestCase("The Hall 3", 1)]
		[TestCase("The Main hall, 1t floor", 2)]
		[TestCase("The Medium hall, 2nd floor", 2)]
		[TestCase("The Small hall, 2nd floor", 3)]
		public void Layout_description_is_unique(string description, int venueId)
		{
			//Arrange
			var layout = new Entities.Layout { Description = description, VenueId = venueId };

			//Act
			var exception = Assert.Catch(() => (venueService as IService<Entities.Layout>).Create(layout));

			//Assert
			Assert.That(exception.Message, Is.Not.EqualTo("Layout description isn't unique"));
		}

		[Test]
		public void Layout_is_valid()
		{
			var toVenue = 1;
			var layout = new Entities.Layout { Description = "Main hall", VenueId = toVenue, AreaList = new List<Entities.Area>(), Id = 1 };
			layout.AreaList.Add(new Entities.Area { Description = "any area description", SeatList = new List<Entities.Seat>(), Id = 5 });
			layout.AreaList.First().SeatList.Add(new Entities.Seat { Number = 2, Row = 1 });

			//Assert
			Assert.DoesNotThrow(() => (venueService as IService<Entities.Layout>).Create(layout));
		}

		[Test]
		public void Inserting_of_layout_without_area_expected_exception()
		{
			var toVenue = 1;
			var layout = new Entities.Layout { Description = "Main hall", VenueId = toVenue, AreaList = new List<Entities.Area>() };

			//Assert
			Assert.Throws<LayoutException>(() => (venueService as IService<Entities.Layout>).Create(layout));
		}


		[TestCase("The Hall 1", 1)]
		[TestCase("The big hall, 1t floor", 2)]
		[TestCase("The Big hall, 2nd floor", 2)]
		[TestCase("The Main hall", 3)]
		public void Layout_description_is_unique_throws_exception(string description, int venueId)
		{
			//Arrange
			var layout = new Entities.Layout() { Description = description, VenueId = venueId };
			var exception = Assert.Catch<LayoutException>(() => (venueService as IService<Entities.Layout>).Create(layout));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Layout description isn't unique");
		}

		[Test]
		public void Layout_is_null_throws_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => (venueService as IService<Entities.Layout>).Create(null));
		}

		[TestCase("The area #5", 1)]
		[TestCase("Very cheap area", 3)]
		[TestCase("Large area", 3)]
		[TestCase("The second area", 4)]
		[TestCase("The second area", 5)]
		[TestCase("The third area", 4)]
		[TestCase("The fourth area", 4)]
		public void Area_description_is_unique(string description, int layoutId)
		{
			//Arrange
			var area = new Entities.Area { Description = description, LayoutId = layoutId };

			//Act
			var exception = Assert.Catch(() => (venueService as IService<Entities.Area>).Create(area));

			//Assert
			Assert.That(exception, Is.Not.EqualTo("Area description isn't unique"));
		}

		[Test]
		public void Area_is_valid()
		{
			var toLayout = 1;
			var area = new Entities.Area { Description = "area #1", LayoutId = toLayout, SeatList = new List<Entities.Seat>(), Id = 1 };
			area.SeatList.Add(new Entities.Seat { Row = 3, Number = 2 });

			//Assert
			Assert.DoesNotThrow(() => (venueService as IService<Entities.Area>).Create(area));
		}

		[Test]
		public void Inserting_of_area_with_no_seat_expected_exception()
		{
			var toLayout = 1;
			var area = new Entities.Area { Description = "Area #10", LayoutId = toLayout, SeatList = new List<Entities.Seat>() };

			//Assert
			Assert.Throws<AreaException>(() => (venueService as IService<Entities.Area>).Create(area));
		}

		[TestCase("The area #1", 1)]
		[TestCase("The area #2", 1)]
		[TestCase("Expensive area", 3)]
		[TestCase("The first area", 4)]
		[TestCase("The first area", 5)]
		[TestCase("The area #1", 6)]
		public void Area_description_is_unique_throws_exception(string description, int layoutId)
		{
			//Arrange
			var area = new Entities.Area { Description = description, LayoutId = layoutId };
			var exception = Assert.Catch<AreaException>(() => (venueService as IService<Entities.Area>).Create(area));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Area description isn't unique");
		}

		[Test]
		public void Area_is_null_throws_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => (venueService as IService<Entities.Area>).Create(null));
		}

		[TestCase(1, 4, 1)]
		[TestCase(2, 3, 1)]
		[TestCase(1, 3, 2)]
		[TestCase(1, 4, 2)]
		[TestCase(2, 2, 2)]
		[TestCase(2, 3, 2)]
		[TestCase(1, 3, 7)]
		[TestCase(1, 4, 7)]
		[TestCase(2, 3, 7)]
		[TestCase(2, 4, 7)]
		[TestCase(3, 1, 7)]
		[TestCase(1, 2, 3)]
		[TestCase(1, 3, 3)]
		[TestCase(2, 1, 3)]
		[TestCase(1, 2, 3)]
		[TestCase(1, 2, 4)]
		[TestCase(2, 1, 4)]
		[TestCase(1, 2, 6)]
		[TestCase(1, 2, 6)]
		public void Seat_is_unique(int row, int number, int areaId)
		{
			//Arrange
			var seat = new Entities.Seat { Row = row, Number = number, AreaId = areaId };

			//Assert
			Assert.DoesNotThrow(() => (venueService as IService<Entities.Seat>).Create(seat));
		}

		[TestCase(1, 1, 1)]
		[TestCase(1, 2, 1)]
		[TestCase(1, 1, 2)]
		[TestCase(1, 2, 2)]
		[TestCase(2, 1, 1)]
		[TestCase(1, 1, 3)]
		[TestCase(1, 1, 7)]
		[TestCase(1, 2, 7)]
		[TestCase(2, 1, 7)]
		[TestCase(2, 2, 7)]
		public void Seat_is_unique_throws_exception(int row, int number, int areaId)
		{
			//Arrange
			var seat = new Entities.Seat { Row = row, Number = number, AreaId = areaId };
			var exception = Assert.Catch<SeatException>(() => (venueService as IService<Entities.Seat>).Create(seat));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Seat already exists");
		}

		[TestCase(2, 2, 1)]
		[TestCase(1, 1, 2)]
		[TestCase(1, 1, 3)]
		public void Event_seat_is_unique_throws_exception(int row, int number, int eventAreaId)
		{
			//Arrange
			var seat = new Entities.EventSeat { Row = row, Number = number, EventAreaId = eventAreaId };
			var exception = Assert.Catch<EventSeatException>(() => eventSeatService.Create(seat));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Seat already exists");
		}

		[TestCase(3, 1, 1)]
		[TestCase(1, 2, 3)]
		[TestCase(2, 4, 9)]
		[TestCase(3, 1, 9)]
		public void Event_seat_is_unique(int row, int number, int eventAreaId)
		{
			//Arrange
			var seat = new Entities.EventSeat { Row = row, Number = number, EventAreaId = eventAreaId };

			//Assert
			Assert.DoesNotThrow(() => eventSeatService.Create(seat));
		}

		[Test]
		public void Event_seat_is_null_throws_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => eventSeatService.Create(null));
		}

		[Test]
		public void Seat_is_null_throws_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => (venueService as IService<Entities.Seat>).Create(null));
		}

		[TestCase("The area #4", 1)]
		[TestCase("The area #5", 1)]
		[TestCase("The area #1", 3)]
		public void Event_area_is_unique(string description, int eventId)
		{
			//Arrange
			var area = new Entities.EventArea { Description = description, EventId = eventId };

			//Assert
			Assert.DoesNotThrow(() => eventAreaService.Create(area));
		}

		[TestCase("The area #1", 1)]
		[TestCase("The area #3", 1)]
		[TestCase("The first area", 3)]
		public void Event_area_is_unique_throws_exception(string description, int eventId)
		{
			//Arrange
			var area = new Entities.EventArea { Description = description, EventId = eventId };
			var exception = Assert.Catch<EventAreaException>(() => eventAreaService.Create(area));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Area description isn't unique");
		}

		[Test]
		public void Event_area_is_null_retun_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => eventAreaService.Create(null));
		}

		[Test, TestCaseSource(typeof(TestData), "EventsValid")]
		public void Event__is_valid(Entities.Event e)
		{
			//Assert
			Assert.DoesNotThrow(() => eventService.Create(e));
		}

		[Test, TestCaseSource(typeof(TestData), "EventsDateisntValid")]
		public void Events_date_is_valid_throws_exception(Entities.Event e)
		{
			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(e));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Invalid date");
		}

		[Test, TestCaseSource(typeof(TestData), "EventInThePast")]
		public void Events_in_the_past_throws_exception(Entities.Event e)
		{
			//Act
			var exception = Assert.Catch<EventException>(() => eventService.Create(e));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Attempt of creating event with a date in the past");
		}

		[Test]
		public void Event_is_null_throws_exception()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => eventService.Create(null));
		}*/
	}
}



