using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {

        /// <summary>
        /// an extenstion method to avoid the warning when firing an async task without await
        /// from inside another async task
        /// for fire and forget scenarios where we don't need to wait for the task to complete
        /// http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
        /// </summary>
        /// <param name="task"></param>
        public static void Forget(this Task task)
        {

        }
    }
}
