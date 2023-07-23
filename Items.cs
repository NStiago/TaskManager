using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager__Businescope_
{
    public class Items
    {
        public Items(int id, string name, double memory, string isResponse)
        {
            ProcessId = id;
            Name=name;
            Memory=memory;
            IsResponse=isResponse;
        }
        public int ProcessId { get;set; }
        public string Name { get; set; }
        public double Memory { get; set; }
        public string IsResponse { get; set; }
    }
}
