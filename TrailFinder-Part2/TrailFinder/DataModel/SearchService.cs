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
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Microsoft.Azure.Search.Models;
using TrailFinder.ViewModels;
using Microsoft.Azure.Search;

namespace TrailFinder.DataModel
{
    public class SearchService
    {
        private const string ServiceName = <your service name>;
        private const string IndexName = <your index name>;
        private const string QueryKey = <your query key>;

        public static async Task<IEnumerable<ItemViewModel>> SearchAsync(string searchString)
        {
            var searchParameters = new SearchParameters()
            {
                Filter = "FEATURE_CLASS eq 'Trail'"
            };
            return await DoSearchAsync(searchString, searchParameters);
        }

        public static async Task<IEnumerable<ItemViewModel>> GeoSearchAsync(Geocoordinate coordinate)
        {
            var position = coordinate.Point.Position;
            var orderByFilter = String.Format("geo.distance(LOCATION, geography'POINT({0} {1})')", position.Longitude, position.Latitude);

            var searchParameters = new SearchParameters()
            {
                Filter = "FEATURE_CLASS eq 'Trail'",
                OrderBy = new[]
                {
                    orderByFilter
                }
            };

            return await DoSearchAsync("*", searchParameters);
        }

        private static async Task<IEnumerable<ItemViewModel>> DoSearchAsync(string searchString, SearchParameters parameters)
        {
            List<ItemViewModel> searchResults = new List<ItemViewModel>();

            using (var indexClient = new SearchIndexClient(ServiceName, IndexName, new SearchCredentials(QueryKey)))
            {
                var response = await indexClient.Documents.SearchAsync<ItemViewModel>(searchString, parameters);
                searchResults.AddRange(response.Results.Select(result => result.Document));
            }
            return searchResults;
        }
    }
}
