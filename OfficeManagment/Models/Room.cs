using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    public class Room : Resource
    {
        public string Name { get; set; }

        public int Rate { get; set; }
    }
}
