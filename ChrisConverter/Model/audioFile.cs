using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisConverter.Model
{
    internal class AudioFile
    {
        private string? fileName;
        private Audioextension? fileExtension;
        private TimeSpan? fileLength;
        private double? fileSize;

        public AudioFile(string? fileName, Audioextension? fileExtension, TimeSpan? fileLength, double? fileSize)
        {
            this.FileName = fileName;
            this.FileExtension = fileExtension;
            this.FileLength = fileLength;
            this.FileSize = fileSize;
        }

        public AudioFile(string? fileName, Audioextension? fileExtension)
        {
            this.FileName = fileName;
            this.FileExtension = fileExtension;
        }

        public AudioFile(string? fileName)
        {
            this.FileName = fileName;
            this.FileExtension = new Audioextension("mp3", "Extension audio par défaut.");
        }

        public string? FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public Audioextension? FileExtension
        {
            get
            {
                return fileExtension;
            }

            set
            {
                fileExtension = value;
            }
        }

        public TimeSpan? FileLength
        {
            get
            {
                return fileLength;
            }

            set
            {
                fileLength = value;
            }
        }

        public double? FileSize
        {
            get
            {
                return this.fileSize;
            }

            set
            {
                this.fileSize = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return this.FileName== ((AudioFile)obj).FileName && this.FileExtension == ((AudioFile)obj).FileExtension && this.FileLength == ((AudioFile)obj).FileLength && this.FileSize == ((AudioFile)obj).FileSize;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return $"Nom du dossier: {this.FileName}, Extension: {this.FileExtension}, Durée: {this.FileLength}, Taille (En Go): {this.FileSize}";
        }
    }
}
