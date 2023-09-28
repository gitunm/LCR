using LeftCenterRight.DataModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.ViewModel
{
    public class ChartViewModel : BaseViewModel
    {
        public ChartViewModel(BaseViewModel parent, ILog log)
            : base(parent)
        {
            Log = log;
        }

        private ChartDataModel? _chartDM;
        public ChartDataModel? ChartDM => _chartDM ??= new ChartDataModel();
    }
}
