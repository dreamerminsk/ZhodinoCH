using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ZhodinoCH.Model
{
    public class Record
    {
        public string ID { get; set; }
        public string Rev { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Comment { get; set; }
        

        public Record()
        {
            ID = Repository.GetID();
            Rev = "";
            Date = DateTime.Now;
            Name = "";
            Tel = "";
            Comment = "";
        }

        public Record(String ID, String rev, String date, String name, String tel, String comment)
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
