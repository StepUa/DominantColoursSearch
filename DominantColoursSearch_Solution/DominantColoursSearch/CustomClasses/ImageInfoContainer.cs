using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominantColoursSearch.CustomClasses
{
    public class ImageInfoContainer
    {
        public ImageInfoContainer(string fullPathToImage, Size imageResolution)
        {
            if (!File.Exists(fullPathToImage))
            {
                this.IsInitialized = false;

                return;
            }

            this.PathToFile = fullPathToImage;
            this.Name = Path.GetFileNameWithoutExtension(this.PathToFile);
            this.Extension = Path.GetExtension(this.PathToFile);
            this.ResolutionText = imageResolution.Width.ToString() + "x" + imageResolution.Height.ToString();

            this.IsInitialized = true;
        }

        public string PathToFile { get; private set; }
        public string Name { get; private set; }
        public string ResolutionText { get; private set; }
        public string Extension { get; private set; }
        public bool IsInitialized { get; private set; }
    }
}
