using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PriceCalculator.Data;
using Xamarin.Forms;

namespace PriceCalculator.CustomControls
{
    class AdvancedListView : ListView
    {
        public AdvancedListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            ItemAppearing += AdvancedListView_ItemAppearing;
            ItemsSource = new ObservableCollection<Cell>();
            //hasMore = true;
        }
        private void AdvancedListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            Entity en = e.Item as Entity;
            ObservableCollection<Entity> data = new ObservableCollection<Entity>(ItemsSource.Cast<Entity>());
            //throw new NotImplementedException();
            if (data?.LastOrDefault()?.Id == en?.Id)
            {
                GetData?.Execute(data.Count); ;
            }
        }

        ~AdvancedListView()
        {
            ItemAppearing -= AdvancedListView_ItemAppearing;
        }

        public ICommand GetData
        {
            get { return (ICommand)GetValue(GetDataProperty); }
            set { SetValue(GetDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GetData.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty GetDataProperty =
            BindableProperty.Create("GetData", typeof(ICommand), typeof(AdvancedListView), null, BindingMode.TwoWay);



        public int? Position
        {
            get { return (int?)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        //private bool hasMore;

        // Using a BindingProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create("Position", typeof(int?), typeof(AdvancedListView), null, BindingMode.TwoWay, propertyChanged: onPositionChanged);

        public static void onPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (((int?)newValue).HasValue)
            {
                AdvancedListView view = bindable as AdvancedListView;
                if (view != null && view.GetData != null)
                    view.GetData?.Execute(0);
            }
        }
    }
}
