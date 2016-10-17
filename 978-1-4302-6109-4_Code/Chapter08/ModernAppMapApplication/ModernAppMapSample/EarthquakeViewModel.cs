using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ModernAppMapSample
{
    public class EarthquakeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Earthquake> _earthquakes;
        public ObservableCollection<Earthquake> Earthquakes
        {
            get { return _earthquakes; }
            set
            {
                _earthquakes = value;
                OnPropertyChanged("Earthquakes");
            }
        }

        public EarthquakeViewModel()
        {
            USGSEarthquakeService.GetRecentEarthquakes((o, ea) =>
            {
                Earthquakes = new ObservableCollection<Earthquake>(ea.Locations);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
