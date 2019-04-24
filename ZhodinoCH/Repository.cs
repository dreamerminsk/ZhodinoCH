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

        public static List<Record> GetAll(string db)
        {
            var recs = new List<Record>();
            WebClient client = new WebClient();
            string response = client.DownloadString(new Uri("http://178.124.170.17:5984/" + db + "/_all_docs?include_docs=true"));
            JObject records = JObject.Parse(response);
            JArray rows = (JArray)records["rows"];
            foreach (var row in rows)
            {
                var doc = row["doc"];
                var rec = new Record(
                    (string)doc["_id"],
                    (string)doc["_rev"],
                    (string)doc["date"],
                    (string)doc["patient"],
                    (string)doc["tel"],
                    (string)doc["comment"]
                    );
                recs.Add(rec);
            }
            return recs;
        }

        public static void Update(string db, Record rec)
        {
            WebClient client = new WebClient();
            client.DownloadString(new Uri("http://178.124.170.17:5984/" + db + "/" + rec.ID + "/"));
            string str = "{ \"patient\": \"" + rec.Name + "\", " +
                "\"date\": \"" + rec.Date.ToShortDateString() + "\", " +
                "\"tel\": \"" + rec.Tel + "\", " +
                "\"comment\": \"" + rec.Comment + "\", " +
                "\"_rev\": \"" + rec.Rev + "\" }";
            var encoding = Encoding.GetEncoding("utf-8");
            byte[] arr = encoding.GetBytes(str);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://178.124.170.17:5984/" + db + "/" + rec.ID + "/");
            request.Method = "PUT";
            request.ContentType = "text/json";
            request.ContentLength = arr.Length;
            request.KeepAlive = true;
            var dataStream = request.GetRequestStream();
            dataStream.Write(arr, 0, arr.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
        }
    }
}
