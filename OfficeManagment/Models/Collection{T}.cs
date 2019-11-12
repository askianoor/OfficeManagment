using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    //Define a class for Ion Collection Type
    public class Collection<T> : Resource
    {
        public T[] value { get; set; }
    }
}
