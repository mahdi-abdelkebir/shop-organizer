using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project__Making_Life_Easier
{
    [Serializable]
    public class LogItem 
    {

        public string Name;
        public List<string> Rows;
        public LogItem() { }

        public LogItem(string _name, List<string> _rows)
        {
            Name = _name;
            Rows = _rows;
        }
    }
}
