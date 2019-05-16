using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ZhodinoCH.Utils
{
    public class WebClient
    {

        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; rv:66.0) Gecko/20100101 Firefox/66.0";

        private static readonly HttpClient HttpClient = new HttpClient();

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
            var taskIndex = Task<string>.WaitAny(
                new List<Task<string>>()
            {
                DownloadStringAsync(Properties.Settings.Default.RemoteHost),
                DownloadStringAsync(Properties.Settings.Default.LocalHost)
            }.ToArray());
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

        public static async Task<string> DownloadStringAsync(string uri)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(uri);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", USER_AGENT);

            HttpResponseMessage response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var json = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
                return json;
            }
            else
            {
                throw new Exception("HttpClient Exception: " + response.StatusCode);
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
