//Copyright Microsoft. All rights reserved.
//Licensed under the MIT License at http://mit-license.org/

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

// Data provided by U.S. Geological Survey - Department of the Interior/USGS
// http://geonames.usgs.gov/domestic/download_data.htm

using Windows.Devices.Geolocation;
using Microsoft.Spatial;

namespace TrailFinder.ViewModels
{
    public class ItemViewModel : BaseViewModel
    {
        private string _Feature_Name;
        public string Feature_Name
        {
            get { return _Feature_Name; }
            set { RaisePropertyChanged(ref _Feature_Name, value); }
        }

        private string _Feature_Class;
        public string Feature_Class
        {
            get { return _Feature_Class; }
            set { RaisePropertyChanged(ref _Feature_Class, value); }
        }

        private string _State_Alpha;
        public string State_Alpha
        {
            get { return _State_Alpha; }
            set { RaisePropertyChanged(ref _State_Alpha, value); }
        }

        private string _County_Name;
        public string County_Name
        {
            get { return _County_Name; }
            set { RaisePropertyChanged(ref _County_Name, value); }
        }

        // You'll need to install the AzureSearch nuget package for this to compile
        // Install-Package Microsoft.Azure.Search -Pre
        private GeographyPoint _Location;
        public GeographyPoint Location
        {
            get { return _Location; }
            set { RaisePropertyChanged(ref _Location, value); }
        }
        
        public Geopoint GeoPoint
        {
            get { return new Geopoint(new BasicGeoposition() {Latitude = Location.Latitude, Longitude = Location.Longitude}); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { RaisePropertyChanged(ref _Description, value); }
        }

        private string _Map_Name;
        public string Map_Name
        {
            get { return _Map_Name; }
            set { RaisePropertyChanged(ref _Map_Name, value); }
        }

        private string _Elev_in_ft;
        public string Elev_in_ft
        {
            get { return _Elev_in_ft; }
            set { RaisePropertyChanged(ref _Elev_in_ft, value); }
        }
    }
}
