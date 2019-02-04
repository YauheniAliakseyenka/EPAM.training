using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using BusinessLogic.Services.Tests.DiContainer;
using DataAccess.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Tests
{
	internal class VenueApi
	{
		private IStoreService<VenueDto, int> venueService;
		private DeployDb dateBase;

		//object
		private VenueDto venue;

		[OneTimeSetUp]
		public void Init()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;
			dateBase = new DeployDb();
			dateBase.Deploy();

			venueService = Container.GetContainer().Resolve<IStoreService<VenueDto, int>>();

			//venue
			venue = new VenueDto
            {
				Address = "London",
				Name = "New Royal Albert Hall11",
				Description = "Royal Albert Hall",
				Phone = "111-222-333",
				Timezone = "Central Standard Time (Mexico)"
			};

			//layout
			var layoutFirst = new LayoutDto
			{
				Description = "The Hall #1"
			};

			//area 1
			var areaFirst = new AreaDto
			{
				CoordX = 1,
				CoordY = 2,
				Description = "The area #1"
			};

			//area 2
			var areaSecond = new AreaDto
			{
				CoordX = 1,
				CoordY = 2,
				Description = "The area #2"
			};

			//seat list 1
			var seatListFirst = new List<SeatDto>
			{
				new SeatDto { Row = 1, Number = 1 },
				new SeatDto { Row = 1, Number = 2 },
				new SeatDto { Row = 2, Number = 1 }
			};
            
			//seat list 2
			var seatListSecond = new List<SeatDto>
			{
				new SeatDto { Row = 1, Number = 1 },
				new SeatDto { Row = 1, Number = 2 },
				new SeatDto { Row = 1, Number = 3 }
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
			var exception = Assert.CatchAsync<VenueException>(async () => await venueService.Create(venue));

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
			venue.LayoutList.Last().AreaList.Last().SeatList = new List<SeatDto>();
			var exception = Assert.CatchAsync<AreaException>(async () => await venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Incorrect state of area. An area must have atleast one seat"));
			venue.LayoutList.Last().AreaList.Last().SeatList = temp;
		}

		[Test, Order(3)]
		public void Create_venue_which_is_null()
		{
			//Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await venueService.Create(null));
		}

		[Test, Order(4)]
		public async Task Create_valid_venue()
		{
			//Act
			await venueService.Create(venue);
			var inserted = await venueService.Get(venue.Id);

			//Assert
			Assert.IsNotNull(inserted);
		}

		[Test, Order(5)]
		public void Create_not_unique_by_name_venue()
		{
			//Act
			var exception = Assert.CatchAsync<VenueException>(async () => await venueService.Create(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Such venue already exists"));
		}

		[Test, Order(6)]
		public async Task Update_venue()
		{
			//Arrange
			venue.Description = "changing of description";
			venue.Name = "Test unique name";

			//Act
			await venueService.Update(venue);
			venue.LayoutList.Clear(); // set list to 0 to check object because  method 'Get' doesn't back dependent objects

			//Assert
			Assert.AreEqual(venue, await venueService.Get(venue.Id));
		}

		[Test, Order(7)]
		public void Update_venue_which_is_null()
		{
			//Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await venueService.Update(null));
		}

		[Test, Order(8)]
		public void Update_venue_name_existing_name()
		{
			//Arrange
			var temp = venue.Name;
			venue.Name = "Royal Albert Hall";

			//Act
			var exception = Assert.CatchAsync<VenueException>(async () => await venueService.Update(venue));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Such venue already exists"));
			venue.Name = temp;
		}

		[Test, Order(9)]
		public async Task Get_venue_list()
		{
			//Act
			var list = await venueService.GetList();

			//Assert
			Assert.Multiple(() =>
			{
				Assert.IsTrue(list.Any());
				Assert.IsTrue(list.Contains(venue));
			});
		}

		[Test, Order(10)]
		public async Task Delete_venue()
		{
            //Act
            await venueService.Delete(venue.Id);

            //Assert
            Assert.IsNull(await venueService.Get(venue.Id));
		}

		[OneTimeTearDown]
		public void CleanUp()
		{
			dateBase.Drop();
		}
	}
}
