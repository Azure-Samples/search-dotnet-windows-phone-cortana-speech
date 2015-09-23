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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Media.SpeechRecognition;
using TrailFinder.Common;
using TrailFinder.DataModel;
using TrailFinder.Views;

namespace TrailFinder.ViewModels
{
    public class MainViewModel: BaseViewModel
    {
        private SpeechRecognizer SpeechRecognizer;

        #region Bindable properties

        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ObservableCollection<string> Facets { get; private set; }

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

        private string _SelectedFacet;
        public string SelectedFacet
        {
            get { return _SelectedFacet; }
            set { RaisePropertyChanged(ref _SelectedFacet, value); }
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

        private bool _AreFacetsVisible;
        public bool AreFacetsVisible
        {
            get { return _AreFacetsVisible; }
            set { RaisePropertyChanged(ref _AreFacetsVisible, value); }
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

        public ICommand RecognizeSpeechCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await SpeechRecognizer.CompileConstraintsAsync();
                    Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await SpeechRecognizer.RecognizeWithUIAsync();
                    SearchTerm = speechRecognitionResult.Text.Replace('.', ' ');
                });
            }
        }

        public ICommand LoadItemCommand
        {
            get { return new RelayCommand(() => Navigate(typeof(ItemPage), SelectedIndex)); }
        }

        public ICommand ShowFacetsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AreFacetsVisible = true;
                    Facets.Clear();
                    foreach (var facet in SearchService.Facets)
                    {
                        Facets.Add(facet.Value.ToString() + " (" + facet.Count + ")");
                    }
                });
            }
        }

        public ICommand FilterByFacetCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    AreFacetsVisible = false;
                    string facet = SelectedFacet.Split(' ')[0];
                    if (SearchType == "Trails near me")
                    {
                        await SearchTrailsNearMe(facet);
                    }
                    else 
                    {
                        await SearchTrails(facet);
                    }
                });
            }
        }

        #endregion

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            Facets = new ObservableCollection<string>();
            SpeechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();
            AreFacetsVisible = false;
        }

        private async Task SearchTrails(string filter = null)
        {
            SearchType = String.Format("Trails like {0}", SearchTerm);
            IsLoading = true;
            Items.Clear();
            var results = await SearchService.SearchAsync(SearchTerm, filter);
            AddSearchResults(results);
        }

        private async Task SearchTrailsNearMe(string filter = null)
        {
            SearchType = "Trails near me";
            IsLoading = true;
            Items.Clear();
            var position = await GetCurrentGeolocation();
            var results = await SearchService.GeoSearchAsync(position.Coordinate, filter);
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
