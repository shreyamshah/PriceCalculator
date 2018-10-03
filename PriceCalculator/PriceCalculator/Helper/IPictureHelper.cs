using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PriceCalculator.Helper
{
    public interface IPictureHelper
    {
        void SavePicture(string filename, ImageSource imageData);
    }
}
