using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhodinoCH
{
    public class Record
    {

        private DateTime date;
        private string name;
        private string tel;
        private string comment;

        public DateTime Date { get => date; set => date = value; }
        public string Name { get => name; set => name = value; }
        public string Tel { get => tel; set => tel = value; }
        public string Comment { get => comment; set => comment = value; }

        public Record()
        {
            date = DateTime.Now;
        }
    }

    
}
