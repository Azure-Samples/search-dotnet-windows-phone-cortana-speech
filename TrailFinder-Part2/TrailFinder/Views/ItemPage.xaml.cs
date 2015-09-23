using TrailFinder.Common;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace TrailFinder.Views
{
    public sealed partial class ItemPage : BasePage
    {
        public ItemPage()
        {
            InitializeComponent();
            NavigationHelper.LoadState += NavigationHelper_LoadState;
        }

        void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var index = e.NavigationParameter as int?;

            if (index != null)
            {
                var item = App.MainViewModel.Items[index.Value];
                DataContext = item;

                MapControl.MapElements.Clear();
                MapControl.Center = item.GeoPoint;
                MapIcon mapIcon = new MapIcon
                {
                    Location = item.GeoPoint,
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    Title = item.Feature_Name
                };
                MapControl.MapElements.Add(mapIcon);
            }
        }
    }
}