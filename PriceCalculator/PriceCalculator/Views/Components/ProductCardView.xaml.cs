using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriceCalculator.Views.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCardView : ContentView
    {
        public ProductCardView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        ~ProductCardView()
        {

        }
        Product product;
        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            try
            {
                product = BindingContext as Product;
               /* if (product != null)
                {
                    if (product != null && !string.IsNullOrEmpty(product.ImgName))
                    {
                        string imgPath = Xamarin.Forms.DependencyService.Get<IImageHelper>().GetCompressImagePath();
                        img.Source = imgPath + "/" + product.ImgName;
                    }
                    //img.Source = ImageSource.FromStream(() => stream);
                    //img.Source = product.ImgName;
                }
                else
                    img.Source = null;*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw new Exception();
            }
        }
    }
}