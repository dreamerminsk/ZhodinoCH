using System;
using System.Net;
using ZhodinoCH.Utils;

namespace ZhodinoCH.Model
{
    public class Session
    {

        public string ID { get; set; }
        public string Rev { get; set; }
        public string User { get; set; }
        public IPAddress IPAddress { get; set; }
        public DateTime Started { get; set; }

        public Session()
        {
            ID = PushIDGenerator.GeneratePushId();
            User = Properties.Settings.Default.DefaultUser;
            IPAddress = NetUtils.LocalIPAddress();
            Started = DateTime.Now;
        }

        public override string ToString()
        {
            return "[" + IPAddress.ToString() + "] - " + (DateTime.Now - Started);
        }

    }
}
