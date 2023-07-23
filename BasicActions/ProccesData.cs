using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager__Businescope_.Models;

namespace TaskManager__Businescope_.BasicActions
{
    public class ProccesData
    {
        public static List<Process> GetProcessList()
        {
            return Process.GetProcesses().OrderBy(x=>x.ProcessName).ToList();
        }

        public static List<ProcessForDisplaying> GetProcessForDisplayingList(List<Process> processList)
        {
            List <ProcessForDisplaying> returnList = new List <ProcessForDisplaying>();

            foreach (Process process in processList)
            {
                var memorySize = Convert.ToDouble(process.WorkingSet64) / (1024 * 1024);
                memorySize = Math.Round(memorySize, 2);

                ProcessForDisplaying item = new ProcessForDisplaying(
                    process.Id,
                    process.ProcessName.ToString(),
                    memorySize,
                    process.Responding == true ? "Responding" : "Not responding");
                returnList.Add(item);
            }

            return returnList;
        }

        public static string GetCountOfProcess(List<Process> processList)
        {
                return processList.Count.ToString();
        }
        public static DataTable serializeProcessForDisplaying(List<ProcessForDisplaying> processes)
        {
            DataTable returnTable = new DataTable();
            returnTable.Columns.Add(new DataColumn("ID"));
            returnTable.Columns.Add(new DataColumn("Name"));
            returnTable.Columns.Add(new DataColumn("Memory MB")); 
            returnTable.Columns.Add(new DataColumn("Status"));
            foreach(var process in processes)
            {
                returnTable.AcceptChanges();
                DataRow row = returnTable.NewRow();
                row[0] = process.ProcessId;
                row[1] = process.ProcessName;
                row[2] = process.ProcessMemory;
                row[3] = process.IsResponding;
                returnTable.Rows.Add(row);
            }

            return returnTable;
        }

    }
}
