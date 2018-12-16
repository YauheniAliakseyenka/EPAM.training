using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class LayoutView
	{
		public int Id { get; set; }
		public int VenueId { get; set; }
		public string Description { get; set; }
		public List<AreaView> AreaList { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as LayoutView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				VenueId == entity.VenueId &&
				Description.Equals(entity.Description) &&
				AreaList.SequenceEqual(entity.AreaList))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
