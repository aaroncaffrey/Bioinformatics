using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.TaskManagement
{
    public class TaskMethods
    {
        /// <summary>
        ///     This method removes null or completed tasks from a list of tasks.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static List<Task> RemoveCompletedTasks(List<Task> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            for (int taskIndex = tasks.Count - 1; taskIndex >= 0; taskIndex--)
            {
                if (tasks[taskIndex] != null && tasks[taskIndex].IsCompleted)
                {
                    tasks[taskIndex].Dispose();
                    tasks.RemoveAt(taskIndex);
                }
                else if (tasks[taskIndex] == null)
                {
                    tasks.RemoveAt(taskIndex);
                }
            }

            return tasks;
        }

        /// <summary>
        ///     This method removes null or completed tasks from a list of tasks.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static List<Task<T>> RemoveCompletedTasks<T>(List<Task<T>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            for (int taskIndex = tasks.Count - 1; taskIndex >= 0; taskIndex--)
            {
                if (tasks[taskIndex] != null && tasks[taskIndex].IsCompleted)
                {
                    tasks[taskIndex].Dispose();
                    tasks.RemoveAt(taskIndex);
                }
                else if (tasks[taskIndex] == null)
                {
                    tasks.RemoveAt(taskIndex);
                }
            }

            return tasks;
        }

        /// <summary>
        ///     This method removes null or completed tasks from an array of tasks.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task[] RemoveCompletedTasks(Task[] tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            return RemoveCompletedTasks(tasks.ToList()).ToArray();
        }


        /// <summary>
        ///     This method removes null or completed tasks from an array of tasks.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task<T>[] RemoveCompletedTasks<T>(Task<T>[] tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            return RemoveCompletedTasks(tasks.ToList()).ToArray();
        }
    }
}