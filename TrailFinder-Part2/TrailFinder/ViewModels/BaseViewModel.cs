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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrailFinder.Common;

namespace TrailFinder.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public NavigationHelper NavigationHelper;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                NotifyPropertyChanged(propertyName);
            }
        }

        public virtual void Navigate(Type page)
        {
            App.RootFrame.Navigate(page);
        }

        public virtual void Navigate(Type page, Object parameter)
        {
            App.RootFrame.Navigate(page, parameter);
        }
    }
}
