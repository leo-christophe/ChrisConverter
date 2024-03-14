using ChrisConverter.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ChrisConverter.Services;
using System.IO;
using System.Security.Policy;
using ChrisConverter.DBAccess;

namespace ChrisConverter.ViewModel
{
    internal class AudioConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ConvertCommand { get; private set; }
        public ICommand BrowseInputCommand { get; private set; }

        // private Services.FFmpegInstaller FFmpegManager;
        private List<Audioextension>? outputFormats;




        private string? selectedInputFile;
        public string? SelectedInputFile
        {
            get => selectedInputFile;
            set
            {
                selectedInputFile = value;
                // Notification de propriété pour le fichier sélectionné
                OnPropertyChanged("SelectedInputFile");
                // Notification de propriété pour la variable "Peut Convertir"
                OnPropertyChanged("CanConvert");
            }
        }

        private Audioextension? selectedOutputFormat;
        public Audioextension? SelectedOutputFormat
        {
            get => selectedOutputFormat;
            set
            {
                selectedOutputFormat = value;
                OnPropertyChanged("SelectedOutputFormat");
                OnPropertyChanged("CanConvert");
            }
        }
        private double? conversionProgress;
        public double? ConversionProgress
        {
            get => conversionProgress;
            set
            {
                conversionProgress = value;
                OnPropertyChanged("ConversionProgress");
            }
        }

        private bool isConverting;
        public bool IsConverting
        {
            get => isConverting;
            set
            {
                isConverting = value;
                OnPropertyChanged("IsConverting");
            }
        }

        public bool CanConvert
        {
            get
            {
                return FilesToConvert != null && FilesToConvert.Any() &&
                       SelectedOutputFormat != null && !string.IsNullOrEmpty(SelectedOutputFormat.NomExtension);
            }
        }



        public List<Audioextension>? OutputFormats
        {
            get
            {
                return this.outputFormats;
            }

            set
            {
                this.outputFormats = value;
                OnPropertyChanged("OutputFormats");
                OnPropertyChanged("CanConvert");
            }
        }

        private List<AudioFile>? filesToConvert;
        public List<AudioFile>? FilesToConvert
        {
            get
            {
                return this.filesToConvert;
            }

            set
            {
                this.filesToConvert = value;
                OnPropertyChanged("FilesToConvert");
                OnPropertyChanged("CanConvert");
            }
        }

        /// <summary>
        /// Constructeur de la classe AudioConverterViewModel
        /// </summary>
        public AudioConverterViewModel()
        {
            // RelayCommands pour faire le binding
            ConvertCommand = new AsyncRelayCommand(ConvertAudioAsync);
            BrowseInputCommand = new RelayCommand(BrowseInput);

            var app = (App)Application.Current;
            var ServiceProvider = app.ServiceProvider;
            var extensionsDB = (ExtensionsDB)(ServiceProvider.GetService(typeof(ExtensionsDB)));


            // Liste des formats audios
            this.OutputFormats = extensionsDB.GET_extensions();

            // Liste des fichiers à convertir initialisée.
            FilesToConvert = new List<AudioFile>();

            // Gestion de FFmpeg
            // FFmpegManager = new FFmpegInstaller();
            // Vérification de l'installation de FFmpeg: FFmpegManager.InstallFFmpeg();
        }

        public async Task ConvertAudioAsync()
        {
            try
            {
                
                IsConverting = true;
                string outputFormat = this.SelectedOutputFormat.NomExtension;

                if (FilesToConvert != null) 
                {
                    foreach (var fileToConvert in FilesToConvert)
                    {
                        string inputFilePath = fileToConvert.FileName;
                        string outputFilePath = $"{System.IO.Path.ChangeExtension(inputFilePath, outputFormat)}";
                        
                        await Services.FFmpeg.FFmpegFunctions.RunFFmpegAsync($"-i \"{inputFilePath}\" \"{outputFilePath}\"");

                        if (!File.Exists(outputFilePath))
                        {
                            MessageBox.Show("La conversion a échouée...", "Echec", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Aucun fichier à convertir n'a été sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("La conversion a réussi !", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Une erreur s'est produite lors de la conversion.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsConverting = false;
            }
        }





        /// <summary>
        ///  Méthode permettant de parcourir les fichiers pour en choisir un. Les fichiers valides sont d'extensions .mp3, .aac, .ogg. 
        /// </summary>
        public void BrowseInput()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Ajout des filtres selon les formats possibles
            string filter = "";
            foreach(Audioextension format in OutputFormats)
            {
                filter += "*" + format.NomExtension;
                // on ajoute un point virgule entre chaque si jamais pour séparer (.mp3;.ogg;.aac)
                if (! (this.OutputFormats.IndexOf(format) == this.OutputFormats.Count() - 1))
                {
                    filter += ";";
                }
            }
            openFileDialog.Filter = "Audio files (" + filter + ")|" + filter;


            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            // openFileDialog.Multiselect = true; pour plusieurs fichiers à choisir.

            if (openFileDialog.ShowDialog() == true)
            {
                for (int i = 0; i<openFileDialog.FileNames.Count(); i++) {
                    this.FilesToConvert.Add(new AudioFile(openFileDialog.FileNames[i]));
                }
            }
            
        }

        /// <summary>
        /// Méthode permettant de notifier d'un changement de propriété.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
