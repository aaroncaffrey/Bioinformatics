using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;

namespace BioinformaticsHelperLibrary.TaskManagement
{
    public class WorkDivision
    {
        public long FinishTicks;
        public int ItemsCompleted;
        public int ItemsPerThread;
        public int ItemsToProcess;
        public long StartTicks;
        public List<Task> TaskList = new List<Task>();
        public int ThreadCount;
        public int[] ThreadFirstIndex;
        public int[] ThreadLastIndex;
        private readonly object _itemsCompletedLock = new object();

        /// <summary>
        ///     Calculate the number of items each task thread should process.  This assumes an average workload per item.
        /// </summary>
        /// <param name="itemsToProcess">The number of items which need to be processed</param>
        /// <param name="threadCount">The number of threads desired. Set to -1 for one thread per core.</param>
        /// <returns></returns>
        public WorkDivision(int itemsToProcess, int threadCount = -1) 
        {
            if (threadCount <= 0)
            {
                threadCount = Environment.ProcessorCount;
            }

            int itemsPerThread = DivRoundUp(itemsToProcess, threadCount);

            for (int threadIndex = threadCount - 1; threadIndex > -1; threadIndex--)
            {
                if (itemsPerThread * threadIndex >= itemsToProcess)
                {
                    threadCount--;
                }
                else
                {
                    break;
                }
            }

            if (itemsPerThread <= 0 && itemsToProcess > 0)
            {
                //itemsPerThread = 1;
            }

            //return new WorkDivision<T>(itemsPerThread, itemsToProcess, threadCount);

            InitProperties(itemsPerThread, itemsToProcess, threadCount);
        }

        public WorkDivision(int itemsPerThread, int itemsToProcess, int threadCount)
        {
            InitProperties(itemsPerThread, itemsToProcess, threadCount);
        }

        public void IncrementItemsCompleted(int value)
        {
            lock (_itemsCompletedLock)
            {
                ItemsCompleted += value;
            }
        }

        private void InitProperties(int itemsPerThread, int itemsToProcess, int threadCount)
        {
            ItemsPerThread = itemsPerThread;
            ItemsToProcess = itemsToProcess;
            ThreadCount = threadCount;
            ItemsCompleted = 0;
            FinishTicks = -1;

            ThreadFirstIndex = new int[ThreadCount];
            ThreadLastIndex = new int[ThreadCount];

            for (int threadIndex = 0; threadIndex < ThreadCount; threadIndex++)
            {
                ThreadFirstIndex[threadIndex] = ThreadStartIndex(threadIndex);
                ThreadLastIndex[threadIndex] = ThreadFinishIndex(threadIndex);
            }

            RecordStartTicks();            
        }

        /// <summary>
        ///     Modified version of Eric Lippert's DivRoundUp method from
        ///     http://ericlippert.com/2013/01/28/integer-division-that-rounds-up/
        /// </summary>
        /// <param name="dividend">The dividend</param>
        /// <param name="divisor">the divisor</param>
        /// <returns>Returns the dividend divided by the divisor, always rounded up to the nearest whole number.</returns>
        internal static int DivRoundUp(int dividend, int divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }

            if (divisor == -1 && dividend == int.MinValue)
            {
                throw new OverflowException();
            }

            int roundedTowardsZeroQuotient = dividend / divisor;

            bool dividedEvenly = (dividend % divisor) == 0;

            if (dividedEvenly)
            {
                return roundedTowardsZeroQuotient;
            }

            bool wasRoundedDown = ((divisor > 0) == (dividend > 0));

            if (wasRoundedDown)
            {
                return roundedTowardsZeroQuotient + 1;
            }
            return roundedTowardsZeroQuotient;
        }

        public void RecordStartTicks()
        {
            StartTicks = DateTime.Now.Ticks;
        }

        public void RecordFinishTicks()
        {
            FinishTicks = DateTime.Now.Ticks;
        }

        private int ThreadStartIndex(int threadNumber)
        {
            int startIndex = threadNumber * ItemsPerThread;

            if (startIndex > 0 && startIndex >= ItemsToProcess)
            {
                startIndex = ItemsToProcess - 1;
            }

            if (threadNumber > 0 && startIndex == ItemsToProcess - 1)
            {
                if (ThreadFinishIndex(threadNumber - 1) == startIndex || ThreadFinishIndex(threadNumber - 1) == -1)
                {
                    startIndex = -1;
                }
            }

            return startIndex;
        }

        private int ThreadFinishIndex(int threadNumber)
        {
            int finishIndex = ((threadNumber + 1) * ItemsPerThread) - 1;

            if (finishIndex > 0 && finishIndex >= ItemsToProcess)
            {
                finishIndex = ItemsToProcess - 1;
            }

            if (threadNumber > 0 && finishIndex == ItemsToProcess - 1)
            {
                if (ThreadFinishIndex(threadNumber - 1) == finishIndex || ThreadFinishIndex(threadNumber - 1) == -1)
                {
                    finishIndex = -1;
                }
            }

            return finishIndex;
        }

        public void WaitAllTasks()
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
                throw;
            }
            RecordFinishTicks();
        }
    }
}
