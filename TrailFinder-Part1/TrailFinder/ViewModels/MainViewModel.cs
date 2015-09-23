//Copyright Microsoft. All rights reserved.
//Licensed under the MIT License at http://mit-license.org/

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

// Data provided by U.S. Geological Survey - Department of the Interior/USGS
// http://geonames.usgs.gov/domestic/download_data.htm


using System.Collections.ObjectModel;
using System.Windows.Input;
using TrailFinder.Common;
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

        public ICommand LoadItemCommand
        {
            get { return new RelayCommand(() => Navigate(typeof(ItemPage), SelectedIndex)); }
        }

        #endregion

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
        }
    }
}
