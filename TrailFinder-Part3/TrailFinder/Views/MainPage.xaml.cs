namespace TrailFinder.Views
{
    public sealed partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.MainViewModel; 
        }
    }
}
