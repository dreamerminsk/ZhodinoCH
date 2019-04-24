using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ZhodinoCH
{
    class Repository
    {

        public static string GetID()
        {
            WebClient client = new WebClient();
            string response = client.DownloadString(new Uri("http://178.124.170.17:5984/_uuids"));
            JObject uuids = JObject.Parse(response);
            JToken uuid = uuids["uuids"][0];
            return (string)uuid;
        }
    }
}
