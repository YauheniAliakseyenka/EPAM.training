using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class EventView 
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int LayoutId { get; set; }
		public DateTime Date { get; set; }
		public List<int> EventAreaList { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as EventView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				Name.Equals(entity.Name) &&
				Description.Equals(entity.Description) &&
				LayoutId == entity.LayoutId &&
				Date.Equals(entity.Date) &&
				EventAreaList.SequenceEqual(entity.EventAreaList))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
