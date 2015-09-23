namespace TrailFinder.Views
{
    public sealed partial class ResultsPage : BasePage
    {
        public ResultsPage()
        {
            InitializeComponent();
            DataContext = App.MainViewModel; 
        }

        protected override void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            if (App.MainViewModel.AreFacetsVisible == true)
            {
                App.MainViewModel.AreFacetsVisible = false;
                e.Cancel = true;
            }
            base.OnNavigatingFrom(e);
        }

    }
}
