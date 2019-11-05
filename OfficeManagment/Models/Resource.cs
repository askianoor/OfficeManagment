using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    public abstract class Resource
    {
        //Add JsonProperty line to ensure Href will be at the top of our Json File (Base On Self Documentation of Ion+json)
        [JsonProperty( Order = -2)]
        public string Href { get; set; }
    }
}
