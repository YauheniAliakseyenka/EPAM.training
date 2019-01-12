using System;
using System.Collections.Generic;

namespace BusinessLogic.DTO
{
	public class VenueDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public List<LayoutDto> LayoutList { get; set; }

		public VenueDto()
		{
			LayoutList = new List<LayoutDto>();
		}

		public override bool Equals(object obj)
		{
			var entity = obj as VenueDto;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase) &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				Address.Equals(entity.Address, StringComparison.OrdinalIgnoreCase) &&
				Phone.Equals(entity.Phone, StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, Name, Description, Address, Phone).GetHashCode();
	}
}
