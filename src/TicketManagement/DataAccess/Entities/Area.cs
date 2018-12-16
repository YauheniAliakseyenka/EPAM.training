using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public string Description { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
	}
}
