using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewEntities
{
	public class VenueView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public List<LayoutView> LayoutList { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as VenueView;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				Name == entity.Name &&
				Description.Equals(entity.Description) &&
				Address.Equals(entity.Address) &&
				Phone.Equals(entity.Phone) &&
				LayoutList.SequenceEqual(entity.LayoutList))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
