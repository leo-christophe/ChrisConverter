using System;
using System.IO;
using System.Net;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace ChrisConverter.Services
{
    public class FFmpegInstaller
    {
        private const string FFmpegDownloadUrl = "https://ffmpeg.org/releases/ffmpeg-6.1.1.tar.xz";
        private const string FFmpegInstallDirectory = @"C:\ffmpeg\";

        public void InstallFFmpeg()
        {
            if (!IsFFmpegInstalled())
            {
                Console.WriteLine("En train d'installer le package FFmpeg...");
                DownloadAndExtractFFmpeg();
                UpdateSystemPath();
            }
            else
            {
                Console.WriteLine("FFmpeg est déjà installé, bonne utilisation!");
            }
        }

        private void DownloadAndExtractFFmpeg()
        {
            using (var client = new WebClient())
            {
                Directory.CreateDirectory(FFmpegInstallDirectory);
                string downloadedFilePath = Path.Combine(FFmpegInstallDirectory, "ffmpeg.tar.xz");
                string extractedDirectoryPath = Path.Combine(FFmpegInstallDirectory, "ffmpeg");

                // Télécharger le fichier tar.xz
                client.DownloadFile(FFmpegDownloadUrl, downloadedFilePath);

                // Extraire le contenu du fichier tar.xz
                using (var archive = ArchiveFactory.Open(downloadedFilePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(extractedDirectoryPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }

                // Supprimer le fichier téléchargé
                File.Delete(downloadedFilePath);
            }
        }

        public bool IsFFmpegInstalled()
        {
            return Directory.Exists(FFmpegInstallDirectory);
        }

        private void UpdateSystemPath()
        {
            string pathVariable = Environment.GetEnvironmentVariable("PATH") ?? "";
            if (!pathVariable.Contains(FFmpegInstallDirectory))
            {
                Environment.SetEnvironmentVariable("PATH", $"{pathVariable};{FFmpegInstallDirectory}", EnvironmentVariableTarget.Machine);
            }
        }
    }
}
