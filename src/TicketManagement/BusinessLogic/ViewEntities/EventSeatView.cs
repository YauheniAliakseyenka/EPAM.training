using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class EventSeatView
	{
		public int Id { get; set; }
		public int EventAreaId { get; set; }
		public int Row { get; set; }
		public int Number { get; set; }
		public int State { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as EventSeatView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				EventAreaId == entity.EventAreaId &&
				Row == entity.Row &&
				Number == entity.Number &&
				State == entity.State)
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
