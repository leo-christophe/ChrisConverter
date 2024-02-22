using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisConverter.Services.FFmpeg
{
    class FFmpegFunctions
    {

        public static void RunFFmpeg(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe", // Assuming ffmpeg.exe is in the PATH or same directory as the application
                Arguments = arguments,
                CreateNoWindow = false, // This will prevent the FFmpeg console window from appearing
                UseShellExecute = false,
                RedirectStandardError = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
