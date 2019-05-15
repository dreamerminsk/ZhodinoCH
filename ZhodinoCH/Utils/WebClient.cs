using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace ZhodinoCH.Utils
{
    public class WebClient
    {
        private const string SHORT_FORMAT = "yyyy-MM-dd";
        private const string LONG_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffffff";
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; rv:66.0) Gecko/20100101 Firefox/66.0";

        private static readonly HttpClient HttpClient;

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
                Task<string>.Factory.StartNew(async () => await DownloadStringAsync(Properties.Settings.Default.RemoteHost)),
                Task<string>.Factory.StartNew(async () => await DownloadStringAsync(Properties.Settings.Default.LocalHost))
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











        private static async Task<string> DownloadStringAsync(string uri)
        {
            using (WebClient webClient = new WebClient())
            {
                Console.WriteLine("DSA: " + uri);
                string text = "";
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["User-Agent"] = USER_AGENT;
                webClient.Headers["Authorization"] = BasicAuth();
                text = await webClient.DownloadStringTaskAsync(new Uri(uri)).ConfigureAwait(false);
                Console.WriteLine("DSA: " + text);
                return text;
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
