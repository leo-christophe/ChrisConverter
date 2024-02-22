using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisConverter.Model
{
    class Audioextension
    {
        private string? nomExtension;
        private string? descriptionExtension;

        public Audioextension(string? extension, string? description)
        {
            this.NomExtension = extension;
            this.DescriptionExtension = description;
        }

        public string? NomExtension
        {
            get
            {
                return nomExtension;
            }

            set
            {
                nomExtension = value;
            }
        }

        public string? DescriptionExtension
        {
            get
            {
                return this.descriptionExtension;
            }

            set
            {
                this.descriptionExtension = value;
            }
        }
    }
}
