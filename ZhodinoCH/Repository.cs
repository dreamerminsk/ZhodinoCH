using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZhodinoCH.Model;

namespace ZhodinoCH
{
    public static class Repository
    {
        public static string CurrentHost { get; set; }

        public static void CheckHost()
        {
            if (string.IsNullOrEmpty(CurrentHost))
            {
                CurrentHost = GetActiveHost();
            }
        }

        public static string GetActiveHost()
        {
            var tasks = new List<Task<string>>()
            {
                Task<string>.Factory.StartNew( () => DownloadString(Properties.Settings.Default.RemoteHost)),
                Task<string>.Factory.StartNew( () => DownloadString(Properties.Settings.Default.LocalHost))
            };
            Console.WriteLine("STATIC: " + tasks.Count);
            var taskIndex = Task<string>.WaitAny(tasks.ToArray());
            switch(taskIndex)
            {
                case 0:
                    return Properties.Settings.Default.RemoteHost;
                case 1:
                    return Properties.Settings.Default.LocalHost;
                default:
                    return "127.0.0.1:5984";
            }
        }

        public static Record Get(string db, string id)
        {
            CheckHost();
            string response = DownloadString(CurrentHost + "/" + db + "/" + id);
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
            CheckHost();
            var recs = new List<Record>();
            string response = DownloadString(CurrentHost + "/" + db + "/_all_docs?include_docs=true");
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
            CheckHost();
            Console.WriteLine("GetAllAsync(" + db + ")");
            var recs = new List<Record>();
            string response = await DownloadStringAsync(CurrentHost + "/" + db + "/_all_docs?include_docs=true").ConfigureAwait(false);
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
            Console.WriteLine(uri);
            using (WebClient webClient = new WebClient())
            {
                Console.WriteLine(uri);
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
                Console.WriteLine("DSA: " + uri);
                string text = "";
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["User-Agent"] = "Mozilla";
                webClient.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("editor:111"));
                text = await webClient.DownloadStringTaskAsync(new Uri(uri)).ConfigureAwait(false);
                Console.WriteLine("DSA: " + text);
                return text;
            }
        }

        public static void Insert(string db, Record rec)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "patient", rec.Name },
                { "date", rec.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) },
                { "tel", rec.Tel },
                { "comment", rec.Comment }
            };
            PutReq(CurrentHost + "/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static void Update(string db, Record rec)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "patient", rec.Name },
                { "date", rec.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) },
                { "tel", rec.Tel },
                { "comment", rec.Comment },
                { "_rev", rec.Rev }
            };
            PutReq(CurrentHost + "/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static void InsertUser(string user, string password)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "name", user },
                { "password", password },
                { "type", "user" },
                { "roles", new JArray() }
            };
            var res = PutReq(CurrentHost + "/_users/org.couchdb.user:" + user, jsonobj);
            Console.WriteLine(res);
        }

        public static void InsertSecurity(string db, string user)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "admins", new JObject() },
                { "members", new JObject
            {
                { "names", new JArray
            {
                user
            }},
                { "roles", new JArray() }
            }}
            };
            var res = PutReq(CurrentHost + "/" + db + "/_security", jsonobj);
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
