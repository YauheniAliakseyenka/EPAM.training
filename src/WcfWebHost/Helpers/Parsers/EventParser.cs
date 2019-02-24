using BusinessLogic.DTO;
using WcfWebHost.Contracts.Data;

namespace WcfWebHost.Helpers.Parsers
{
    internal class EventParser
    {
        public static Event ToEventContract(EventDto from)
        {
            return new Event
            {
                CreatedBy = from.CreatedBy,
                Date = from.Date,
                Description = from.Description,
                Id = from.Id,
                ImageURL = from.ImageURL,
                LayoutId = from.LayoutId,
                Title = from.Title
            };
        }

        public static EventDto ToEventDto(Event from)
        {
            return new EventDto
            {
                CreatedBy = from.CreatedBy,
                Date = from.Date,
                Description = from.Description,
                Id = from.Id,
                ImageURL = from.ImageURL,
                LayoutId = from.LayoutId,
                Title = from.Title
            };
        }
    }
}
