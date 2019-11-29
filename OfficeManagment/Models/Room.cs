using OfficeManagment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    public class Room : Resource
    {
        [Sortable]
        [Searchable]
        public string Name { get; set; }


        [Sortable(Default =true)]
        [SearchableDecimal]
        public int Rate { get; set; }


        public Form Book { get; set; }
    }
}
