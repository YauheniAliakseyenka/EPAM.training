using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Layout
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Description { get; set; }
	}
}
