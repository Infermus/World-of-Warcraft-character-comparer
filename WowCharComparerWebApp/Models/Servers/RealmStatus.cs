﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowCharComparerWebApp.Models.Servers
{
    /// <summary>
    /// Model using for hold data from json parse response
    /// </summary>
    public class RealmStatus
    {
        public List<Realm> Realms { get; set; }
    }
}