using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
    }
}
