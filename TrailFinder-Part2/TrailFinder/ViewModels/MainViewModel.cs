//Copyright Microsoft. All rights reserved.
//Licensed under the MIT License at http://mit-license.org/

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

// Data provided by U.S. Geological Survey - Department of the Interior/USGS
// http://geonames.usgs.gov/domestic/download_data.htm

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using TrailFinder.Common;
using TrailFinder.DataModel;
using TrailFinder.Views;

namespace TrailFinder.ViewModels
{
    public class MainViewModel: BaseViewModel
    {

        #region Bindable properties

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _SearchType;
        public string SearchType
        {
            get { return _SearchType; }
            set { RaisePropertyChanged(ref _SearchType, value); }
        }
        

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { RaisePropertyChanged(ref _SelectedIndex, value); }
        }
        

        private string _SearchTerm;
        public string SearchTerm
        {
            get { return _SearchTerm; }
            set { RaisePropertyChanged(ref _SearchTerm, value); }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { RaisePropertyChanged(ref _IsLoading, value); }
        }

        #endregion

        #region Bindable Commands
        public ICommand SearchTrailsCommand
        {
            get { 
                return new RelayCommand(async () => 
                { 
                    Navigate(typeof(ResultsPage));
                    await SearchTrails();
                }); 
            }
        }

        public ICommand SearchNearMeCommand
        {
            get { 
                return new RelayCommand(async () => 
                { 
                    Navigate(typeof(ResultsPage));
                    await SearchTrailsNearMe();
                }); 
            }
        }

        public ICommand LoadItemCommand
        {
            get { return new RelayCommand(() => Navigate(typeof(ItemPage), SelectedIndex)); }
        }

        #endregion

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
        }

        private async Task SearchTrails()
        {
            IsLoading = true;
            SearchType = $"Trails like {SearchTerm}";
            var results = await SearchService.SearchAsync(SearchTerm);
            AddSearchResults(results);
        }

        private async Task SearchTrailsNearMe()
        {

            IsLoading = true;
            SearchType = "Trails near me";
            var position = await GetCurrentGeolocation();
            var results = await SearchService.GeoSearchAsync(position.Coordinate);
            AddSearchResults(results);
        }

        private void AddSearchResults(IEnumerable<ItemViewModel> results)
        {
            Items.Clear();
            foreach (var result in results)
            {
                Items.Add(result);
            }
            IsLoading = false;
        }

        private async Task<Geoposition> GetCurrentGeolocation()
        {
            try
            {
                var geolocator = new Geolocator();
                return await geolocator.GetGeopositionAsync();
            }
            catch (UnauthorizedAccessException)
            {
                // the app does not have the right capability or the location master switch is off
                return null;
            }
        }
    }
}
