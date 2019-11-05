using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    public class ApiError
    {
        public string Message { get; set; }

        public string Detail { get; set; }


        //Ignoring NULL values of StackTrace to be add to the error handeling response
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string StackTrace { get; set; }
    }
}
