﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ZhodinoCH.Utils;

namespace ZhodinoCH.Model
{
    public class Session
    {

        public string ID { get; set; }
        public string User { get; set; }
        public IPAddress IPAddress { get; private set; }
        public DateTime Started { get; set; }

        public Session()
        {
            ID = PushIDGenerator.GeneratePushId();
            User = Properties.Settings.Default.DefaultUser;
            IPAddress = NetUtils.LocalIPAddress();
            Started = DateTime.UtcNow;
        }

    }
}