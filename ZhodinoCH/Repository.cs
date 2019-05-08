using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZhodinoCH
{
    class Repository
    {

        private static readonly string CURRENT_HOST = Properties.Settings.Default.RemoteHost;

        public static string GetID()
        {
            WebClient client = new WebClient();
            string response = client.DownloadString(new Uri(CURRENT_HOST + "/_uuids"));
            JObject uuids = JObject.Parse(response);
            JToken uuid = uuids["uuids"][0];
            return (string)uuid;
        }

        public static Record Get(string db, string id)
        {
            string response = DownloadString(CURRENT_HOST + "/" + db + "/" + id);
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
            string response = DownloadString(CURRENT_HOST + "/" + db + "/_all_docs?include_docs=true");
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
            Console.WriteLine("GetAllAsync(" + db + ")");
            var recs = new List<Record>();
            string response = await DownloadStringAsync(CURRENT_HOST + "/" + db + "/_all_docs?include_docs=true").ConfigureAwait(false);
            Console.WriteLine(response);
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
                webClient.Headers["User-Agent"] = "Mozilla";
                webClient.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("editor:111"));
                string response = webClient.DownloadString(new Uri(uri));
                return response;
            }
        }

        private static async Task<string> DownloadStringAsync(string uri)
        {
            using (WebClient webClient = new WebClient())
            {
                string text = "";
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["User-Agent"] = "Mozilla";
                webClient.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("editor:111"));
                text = await webClient.DownloadStringTaskAsync(new Uri(uri)).ConfigureAwait(false);
                return text;
            }
        }

        public static void Insert(string db, Record rec)
        {
            JObject jsonobj = new JObject
            {
                { "patient", rec.Name },
                { "date", rec.Date.ToShortDateString() },
                { "tel", rec.Tel },
                { "comment", rec.Comment }
            };
            PutReq(CURRENT_HOST + "/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static void Update(string db, Record rec)
        {
            JObject jsonobj = new JObject
            {
                { "patient", rec.Name },
                { "date", rec.Date.ToShortDateString() },
                { "tel", rec.Tel },
                { "comment", rec.Comment },
                { "_rev", rec.Rev }
            };
            PutReq(CURRENT_HOST + "/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static void InsertUser(string user, string password)
        {
            JObject jsonobj = new JObject
            {
                { "name", user },
                { "password", password },
                { "type", "user" },
                { "roles", new JArray() }
            };
            var res = PutReq(CURRENT_HOST + "/_users/org.couchdb.user:" + user, jsonobj);
            Console.WriteLine(res);
        }

        public static void InsertSecurity(string db, string user)
        {
            var names = new JArray();
            names.Add(user);
            var members = new JObject
            {
                { "names", names },
                { "roles", new JArray() }
            };
            JObject jsonobj = new JObject
            {
                { "admins", new JObject() },
                { "members", members}
            };
            var res = PutReq(CURRENT_HOST + "/" + db + "/_security", jsonobj);
            Console.WriteLine(res);
        }

        private static string PutReq(string url, JObject jsonobj)
        {
            var encoding = Encoding.GetEncoding("utf-8");
            byte[] arr = encoding.GetBytes(jsonobj.ToString());
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = arr.Length;
            request.KeepAlive = false;
            request.UserAgent = "Mozilla";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("editor:111")));
            var dataStream = request.GetRequestStream();
            dataStream.Write(arr, 0, arr.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            response.Close();
            return returnString;
        }
    }
}
