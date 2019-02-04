using DataAccess.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Tests.Unit.FakeRepositories.Data
{
	internal class FakeVenueData
	{
		public static List<Venue> Venues()
		{
			return new List<Venue>
			{
				new Venue
				{
					Id = 1, Name ="Symphony Hall", Address = "Birmingham", Description = "Symphony Hall description", Phone = "111-111-111",
					Timezone = "UTC"
				},
				new Venue
				{
					Id = 2, Name ="ENO, Coliseum", Address = "London", Description = "ENO description", Phone = "222-222-222",
					Timezone = "Central Standard Time (Mexico)"
				},
				new Venue
				{
					Id = 3, Name ="Royal Albert Hall", Address = "London", Description = "Royal Albert Hall description", Phone = "333-333-333",
					Timezone = "Central Standard Time (Mexico)"
				}

			};
		}

		public static List<Layout> Layouts()
		{
			return new List<Layout>
			{
				new Layout { Id = 1, VenueId = 1, Description = "The Hall 1" },
				new Layout { Id = 3, VenueId = 2, Description = "The Big hall, 1t floor" },
				new Layout { Id = 4, VenueId = 2, Description = "The Small hall, 1t floor" },
				new Layout { Id = 5, VenueId = 3, Description = "The Main hall" },
				new Layout { Id = 6, VenueId = 2, Description = "The Big hall, 2nd floor" },
				new Layout { Id = 7, VenueId = 1, Description = "The Main hall" }
			};
		}

		public static List<Area> Areas()
		{
			return new List<Area>
			{
				new Area{ Id = 1, LayoutId = 1, Description = "The area #1", CoordX = 1, CoordY = 1},
				new Area{ Id = 2, LayoutId = 3, Description = "Cheap area", CoordX = 2, CoordY = 2},
				new Area{ Id = 3, LayoutId = 1, Description = "The area #2", CoordX = 1, CoordY = 2},
				new Area{ Id = 4, LayoutId = 3, Description = "Expensive area", CoordX = 3, CoordY = 3},
				new Area{ Id = 5, LayoutId = 1, Description = "The area #3", CoordX = 1, CoordY = 1},
				new Area{ Id = 6, LayoutId = 4, Description = "The first area", CoordX = 3, CoordY = 3},
				new Area{ Id = 7, LayoutId = 5, Description = "The first area", CoordX = 1, CoordY = 1},
				new Area{ Id = 8, LayoutId = 6, Description = "The area #1", CoordX = 1, CoordY = 1},
				new Area{ Id = 9, LayoutId = 6, Description = "The area #2", CoordX = 1, CoordY = 1}
			};
		}

		public static List<Seat> Seats()
		{
			return new List<Seat>
			{
				new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1},
				new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1},
				new Seat { Id = 3, AreaId = 1, Number = 3, Row = 1},
				new Seat { Id = 4, AreaId = 1, Number = 1, Row = 2},
				new Seat { Id = 5, AreaId = 1, Number = 2, Row = 2},
				new Seat { Id = 6, AreaId = 2, Number = 1, Row = 1},
				new Seat { Id = 7, AreaId = 2, Number = 2, Row = 1},
				new Seat { Id = 8, AreaId = 2, Number = 1, Row = 2},
				new Seat { Id = 9, AreaId = 7, Number = 1, Row = 1},
				new Seat { Id = 15, AreaId = 4, Number = 1, Row = 1},
				new Seat { Id = 10, AreaId = 7, Number = 1, Row = 2},
				new Seat { Id = 11, AreaId = 7, Number = 2, Row = 1},
				new Seat { Id = 12, AreaId = 7, Number = 2, Row = 2},
				new Seat { Id = 13, AreaId = 3, Number = 1, Row = 1},
				new Seat { Id = 14, AreaId = 5, Number = 1, Row = 1},
				new Seat { Id = 17, AreaId = 6, Number = 1, Row = 1},
				new Seat { Id = 16, AreaId = 8, Number = 1, Row = 1}
			};
		}
	}
}
