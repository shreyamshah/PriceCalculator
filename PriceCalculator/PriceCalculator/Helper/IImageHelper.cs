using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PriceCalculator.Helper
{
    public interface IImageHelper
    {
        Task<string> ResizeImage(string imageArray);
        string GetCompressImagePath();
    }
}
