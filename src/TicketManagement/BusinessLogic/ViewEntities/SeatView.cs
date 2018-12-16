using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class SeatView
	{
		public int Id { get; set; }
		public int AreaId { get; set; }
		public int Row { get; set; }
		public int Number { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as SeatView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				AreaId == entity.AreaId &&
				Row == entity.Row &&
				Number == entity.Number)
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
