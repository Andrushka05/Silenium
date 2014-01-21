using System.Diagnostics;

namespace ParserHelpers
{
    public class ProxySearch
    {
        /// <summary>
        /// Проверяет открытые порты (80,443,1080,1081,3128,8080)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetOpenProxyPorts(string ip)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = @"D:\Projects\Parser\Parser\bin\Debug\nmap-6.40\nmap.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = " -sS -vv -PN -open -n -p1080,8080,3128,443,80,1081 --max-rtt-timeout 1000ms " + ip + " -T4",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            string str = string.Empty;
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (var exeProcess = Process.Start(startInfo))
            {
                str = exeProcess.StandardOutput.ReadToEnd();
                var dsa = exeProcess.StandardError.ReadToEnd();
                exeProcess.WaitForExit();
            }

            return str;
        }

        /// <summary>
        /// Получить все открытые порты с указанного ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetOpenPorts(string ip)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = @"D:\Projects\Parser\Parser\bin\Debug\nmap-6.40\nmap.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = " -sS -vv -PN -open -n -p 1-65535 --max-rtt-timeout 1000ms " + ip + " -T5",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            string str = string.Empty;
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (var exeProcess = Process.Start(startInfo))
            {
                str = exeProcess.StandardOutput.ReadToEnd();
                var dsa = exeProcess.StandardError.ReadToEnd();
                exeProcess.WaitForExit();
            }
            return str;
        }
    }
}
