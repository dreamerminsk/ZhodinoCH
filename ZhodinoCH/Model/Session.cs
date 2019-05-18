using System;
using System.Globalization;
using System.Net;
using ZhodinoCH.Utils;

namespace ZhodinoCH.Model
{
    public class Session : IEquatable<Session>
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
            return "[" + IPAddress.ToString() + "] - " + (DateTime.Now - Started).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }

        public override bool Equals(object other) => Equals(other as Session);

        public bool Equals(Session other)
        {
            if (this == other) return true;
            if (other == null) return false;
            return string.Equals(ID, other.ID, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (16777619 * hash) ^ (ID?.GetHashCode() ?? 0);
                return hash;
            }
        }

    }
}
