using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackService.DTO
{
    public class SeanceDTO
    {
        public int Id { get; set; }
        public string Hall { get; set; }
        public DateTime BeginTime { get; set; }
        public int Duration { get; set; }
        public IEnumerable<PlaceDTO> Places { get; set; }
    }
}
