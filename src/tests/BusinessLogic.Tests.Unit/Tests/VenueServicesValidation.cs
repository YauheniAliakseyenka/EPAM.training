using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using BusinessLogic.Services;
using BusinessLogic.Tests.Unit.DiContainer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogin.Unit.Tests
{
	internal class VenueServicesValidation
	{
		private IContainer _container;

		[SetUp]
		public void Init()
		{
            _container = Container.GetContainer();
		}

		[TestCase("Symphony House")]
		[TestCase("ENO, Super Coliseum")]
		[TestCase("Colston Hall")]
		[TestCase("Club Koko")]
		public void Venue_name_is_unique(string venueName)
		{
			//Arrange
			var venue = new VenueDto { Name = venueName };
            var venueService = _container.Resolve<IStoreService<VenueDto, int>>();

            //Act
            var exception = Assert.CatchAsync(async () => await venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.Not.EqualTo("Such venue already exists"));
		}

		[Test]
		public void Venue_is_valid()
		{
			//Arrange
            var venueService = _container.Resolve<IStoreService<VenueDto, int>>();
            var venue = new VenueDto { Name = "Colston Hall", LayoutList = new List<LayoutDto>(), Id = 2 };
			venue.LayoutList.Add(new LayoutDto { AreaList = new List<AreaDto>(), Description = "any layout description", Id = 4 });
			venue.LayoutList.First().AreaList.Add(new AreaDto { Description = "any area description", SeatList = new List<SeatDto>(), Id = 4 });
			venue.LayoutList.First().AreaList.First().SeatList.Add(new SeatDto { Number = 2, Row = 1 });

			//Assert
			Assert.DoesNotThrowAsync(async () => await venueService.Create(venue));
		}

		[Test]
		public void Create_venue_without_layout_expected_exception()
		{
			//Arrange
			var venue = new VenueDto { Name = "Club Koko", LayoutList = new List<LayoutDto>() };
            var venueService = _container.Resolve<IStoreService<VenueDto, int>>();

            //Assert
            Assert.ThrowsAsync<VenueException>(async () => await venueService.Create(venue));
		}

		[TestCase("Symphony Hall")]
		[TestCase("ENO, Coliseum")]
		[TestCase("Royal Albert Hall")]
		public void Venue_name__is_unique_expected_exception(string venueName)
		{
			//Arrange
			var venue = new VenueDto { Name = venueName };
            var venueService = _container.Resolve<IStoreService<VenueDto, int>>();

            //Act
            var exception = Assert.CatchAsync<VenueException>(async () => await venueService.Create(venue));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Such venue already exists");
		}

		[Test]
		public void Venue_is_null_retun_exception()
		{
            //Arrange
            var venueService = _container.Resolve<IStoreService<VenueDto, int>>();

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await venueService.Create(null));
		}

		[TestCase("The Hall 2", 1)]
		[TestCase("The Hall 3", 1)]
		[TestCase("The Main hall, 1t floor", 2)]
		[TestCase("The Medium hall, 2nd floor", 2)]
		[TestCase("The Small hall, 2nd floor", 3)]
		public void Layout_description_is_unique(string description, int venueId)
		{
			//Arrange
			var layout = new LayoutDto { Description = description, VenueId = venueId };
            var layoutService = _container.Resolve<IStoreService<LayoutDto, int>>();

            //Act
            var exception = Assert.CatchAsync(async () => await layoutService.Create(layout));

			//Assert
			Assert.That(exception.Message, Is.Not.EqualTo("Layout description isn't unique"));
		}

		[Test]
		public void Layout_is_valid()
		{
			//Arrange
            var layoutService = _container.Resolve<IStoreService<LayoutDto, int>>();
            var toVenue = 1;
			var layout = new LayoutDto { Description = "Main hall", VenueId = toVenue, AreaList = new List<AreaDto>(), Id = 1 };
			layout.AreaList.Add(new AreaDto { Description = "any area description", SeatList = new List<SeatDto>(), Id = 5 });
			layout.AreaList.First().SeatList.Add(new SeatDto { Number = 2, Row = 1 });

			//Assert
			Assert.DoesNotThrowAsync(async () => await layoutService.Create(layout));
		}

		[Test]
		public void Create_layout_without_area_excepted_exception()
		{
            //Arrange
            var layoutService = _container.Resolve<IStoreService<LayoutDto, int>>();
            var toVenue = 1;
			var layout = new LayoutDto { Description = "Main hall", VenueId = toVenue, AreaList = new List<AreaDto>() };

			//Assert
			Assert.ThrowsAsync<LayoutException>(async () => await layoutService.Create(layout));
		}

		[TestCase("The Hall 1", 1)]
		[TestCase("The big hall, 1t floor", 2)]
		[TestCase("The Big hall, 2nd floor", 2)]
		[TestCase("The Main hall", 3)]
		public void Layout_description_is_unique_expected_exception(string description, int venueId)
		{
            //Arrange
            var layoutService = _container.Resolve<IStoreService<LayoutDto, int>>();
            var layout = new LayoutDto() { Description = description, VenueId = venueId };
			var exception = Assert.CatchAsync<LayoutException>(async () => await layoutService.Create(layout));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Layout description isn't unique");
		}

		[Test]
		public void Layout_is_null_throws_exception()
		{
            //Arrange
            var layoutService = _container.Resolve<IStoreService<LayoutDto, int>>();

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await layoutService.Create(null));
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
            var areaService = _container.Resolve<IStoreService<AreaDto, int>>();
            var area = new AreaDto { Description = description, LayoutId = layoutId };

			//Act
			var exception = Assert.CatchAsync(async () => await areaService.Create(area));

			//Assert
			Assert.That(exception, Is.Not.EqualTo("Area description isn't unique"));
		}

		[Test]
		public void Area_is_valid()
		{
			//Arrange
            var areaService = _container.Resolve<IStoreService<AreaDto, int>>();
            var toLayout = 1;
			var area = new AreaDto { Description = "area #1", LayoutId = toLayout, SeatList = new List<SeatDto>(), Id = 1 };
			area.SeatList.Add(new SeatDto { Row = 3, Number = 2 });

			//Assert
			Assert.DoesNotThrowAsync(async () => await areaService.Create(area));
		}

		[Test]
		public void Create_area_without_seats_expected_exception()
		{
            //Arrange
            var areaService = _container.Resolve<IStoreService<AreaDto, int>>();
            var toLayout = 1;
			var area = new AreaDto { Description = "Area #10", LayoutId = toLayout, SeatList = new List<SeatDto>() };

			//Assert
			Assert.ThrowsAsync<AreaException>(async () => await areaService.Create(area));
		}

		[TestCase("The area #1", 1)]
		[TestCase("The area #2", 1)]
		[TestCase("Expensive area", 3)]
		[TestCase("The first area", 4)]
		[TestCase("The first area", 5)]
		[TestCase("The area #1", 6)]
		public void Area_description_is_unique_excpected_exception(string description, int layoutId)
		{
            //Arrange
            var areaService = _container.Resolve<IStoreService<AreaDto, int>>();
            var area = new AreaDto { Description = description, LayoutId = layoutId };
			var exception = Assert.CatchAsync<AreaException>(async () => await areaService.Create(area));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Area description isn't unique");
		}

		[Test]
		public void Area_is_null_throws_exception()
		{
            //Arrange
            var areaService = _container.Resolve<IStoreService<AreaDto, int>>();

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await areaService.Create(null));
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
            var seatService = _container.Resolve<IStoreService<SeatDto, int>>();
            var seat = new SeatDto { Row = row, Number = number, AreaId = areaId };

			//Assert
			Assert.DoesNotThrowAsync(async () => await seatService.Create(seat));
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
		public void Seat_is_unique_excpected_exception(int row, int number, int areaId)
		{
            //Arrange
            var seatService = _container.Resolve<IStoreService<SeatDto, int>>();
            var seat = new SeatDto { Row = row, Number = number, AreaId = areaId };

			//Act
			var exception = Assert.CatchAsync<SeatException>(async () => await seatService.Create(seat));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Seat already exists");
		}
	}
}
