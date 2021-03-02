using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Models
{
    public class DataModel
    {
        public Culture[] CultureData { get; set; }
        public Event[] Events { get; set; }
        public System[] Systems { get; set; }
        public PioneerGroup[] PioneerGroups { get; set; }
    }
}
