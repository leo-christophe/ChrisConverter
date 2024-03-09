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

namespace ChrisConverter.ViewModel
{
    internal class AudioConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ConvertCommand { get; private set; }
        public ICommand BrowseInputCommand { get; private set; }

        // private Services.FFmpegInstaller FFmpegManager;
        private List<Audioextension> outputFormats;

        private string selectedInputFile;
        public string SelectedInputFile
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

        private Audioextension selectedOutputFormat;
        public Audioextension SelectedOutputFormat
        {
            get => selectedOutputFormat;
            set
            {
                selectedOutputFormat = value;
                OnPropertyChanged("SelectedOutputFormat");
                OnPropertyChanged("CanConvert");
            }
        }
        private double conversionProgress;
        public double ConversionProgress
        {
            get => conversionProgress;
            set
            {
                conversionProgress = value;
                OnPropertyChanged("ConversionProgress");
            }
        }
        private string conversionMessage;
        public string ConversionMessage
        {
            get => conversionMessage;
            set
            {
                conversionMessage = value;
                OnPropertyChanged("ConversionMessage");
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

        public bool CanConvert => !string.IsNullOrEmpty(SelectedInputFile) && !string.IsNullOrEmpty(SelectedOutputFormat.NomExtension);

        public List<Audioextension> OutputFormats
        {
            get
            {
                return this.outputFormats;
            }

            set
            {
                this.outputFormats = value;
                OnPropertyChanged("OutputFormats");
            }
        }

        /// <summary>
        /// Constructeur de la classe AudioConverterViewModel
        /// </summary>
        public AudioConverterViewModel()
        {
            // RelayCommands pour faire le binding
            ConvertCommand = new RelayCommand(ConvertAudioAsync);
            BrowseInputCommand = new RelayCommand(BrowseInput);

            // Liste des formats audios
            this.OutputFormats = new List<Audioextension>{new Audioextension(".mp3", "Format audio par défaut"),
                                                          new Audioextension(".aac", "format audio de apple"),
                                                          new Audioextension(".ogg", "format ogg"),
                                                          new Audioextension(".wav", "format video"),
                                                          new Audioextension(".flac", "format video"),
                                                          new Audioextension(".avi", ""),
                                                          new Audioextension(".aiff", "Apple format, better metadata."),
                                                          new Audioextension(".ac3", "Used in film and video production.")};
            // Gestion de FFmpeg
            // FFmpegManager = new FFmpegInstaller();
            // Vérification de l'installation de FFmpeg: FFmpegManager.InstallFFmpeg();
        }

        /// <summary>
        ///  Méthode asynchrone de conversion de fichier.
        /// </summary>
        private async void ConvertAudioAsync()
        {
            try
            {
                IsConverting = true;
                string outputFormat = this.SelectedOutputFormat.NomExtension;

                string outputFilePath = $"{System.IO.Path.ChangeExtension(this.SelectedInputFile, outputFormat)}";

             
                await Task.Run(() => Services.FFmpeg.FFmpegFunctions.RunFFmpeg($"-i \"{this.SelectedInputFile}\" \"{outputFilePath}\""));
                
                if (File.Exists(outputFilePath))
                {
                    var result = MessageBox.Show("La conversion a réussi !", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var result = MessageBox.Show("La conversion a échouée...", "Echec", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            openFileDialog.Multiselect = false;
            // openFileDialog.Multiselect = true; pour plusieurs fichiers à choisir.

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedInputFile = openFileDialog.FileName;
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
