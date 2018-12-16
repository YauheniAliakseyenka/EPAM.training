using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class EventArea
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public decimal Price { get; set; }
        public int AreaDefaultId { get; set; }	
	}
}
