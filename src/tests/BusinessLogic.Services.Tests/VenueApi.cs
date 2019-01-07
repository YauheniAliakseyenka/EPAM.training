using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BusinessLogic.Services.Tests
{
	internal class VenueApi
	{
		/*private IService<Entities.Venue> venueService;
		private DeployDb dateBase;

		//object
		private Entities.Venue venue;

		[OneTimeSetUp]
		public void Init()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;
			dateBase = new DeployDb();
			dateBase.Deploy();


			venueService = null;

			//venue
			venue = new Entities.Venue
			{
				Address = "London",
				Name = "New Royal Albert Hall11",
				Description = "Royal Albert Hall",
				Phone = "111-222-333"
			};

			//layout
			var layoutFirst = new Entities.Layout
			{
				Description = "The Hall #1"
			};

			//area 1
			var areaFirst = new Entities.Area
			{
				CoordX = 1,
				CoordY = 2,
				Description = "The area #1"
			};

			//area 2
			var areaSecond = new Entities.Area
			{
				CoordX = 1,
				CoordY = 2,
				Description = "The area #2"
			};

			//seat list 1
			var seatListFirst = new List<Entities.Seat>
			{
				new Entities.Seat { Row = 1, Number = 1 },
				new Entities.Seat { Row = 1, Number = 2 },
				new Entities.Seat { Row = 2, Number = 1 }
			};

			//seat list 2
			var seatListSecond = new List<Entities.Seat>
			{
				new Entities.Seat { Row = 1, Number = 1 },
				new Entities.Seat { Row = 1, Number = 2 },
				new Entities.Seat { Row = 1, Number = 3 }
			};

			venue.LayoutList.Add(layoutFirst);
			venue.LayoutList.Last().AreaList.Add(areaFirst);
			venue.LayoutList.Last().AreaList.Last().SeatList.AddRange(seatListFirst);
			venue.LayoutList.Last().AreaList.Add(areaSecond);
			venue.LayoutList.Last().AreaList.Last().SeatList.AddRange(seatListSecond);
		}

		[Test, Order(1)]
		public void Create_venue_with_incorrect_state_by_layout()
		{
			//Arrange
			var temp = venue.LayoutList.First();

			//Act
			venue.LayoutList.Remove(temp);
			var exception = Assert.Catch<VenueException>(() => venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Incorrect state of the venue. The venue must have at least one layout"));
			venue.LayoutList.Add(temp);
		}

		[Test, Order(2)]
		public void Create_venue_with_incorrect_state_by_seats()
		{
			//Arrange
			var temp = venue.LayoutList.Last().AreaList.Last().SeatList;

			//Act
			venue.LayoutList.Last().AreaList.Last().SeatList = new List<Entities.Seat>();
			var exception = Assert.Catch<AreaException>(() => venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Incorrect state of the area. The area must have at least one seat"));
			venue.LayoutList.Last().AreaList.Last().SeatList = temp;
		}

		[Test, Order(3)]
		public void Create_venue_which_is_null()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => venueService.Create(null));
		}

		[Test, Order(4)]
		public void Create_valid_venue()
		{
			//Act
			venueService.Create(venue);
			var inserted = venueService.Get(venue.Id);

			//Assert
			Assert.IsNotNull(inserted);
		}

		[Test, Order(5)]
		public void Create_not_unique_by_name_venue()
		{
			//Act
			var exception = Assert.Catch<VenueException>(() => venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Such venue already exists"));
		}

		[Test, Order(6)]
		public void Update_venue()
		{
			//Arrange
			venue.Description = "changing of description";
			venue.Name = "Test unique name";

			//Act
			venueService.Update(venue);
			venue.LayoutList.Clear(); // set list to 0 to check object because  method 'Get' doesn't back dependent objects
			var s = venueService.Get(venue.Id);

			//Assert
			Assert.AreEqual(venue, venueService.Get(venue.Id));
		}

		[Test, Order(7)]
		public void Update_venue_which_is_null()
		{
			//Assert
			Assert.Throws<NullReferenceException>(() => venueService.Update(null));
		}

		[Test, Order(8)]
		public void Update_venue_name_existing_name()
		{
			//Arrange
			var temp = venue.Name;
			venue.Name = "Royal Albert Hall";

			//Act
			var exception = Assert.Catch<VenueException>(() => venueService.Update(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Such venue already exists"));
			venue.Name = temp;
		}

		[Test, Order(9)]
		public void Get_venue_list()
		{
			//Act
			var list = venueService.GetList();

			//Assert
			Assert.Multiple(() =>
			{
				Assert.IsTrue(list.Any());
				Assert.IsTrue(list.Contains(venue));
			});
		}

		[Test, Order(10)]
		public void Delete_venue_exptected_true()
		{
			//Assert
			Assert.IsTrue(venueService.Delete(venue.Id));
		}

		[OneTimeTearDown]
		public void CleanUp()
		{
			dateBase.Drop();
		}*/
	}
}
