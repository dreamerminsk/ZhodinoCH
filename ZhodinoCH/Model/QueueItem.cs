using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using ZhodinoCH.Utils;

namespace ZhodinoCH.Model
{
    public class QueueItem
    {
        public string ID { get; set; }
        public string Rev { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }


        public QueueItem()
        {
            ID = PushIDGenerator.GeneratePushId();
            Rev = "";
            Date = DateTime.Now;
            Name = "";
            Tel = "";
            Comment = "";
            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public QueueItem(String ID, String rev, String date, String name, String tel, String comment)
        {
            this.ID = ID;
            this.Rev = rev;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
            this.Name = name;
            this.Tel = tel;
            this.Comment = comment;
        }
    }


}
