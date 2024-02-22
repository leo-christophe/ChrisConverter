using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChrisConverter.Services
{
    public class FFmpegInstaller
    {
        private const string FFmpegDownloadUrl = "https://ffmpeg.org/download.html";
        private const string FFmpegZipFilePath = "ffmpeg.zip";
        private const string FFmpegInstallDirectory = @"C:\ffmpeg\";

        /// <summary>
        /// Méthode static permettant d'installer FFmpeg, servant à convertir des fichiers. Si il est déjà installé, on laisse sinon on l'installe.
        /// </summary>
        public void InstallFFmpeg()
        {
           
            if (!IsFFmpegInstalled())
            {
                MessageBox.Show("En train d'installer le package FFmpeg...");
                DownloadFFmpeg();
                ExtractFFmpeg();
                UpdateSystemPath();
            }
            else
            {
                MessageBox.Show("FFmpeg est déjà installé, bonne utilisation!");
            }
        }

        private static void DownloadFFmpeg()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(FFmpegDownloadUrl, FFmpegZipFilePath);
            }
        }

        public static bool IsFFmpegInstalled()
        {
            // Check if FFmpeg executable exists in a known installation directory
            string ffmpegDirectory = @"C:\ffmpeg";
            string ffmpegExecutable = "ffmpeg.exe";
            string ffmpegFullPath = Path.Combine(ffmpegDirectory, ffmpegExecutable);

            if (File.Exists(ffmpegFullPath))
            {
                return true;
            }

            // Alternatively, you could check if FFmpeg is available in the system PATH
            // This allows for FFmpeg to be installed in any directory included in the PATH environment variable
            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(pathVariable))
            {
                string[] paths = pathVariable.Split(';');
                foreach (string path in paths)
                {
                    string ffmpegPathInPath = Path.Combine(path, ffmpegExecutable);
                    if (File.Exists(ffmpegPathInPath))
                    {
                        return true;
                    }
                }
            }

            // FFmpeg is not found
            return false;
        }

        private static void ExtractFFmpeg()
        {
            ZipFile.ExtractToDirectory(FFmpegZipFilePath, FFmpegInstallDirectory);
            File.Delete(FFmpegZipFilePath);
        }

        private static void UpdateSystemPath()
        {
            string pathVariable = Environment.GetEnvironmentVariable("PATH") ?? "";
            if (!pathVariable.Contains(FFmpegInstallDirectory))
            {
                Environment.SetEnvironmentVariable("PATH", $"{pathVariable};{FFmpegInstallDirectory}", EnvironmentVariableTarget.Machine);
            }
        }
    }
}
