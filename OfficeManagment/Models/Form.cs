﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Models
{
    public class Form : Collection<FormField>
    {
        public const string Relation = "form";
        public const string EditRelation = "edit-form";
        public const string CreateRelation = "create-form";
        public const string QueryRelation = "query-form";
    }
}
