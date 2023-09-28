using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.DataModel
{
    public class ChartDataModel : BaseDataModel
    {
        private ObservableCollection<DataPoint>? _coordinates;
        public ObservableCollection<DataPoint>? Coordinates
        {
            get => _coordinates ??= new ObservableCollection<DataPoint>();
            set
            {
                _coordinates = value;
                OnPropertyChanged();

                if (_coordinates != null)
                    AverageTurns = _coordinates.Average(c => c.Turns);
            }
        }

        private double _averageTurns;
        public double AverageTurns 
        {
            get => _averageTurns;
            set
            {
                _averageTurns = value;
                OnPropertyChanged();
            }
        }
    }
}
