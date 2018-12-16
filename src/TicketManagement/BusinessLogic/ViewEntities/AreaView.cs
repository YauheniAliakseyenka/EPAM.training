using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class AreaView 
	{
		public int Id { get; set; }
		public int LayoutId { get; set; }
		public string Description { get; set; }
		public int CoordX { get; set; }
		public int CoordY { get; set; }
		public List<SeatView> SeatList { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as AreaView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				LayoutId == entity.LayoutId &&
				Description.Equals(entity.Description) &&
				CoordX == entity.CoordX &&
				CoordY == entity.CoordY &&
				SeatList.SequenceEqual(entity.SeatList))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
