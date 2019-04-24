using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhodinoCH
{
    public class Record
    {
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Comment { get; set; }
        

        public Record()
        {
            ID = Repository.GetID();
            Date = DateTime.Now;
        }
    }


}
