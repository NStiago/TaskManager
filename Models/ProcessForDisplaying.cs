using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager__Businescope_.Models
{
    public class ProcessForDisplaying
    {
        public ProcessForDisplaying(int processId, string processName, double processMemory, string isResponding)
        {
            ProcessId = processId;
            ProcessName = processName;
            ProcessMemory = processMemory;
            IsResponding = isResponding;
        }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public double ProcessMemory { get; set; }
        public string IsResponding { get; set; } = string.Empty;

    }
}
