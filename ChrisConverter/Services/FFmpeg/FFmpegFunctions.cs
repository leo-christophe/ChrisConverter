using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ChrisConverter.Services.FFmpeg
{
    class FFmpegFunctions
    {
        public static async Task RunFFmpegAsync(string arguments)
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

                    Task<string> errorOutputTask = process.StandardError.ReadToEndAsync();
                    Task processTask = process.WaitForExitAsync();

                    await Task.WhenAll(errorOutputTask, processTask);

                    string errorOutput = await errorOutputTask;

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
                        System.Windows.MessageBox.Show("Conversion terminée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
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
