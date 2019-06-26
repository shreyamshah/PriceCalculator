using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PriceCalculator.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class Tabs : StackLayout
    {
        public Tabs()
        {
            Orientation = StackOrientation.Horizontal;
            HeightRequest = 50;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Spacing = 0;
        }

        static StackLayout stack;

        public List<string> ItemSource
        {
            get { return (List<string>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public static readonly BindableProperty ItemSourceProperty =
            BindableProperty.Create(nameof(ItemSource), typeof(List<string>), typeof(Tabs), default(List<string>), BindingMode.TwoWay, propertyChanged: OnItemSourceChanged);

        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create("Position", typeof(string), typeof(Tabs), default(string), BindingMode.TwoWay, propertyChanged: OnSelectedPropertyChanged);


        public static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Tabs tab = bindable as Tabs;
            if (newValue != null)
            {
                tab.Children.Clear();
                List<string> tabs = newValue as List<string>;
                ScrollView scroll = new ScrollView() { Orientation = ScrollOrientation.Horizontal };
                stack = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 50, Spacing = 0 };
                scroll.Content = stack;
                tab.Children.Add(scroll);
                foreach (string item in tabs)
                {
                    Button button = new Button() { Text = item, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 20, HeightRequest = 50, BackgroundColor = Color.Transparent, };
                    //BoxView line = new BoxView() { WidthRequest = 1, Margin = new Thickness(5, 0, 0, 0), HeightRequest = label.Height, Color = Color.Black };

                    button.Pressed += (sender, e) =>
                         {
                             int index = tabs.IndexOf(tabs.Where((ex) => ex.Equals(item)).FirstOrDefault());
                             tab.Position = index.ToString();
                         };
                    stack.Children.Add(button);
                    //stack.Children.Add(line);
                }
                if (stack != null && stack.Children != null && stack.Children.Count > 0)
                {
                    (stack.Children[0] as Button).BackgroundColor = Color.Black;
                }
            }
        }

        public static void OnSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Tabs tab = bindable as Tabs;
            if(newValue != null)
            {
                try
                {
                    if(stack != null && stack.Children!=null && stack.Children.Count>0)
                    {
                        (stack.Children[int.Parse(newValue.ToString())] as Button).BackgroundColor = Color.Black;
                        if(oldValue != null)
                        (stack.Children[int.Parse(oldValue.ToString())] as Button).BackgroundColor = Color.Transparent;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
