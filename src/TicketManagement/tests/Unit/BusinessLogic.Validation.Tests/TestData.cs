namespace BusinessLogic.Validation.Tests
{
	using BusinessLogic.ViewEntities;
	using DataAccess.Entities;
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	internal class TestData
	{
		public static IEnumerable<Venue> VenueList()
		{
			var result = new List<Venue>
			{
				new Venue()
				{
					Id = 1, Name ="Symphony Hall", Address = "Birmingham", Description = "just descri", Phone = "111-111-111"
				},
				new Venue()
				{
					Id = 2, Name ="ENO, Coliseum", Address = "London", Description = "descssad", Phone = "222-222-222"
				},
				new Venue()
				{
					Id = 3, Name ="Royal Albert Hall", Address = "London", Description = "desc123", Phone = "333-333-333"
				}

			};

			return result;
		}

		public static IEnumerable<Layout> LayoutList()
		{
			var result = new List<Layout>
			{
				new Layout() { Id = 1, VenueId = 1, Description = "The Hall 1" },
				new Layout() { Id = 3, VenueId = 2, Description = "The Big hall, 1t floor" },
				new Layout() { Id = 4, VenueId = 2, Description = "The Small hall, 1t floor" },
				new Layout() { Id = 5, VenueId = 3, Description = "The Main hall" },
				new Layout() { Id = 6, VenueId = 2, Description = "The Big hall, 2nd floor" },
				new Layout() { Id = 7, VenueId = 1, Description = "The Main hall" }
			};

			return result;
		}

		public static IEnumerable<Area> AreaList()
		{
			var result = new List<Area>
			{
				new Area(){ Id = 1, LayoutId = 1, Description = "The area #1", CoordX = 1, CoordY = 1},
				new Area(){ Id = 2, LayoutId = 3, Description = "Cheap area", CoordX = 2, CoordY = 2},
				new Area(){ Id = 3, LayoutId = 1, Description = "The area #2", CoordX = 1, CoordY = 2},
				new Area(){ Id = 4, LayoutId = 3, Description = "Expensive area", CoordX = 3, CoordY = 3},
				new Area(){ Id = 5, LayoutId = 1, Description = "The area #3", CoordX = 1, CoordY = 1},
				new Area(){ Id = 6, LayoutId = 4, Description = "The first area", CoordX = 3, CoordY = 3},
				new Area(){ Id = 7, LayoutId = 5, Description = "The first area", CoordX = 1, CoordY = 1},
				new Area(){ Id = 8, LayoutId = 6, Description = "The area #1", CoordX = 1, CoordY = 1},
				new Area(){ Id = 9, LayoutId = 6, Description = "The area #2", CoordX = 1, CoordY = 1}
			};

			return result;
		}

		public static IEnumerable<Seat> SeatList()
		{
			var result = new List<Seat>
			{
				new Seat() { Id = 1, AreaId = 1, Number = 1, Row = 1},
				new Seat() { Id = 2, AreaId = 1, Number = 2, Row = 1},
				new Seat() { Id = 3, AreaId = 1, Number = 3, Row = 1},
				new Seat() { Id = 4, AreaId = 1, Number = 1, Row = 2},
				new Seat() { Id = 5, AreaId = 1, Number = 2, Row = 2},
				new Seat() { Id = 6, AreaId = 2, Number = 1, Row = 1},
				new Seat() { Id = 7, AreaId = 2, Number = 2, Row = 1},
				new Seat() { Id = 8, AreaId = 2, Number = 1, Row = 2},
				new Seat() { Id = 9, AreaId = 7, Number = 1, Row = 1},
				new Seat() { Id = 15, AreaId = 4, Number = 1, Row = 1},
				new Seat() { Id = 10, AreaId = 7, Number = 1, Row = 2},
				new Seat() { Id = 11, AreaId = 7, Number = 2, Row = 1},
				new Seat() { Id = 12, AreaId = 7, Number = 2, Row = 2},
				new Seat() { Id = 13, AreaId = 3, Number = 1, Row = 1},
				new Seat() { Id = 14, AreaId = 5, Number = 1, Row = 1},
				new Seat() { Id = 17, AreaId = 6, Number = 1, Row = 1},
				new Seat() { Id = 16, AreaId = 8, Number = 1, Row = 1}
			};

			return result;
		}

		public static IEnumerable<Event> EventList()
		{
			var list = new List<Event>
			{
				new Event(){ Id = 1, LayoutId = 1, Date = DateTime.Today.AddMonths(1).AddHours(15).AddMinutes(30),
					Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
					Name ="Parsifal"},
				new Event(){ Id = 4, LayoutId = 1, Date = DateTime.Today.AddMonths(3).AddHours(20).AddMinutes(15),
					Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy ",
					Name ="The Mastersingers of Nuremberg"},
				new Event(){ Id = 2, LayoutId = 3, Date = DateTime.Today.AddMonths(2).AddHours(20),
					Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy ",
					Name ="The Mastersingers of Nuremberg"},
				new Event(){ Id = 3, LayoutId = 5, Date = DateTime.Today.AddMonths(1).AddHours(19).AddMinutes(30),
					Description = "Solo-violin sonatas and partitas from Alina Ibragimova", Name ="Late-night Bach: Proms"}
			};

			return list;
		}

		public static IEnumerable<EventArea> EventAreaList()
		{
			var list = new List<EventArea>
			{
				new EventArea(){ Id = 1, EventId = 1, Description = "The area #1", CoordX = 1, CoordY = 1, AreaDefaultId = 1 },
					new EventArea(){ Id = 2, EventId = 1, Description = "The area #2", CoordX = 1, CoordY = 2, AreaDefaultId = 3},
					new EventArea(){ Id = 3, EventId = 1, Description = "The area #3", CoordX = 1, CoordY = 1, AreaDefaultId = 5},
					new EventArea(){ Id = 9, EventId = 3, Description = "The first area", CoordX = 1, CoordY = 1, AreaDefaultId = 7},
			};

			return list;
		}

		public static IEnumerable<EventSeat> EventSeatList()
		{
			var list = new List<EventSeat>
			{
				new EventSeat() { Id = 1, EventAreaId = 1, Number = 1, Row = 1},
				new EventSeat() { Id = 2, EventAreaId = 1, Number = 2, Row = 1},
				new EventSeat() { Id = 3, EventAreaId = 1, Number = 3, Row = 1},
				new EventSeat() { Id = 4, EventAreaId = 1, Number = 1, Row = 2},
				new EventSeat() { Id = 5, EventAreaId = 1, Number = 2, Row = 2},
				new EventSeat() { Id = 13, EventAreaId = 2, Number = 1, Row = 1},
				new EventSeat() { Id = 14, EventAreaId = 3, Number = 1, Row = 1},
				new EventSeat() { Id = 10, EventAreaId = 9, Number = 1, Row = 2},
				new EventSeat() { Id = 11, EventAreaId = 9, Number = 2, Row = 1},
				new EventSeat() { Id = 12, EventAreaId = 9, Number = 2, Row = 2},
				new EventSeat() { Id = 9, EventAreaId = 9, Number = 1, Row = 1},
			};

			return list;
		}

		public static IEnumerable<TestCaseData> EventsValid
		{
			get
			{
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(1).AddHours(18).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 5,
					Date = DateTime.Today.AddMonths(2).AddHours(8).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(1).AddHours(16)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(2).AddHours(8).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 4,
					Date = DateTime.Today.AddMonths(2).AddHours(20).AddMinutes(30)
				});
			}
		}

		public static IEnumerable<TestCaseData> EventsDateisntValid
		{
			get
			{
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(1).AddHours(15).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 5,
					Date = DateTime.Today.AddMonths(1).AddHours(19).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(2).AddHours(20)
				});
			}
		}

		public static IEnumerable<TestCaseData> EventInThePast
		{
			get
			{
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(-1).AddHours(15).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 5,
					Date = DateTime.Today.AddHours(-19).AddMinutes(30)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(-2).AddHours(20)
				});
				yield return new TestCaseData(new EventView()
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(-2).AddHours(-8).AddMinutes(30)
				});
			}
		}
	}
}
