using System;
using System.Diagnostics;

namespace Brainiac.Util
{
    /// <summary>
    /// A utility class to help run processes.
    /// </summary>
    public static class ProcessUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executable"></param>
        /// <param name="arguments"></param>
        public static void RunCommand(string executable, string arguments)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = executable,
                Arguments = arguments
            };

            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit(Convert.ToInt32(TimeSpan.FromSeconds(2).TotalMilliseconds));
        }
    }
}
