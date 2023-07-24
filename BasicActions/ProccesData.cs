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
    //вспомогательный класс с методами, для работы с данными о процессе
    public class ProccesData
    {
        #region 
        //блок методов для кастомной сортировки
        public static List<Process> GetSortetProcessListById()
        {
            return Process.GetProcesses().OrderBy(x=>x.Id).ToList();
        }
        public static List<Process> GetSortetProcessListByName()
        {
            return Process.GetProcesses().OrderBy(x => x.ProcessName).ToList();
        }
        public static List<Process> GetSortetProcessListByMemory()
        {
            return Process.GetProcesses().OrderByDescending(x => x.WorkingSet64).ToList();
        }
        public static List<Process> GetSortetProcessListByRespose()
        {
            return Process.GetProcesses().OrderBy(x => x.Responding).ToList();
        }
        #endregion

        //получение списка экемпляров класса для отображения на главной форме
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

        //получение количества процессов
        public static string GetCountOfProcess(List<Process> processList)
        {
                return processList.Count.ToString();
        }

        //формирование данных для processDataGridView
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
