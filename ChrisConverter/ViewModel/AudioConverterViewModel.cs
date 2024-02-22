using ChrisConverter.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChrisConverter.ViewModel
{
    internal class AudioConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ConvertCommand { get; private set; }
        public ICommand BrowseInputCommand { get; private set; }

        private List<Audioextension> outputFormats;

        private string selectedInputFile;
        public string SelectedInputFile
        {
            get => selectedInputFile;
            set
            {
                selectedInputFile = value;
                OnPropertyChanged("SelectedInputFile");
                OnPropertyChanged("CanConvert");
            }
        }

        private string selectedOutputFormat;
        public string SelectedOutputFormat
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

        public bool CanConvert => !string.IsNullOrEmpty(SelectedInputFile) && !string.IsNullOrEmpty(SelectedOutputFormat);

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

        public AudioConverterViewModel()
        {
            ConvertCommand = new RelayCommand(ConvertAudioAsync);
            BrowseInputCommand = new RelayCommand(BrowseInput);
            this.OutputFormats = new List<Audioextension>{new Audioextension("mp3", "Format audio par défaut"),
                                                                              new Audioextension("aac", "format audio de apple") };
        }

        private async void ConvertAudioAsync()
        {
            try
            {
                IsConverting = true;
                ConversionMessage = "Conversion in progress...";

                // Replace with your actual conversion logic using libraries like NAudio or FFmpeg
                // This is just a placeholder
                await Task.Delay(2000); // Simulate conversion time

                ConversionMessage = "Conversion successful!";
            }
            catch (Exception ex)
            {
                ConversionMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsConverting = false;
            }
        }

        public void BrowseInput()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio files (*.mp3;*.aac)|*.mp3;*.aac|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedInputFile = openFileDialog.FileName;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
