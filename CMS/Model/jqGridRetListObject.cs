using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class jqGridRetListObject
    {
        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }

        public object rows { get; set; }
    }
}
