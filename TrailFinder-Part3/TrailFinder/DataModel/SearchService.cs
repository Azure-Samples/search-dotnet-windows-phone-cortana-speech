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
        private const string FacetName = "COUNTY_NAME";
        private const string BaseFilter = "FEATURE_CLASS eq 'Trail'";
        public static List<FacetResult> Facets;

        public static string ServiceName;
        public static string IndexName;
        public static string QueryKey;

        public static async Task<IEnumerable<ItemViewModel>> SearchAsync(string searchString, string filter = null)
        {
            var searchParameters = new SearchParameters()
            {
                Filter = CreateFilter(filter),
                Facets = new[]
                {
                    FacetName
                }
            };
            return await DoSearchAsync(searchString, searchParameters);
        }

        public static async Task<IEnumerable<ItemViewModel>> GeoSearchAsync(Geocoordinate coordinate, string filter = null)
        {
            var position = coordinate.Point.Position;
            var orderByFilter = $"geo.distance(LOCATION, geography'POINT({position.Longitude} {position.Latitude})')";
                       

            var searchParameters = new SearchParameters()
            {
                Filter = CreateFilter(filter),
                OrderBy = new[]
                {
                    orderByFilter
                },
                Facets = new[]
                {
                    FacetName
                }
            };

            return await DoSearchAsync("*", searchParameters);
        }

        private static string CreateFilter(string filter)
        {
            var searchFilter = BaseFilter;
            if (!String.IsNullOrEmpty(filter))
            {
                searchFilter = $"{searchFilter} and {FacetName} eq '{filter}'";
            }
            return searchFilter;
        }

        private static async Task<IEnumerable<ItemViewModel>> DoSearchAsync(string searchString, SearchParameters parameters)
        {
            List<ItemViewModel> searchResults = new List<ItemViewModel>();

            using (var indexClient = new SearchIndexClient(ServiceName, IndexName, new SearchCredentials(QueryKey)))
            {
                var response = await indexClient.Documents.SearchAsync<ItemViewModel>(searchString, parameters);
                searchResults.AddRange(response.Results.Select(result => result.Document));
                Facets = response.Facets[FacetName].ToList();
            }
            return searchResults;
        }
    }
}
