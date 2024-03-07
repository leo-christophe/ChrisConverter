using System;
using System.Diagnostics;
using System.Windows;

namespace ChrisConverter.Services.FFmpeg
{
    class FFmpegFunctions
    {
        public static void RunFFmpeg(string arguments)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Users\leoch\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg-6.1.1-full_build\bin\ffmpeg.exe",
                    Arguments = arguments,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardError = true
                };

                using (Process process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string errorOutput = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    if (!string.IsNullOrEmpty(errorOutput))
                    {
                        Console.WriteLine("Erreur lors de l'exécution de FFmpeg :\n" + errorOutput + "Erreur");
                    }
                    else
                    {
                        MessageBox.Show("Conversion terminée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de l'exécution de FFmpeg :\n" + ex.Message);
            }
        }

    }
}
