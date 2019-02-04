using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
	public class VenueDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Timezone { get; set; }
        public List<LayoutDto> LayoutList { get; set; }

		public string NameWithOffset
		{
			get
			{
				var timezone = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
				var builder = new StringBuilder(Name);
				builder.Append(" (UTC").Append(timezone.BaseUtcOffset < TimeSpan.Zero ? "-" : "+").Append(timezone.BaseUtcOffset.ToString("hh\\:mm"))
					.Append(")");

				return builder.ToString();
			}
		}

		public VenueDto()
		{
			LayoutList = new List<LayoutDto>();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is VenueDto entity))
				return false;

			if (Id == entity.Id &&
				Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase) &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				Address.Equals(entity.Address, StringComparison.OrdinalIgnoreCase) &&
				Phone.Equals(entity.Phone, StringComparison.OrdinalIgnoreCase) &&
				Timezone.Equals(entity.Timezone, StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, Name, Description, Address, Phone, Timezone).GetHashCode();
	}
}
