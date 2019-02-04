using DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Tests.Unit.FakeRepositories.Data
{
	internal class FakeEventData
	{
		public static List<Event> Events()
		{
			return new List<Event>
			{
				new Event{ Id = 1, LayoutId = 1, Date = DateTime.Today.AddMonths(1).AddHours(15).AddMinutes(30),
					Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
					Title ="Parsifal", ImageURL="http://localhost:61963/Content/images/uploads/1.jpg", CreatedBy = 1},
				new Event{ Id = 4, LayoutId = 1, Date = DateTime.Today.AddMonths(3).AddHours(20).AddMinutes(15),
					Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy",
					Title ="The Mastersingers of Nuremberg", ImageURL="http://localhost:61963/Content/images/uploads/4.jpg", CreatedBy = 1},
				new Event{ Id = 2, LayoutId = 3, Date = DateTime.Today.AddMonths(2).AddHours(20),
					Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy",
					Title ="The Mastersingers of Nuremberg", ImageURL="http://localhost:61963/Content/images/uploads/4.jpg"},
				new Event{ Id = 3, LayoutId = 5, Date = DateTime.Today.AddMonths(1).AddHours(19).AddMinutes(30),
					Description = "Solo-violin sonatas and partitas from Alina Ibragimova", Title ="Late-night Bach: Proms",
				ImageURL="http://localhost:61963/Content/images/uploads/2.jpg", CreatedBy = 1 }
			};
		}

		public static List<EventArea> EventAreas()
		{
			return new List<EventArea>
			{
				new EventArea{ Id = 1, EventId = 1, Description = "The area #1", CoordX = 1, CoordY = 1,
					AreaDefaultId = 1, Price = 145.25M },
					new EventArea{ Id = 2, EventId = 1, Description = "The area #2", CoordX = 1, CoordY = 2,
						AreaDefaultId = 3, Price = 45.7M},
					new EventArea{ Id = 3, EventId = 1, Description = "The area #3", CoordX = 1, CoordY = 1,
						AreaDefaultId = 5, Price = 180.25M},
					new EventArea{ Id = 9, EventId = 3, Description = "The first area", CoordX = 1, CoordY = 1,
						AreaDefaultId = 7, Price = 165.96M},
			};
		}

		public static List<EventSeat> EventSeats()
		{
			return new List<EventSeat>
			{
				new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = 0},
				new EventSeat { Id = 2, EventAreaId = 1, Number = 2, Row = 1, State = 0},
				new EventSeat { Id = 3, EventAreaId = 1, Number = 3, Row = 1, State = 0},
				new EventSeat { Id = 4, EventAreaId = 1, Number = 1, Row = 2, State = 0},
				new EventSeat { Id = 5, EventAreaId = 1, Number = 2, Row = 2, State = 1},
				new EventSeat { Id = 13, EventAreaId = 2, Number = 1, Row = 1, State = 1},
				new EventSeat { Id = 14, EventAreaId = 3, Number = 1, Row = 1, State = 0},
				new EventSeat { Id = 10, EventAreaId = 9, Number = 1, Row = 2, State = 0},
				new EventSeat { Id = 11, EventAreaId = 9, Number = 2, Row = 1, State = 0},
				new EventSeat { Id = 12, EventAreaId = 9, Number = 2, Row = 2, State = 0},
				new EventSeat { Id = 9, EventAreaId = 9, Number = 1, Row = 1, State = 0},
				new EventSeat { Id = 15, EventAreaId = 2, Number = 10, Row = 1, State =1},
				new EventSeat { Id = 16, EventAreaId = 1, Number = 14, Row = 3, State = 1}
			};
		}
	}
}
