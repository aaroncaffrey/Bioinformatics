using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.TaskManagement
{
    public class WorkDivision<T> : WorkDivision
    {
        public new List<Task<T>> TaskList = new List<Task<T>>();

        public WorkDivision(int itemsToProcess, int threadCount = -1) : base(itemsToProcess, threadCount)
        {
            
        }

        public WorkDivision(int itemsPerThread, int itemsToProcess, int threadCount) : base(itemsPerThread, itemsToProcess, threadCount)
        {
            
        }

        public new void WaitAllTasks()
        {
            try
            {
                Task[] tasksToWait = TaskList.Where(t => t != null && !t.IsCompleted).ToArray<Task>();
                if (tasksToWait.Length > 0)
                {
                    Task.WaitAll(tasksToWait);
                }
            }
            catch (AggregateException)
            {
            }
            RecordFinishTicks();
        }
    }
}
