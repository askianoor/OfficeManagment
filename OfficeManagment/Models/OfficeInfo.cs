﻿using Newtonsoft.Json;
using OfficeManagment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    public class OfficeInfo : Resource, IEtaggable
    {
        public string Title { get; set; }

        public string Tagline { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string PhoneNumber { get; set; }

        public Address Location { get; set; }

        public string GetEtag()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return Md5Hash.ForString(serialized);
        }
    }
}
