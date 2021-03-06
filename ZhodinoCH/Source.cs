﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhodinoCH.Model;

namespace ZhodinoCH
{
    public static class Source
    {

        private static readonly Mutex mutex = new Mutex();

        private const string LONG_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffffff";
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; rv:66.0) Gecko/20100101 Firefox/66.0";
        private const string SHORT_FORMAT = "yyyy-MM-dd";

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
            switch (taskIndex)
            {
                case 0:
                    return Properties.Settings.Default.RemoteHost;
                case 1:
                    return Properties.Settings.Default.LocalHost;
                default:
                    return "127.0.0.1:5984";
            }
        }

        public static QueueItem Get(string db, string id)
        {
            CheckHost();
            string response = DownloadString(CurrentHost + "/" + db + "/" + id);
            JObject record = JObject.Parse((response));
            var rec = new QueueItem(
                (string)record["_id"],
                (string)record["_rev"],
                (string)record["date"],
                (string)record["patient"],
                (string)record["tel"],
                (string)record["comment"]
                );
            return rec;
        }

        public static List<QueueItem> GetAll(string db)
        {
            CheckHost();
            var recs = new List<QueueItem>();
            string response = DownloadString(CurrentHost + "/" + db + "/_all_docs?include_docs=true");
            JObject records = JObject.Parse((response));
            JArray rows = (JArray)records["rows"];
            foreach (var row in rows)
            {
                var doc = row["doc"];
                var rec = new QueueItem(
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

        public static async Task<List<QueueItem>> GetAllAsync(string db)
        {
            CheckHost();
            Console.WriteLine("GetAllAsync(" + db + ")");
            var recs = new List<QueueItem>();
            try
            {
                string response = await Utils.WebClient.DownloadStringAsync(CurrentHost + "/" + db + "/_all_docs?include_docs=true").ConfigureAwait(false);
                Console.WriteLine(response);
                JObject records = JObject.Parse((response));
                JArray rows = (JArray)records["rows"];
                foreach (var row in rows)
                {
                    var doc = row["doc"];
                    var rec = new QueueItem(
                        (string)doc["_id"],
                        (string)doc["_rev"],
                        (string)doc["date"],
                        (string)doc["patient"],
                        (string)doc["tel"],
                        (string)doc["comment"]
                        );
                    recs.Add(rec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return recs;
        }

        public static void Insert(string db, QueueItem rec)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "patient", rec.Name },
                { "date", rec.Date.ToString(SHORT_FORMAT, CultureInfo.InvariantCulture) },
                { "tel", rec.Tel },
                { "comment", rec.Comment },
                { "created", rec.Created.ToString(LONG_FORMAT, CultureInfo.InvariantCulture) },
                { "last_modified", rec.LastModified.ToString(LONG_FORMAT, CultureInfo.InvariantCulture)}
            };
            PutReq(CurrentHost + "/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static void Update(string db, QueueItem rec)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "patient", rec.Name },
                { "date", rec.Date.ToString(SHORT_FORMAT, CultureInfo.InvariantCulture) },
                { "tel", rec.Tel },
                { "comment", rec.Comment },
                { "created", rec.Created.ToString(LONG_FORMAT, CultureInfo.InvariantCulture) },
                { "last_modified", DateTime.Now.ToString(LONG_FORMAT, CultureInfo.InvariantCulture) },
                { "_rev", rec.Rev }
            };
            PutReq(CurrentHost + "/" + db + "/" + rec.ID + "/", jsonobj);
        }

        public static async Task<List<Session>> GetAllSessionAsync()
        {
            CheckHost();
            var recs = new List<Session>();
            try
            {
                string response = await Utils.WebClient.DownloadStringAsync(CurrentHost + "/sessions/_all_docs?include_docs=true").ConfigureAwait(false);
                Console.WriteLine(response);
                JObject records = JObject.Parse((response));
                JArray rows = (JArray)records["rows"];
                foreach (var row in rows)
                {
                    var doc = row["doc"];
                    var rec = new Session();
                    rec.ID = (string)doc["_id"];
                    rec.Rev = (string)doc["_rev"];
                    rec.User = (string)doc["user"];
                    rec.IPAddress = IPAddress.Parse((string)doc["ip"]);
                    try
                    {
                        rec.Started = DateTime.Parse((string)doc["started"], CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        rec.Started = DateTime.Now;
                        //throw;
                    }
                    recs.Add(rec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return recs;
        }

        public static Session GetSession(string id)
        {
            CheckHost();
            string response = DownloadString(CurrentHost + "/sessions/" + id);
            JObject record = JObject.Parse((response));
            var rec = new Session();
            rec.ID = (string)record["_id"];
            rec.Rev = (string)record["_rev"];
            rec.User = (string)record["user"];
            rec.IPAddress = IPAddress.Parse((string)record["ip"]);
            try
            {
                rec.Started = DateTime.Parse((string)record["started"], CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                rec.Started = DateTime.Now;
                //throw;
            }

            return rec;
        }

        public static void InsertSession(Session session)
        {
            CheckHost();
            JObject jsonobj = new JObject
            {
                { "user", session.User },
                { "ip", session.IPAddress.ToString() },
                { "started", session.Started.ToString(LONG_FORMAT, CultureInfo.InvariantCulture) }
            };
            var res = PutReq(CurrentHost + "/sessions/" + session.ID, jsonobj);
            Console.WriteLine(res);
        }

        public static void DeleteSession(Session session)
        {
            CheckHost();
            var res = DelReq(CurrentHost + "/sessions/" + session.ID, session.Rev);
            Console.WriteLine(res);
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

        private static string DownloadString(string uri)
        {
            Console.WriteLine(uri);
            using (WebClient webClient = new WebClient())
            {
                Console.WriteLine(uri);
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["User-Agent"] = USER_AGENT;
                webClient.Headers["Authorization"] = BasicAuth();
                string response = webClient.DownloadString(new Uri(uri));
                return response;
            }
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
            request.UserAgent = USER_AGENT;
            request.Headers.Add("Authorization", BasicAuth());
            var dataStream = request.GetRequestStream();
            dataStream.Write(arr, 0, arr.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            response.Close();
            return returnString;
        }

        private static string DelReq(string url, string rev)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "DELETE";
            request.KeepAlive = false;
            request.UserAgent = USER_AGENT;
            request.Headers.Add("Authorization", BasicAuth());
            request.Headers.Add("If-Match", rev);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            response.Close();
            return returnString;
        }

        private static string BasicAuth()
        {
            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("editor:111"));
        }
    }
}