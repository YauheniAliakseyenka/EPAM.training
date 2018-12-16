using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class EventAreaView 
	{
		public int Id { get; set; }
		public int EventId { get; set; }
		public string Description { get; set; }
		public int CoordX { get; set; }
		public int CoordY { get; set; }
		public decimal Price { get; set; }
		public int AreaDefaultId { get; set; }
		public List<int> EventSeatList { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as EventAreaView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				EventId == entity.EventId &&
				Description.Equals(entity.Description) &&
				CoordX == entity.CoordX &&
				CoordY == entity.CoordY &&
				Price.Equals(entity.Price) &&
				AreaDefaultId == entity.AreaDefaultId &&
				EventSeatList.SequenceEqual(entity.EventSeatList))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
