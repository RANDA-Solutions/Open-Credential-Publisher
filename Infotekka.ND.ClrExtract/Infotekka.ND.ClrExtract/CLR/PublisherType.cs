﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infotekka.ND.ClrExtract.CLR
{
    public class PublisherType : IssuerType
    {
        [JsonProperty("hsx:School")]
        public SchoolType School { get; set; }
    }
}