using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public static Record Get(string db, string id)
        {
            var recs = new List<Record>();
            string response = DownloadString("http://178.124.170.17:5984/" + db + "/" + id);
            JObject record = JObject.Parse((response));
            var rec = new Record(
                (string)record["_id"],
                (string)record["_rev"],
                (string)record["date"],
                (string)record["patient"],
                (string)record["tel"],
                (string)record["comment"]
                );
            return rec;
        }

        public static List<Record> GetAll(string db)
        {
            var recs = new List<Record>();
            string response = DownloadString("http://178.124.170.17:5984/" + db + "/_all_docs?include_docs=true");
            JObject records = JObject.Parse((response));
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

        public static async Task<List<Record>> GetAllAsync(string db)
        {
            var recs = new List<Record>();
            string response = await DownloadStringAsync("http://178.124.170.17:5984/" + db + "/_all_docs?include_docs=true");
            JObject records = JObject.Parse((response));
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

        private static string DownloadString(string uri)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                string response = webClient.DownloadString(new Uri(uri));
                return response;
            }
        }

        private static Task<string> DownloadStringAsync(string uri)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                return Task<string>.Factory.StartNew(() => { return webClient.DownloadString(new Uri(uri)); });
            }
        }

        public static void Insert(string db, Record rec)
        {
            JObject jsonobj = new JObject();
            jsonobj.Add("patient", rec.Name);
            jsonobj.Add("date", rec.Date.ToShortDateString());
            jsonobj.Add("tel", rec.Tel);
            jsonobj.Add("comment", rec.Comment);
            putReq("http://178.124.170.17:5984/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static void Update(string db, Record rec)
        {
            JObject jsonobj = new JObject();
            jsonobj.Add("patient", rec.Name);
            jsonobj.Add("date", rec.Date.ToShortDateString());
            jsonobj.Add("tel", rec.Tel);
            jsonobj.Add("comment", rec.Comment);
            jsonobj.Add("_rev", rec.Rev);
            putReq("http://178.124.170.17:5984/" + db + "/" + rec.ID + "/", jsonobj);
        }

        private static void putReq(string url, JObject jsonobj)
        {
            var encoding = Encoding.GetEncoding("utf-8");
            byte[] arr = encoding.GetBytes(jsonobj.ToString());
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = arr.Length;
            request.KeepAlive = false;
            var dataStream = request.GetRequestStream();
            dataStream.Write(arr, 0, arr.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            response.Close();
        }
    }
}
