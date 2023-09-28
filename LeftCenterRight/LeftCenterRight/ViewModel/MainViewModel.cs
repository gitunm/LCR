using log4net;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(ILog log)
            : base(null)
        {
            Log = log;
        }


        private SimulatorInputViewModel? _simulatorInputVM;
        public SimulatorInputViewModel? SimulatorInputVM => _simulatorInputVM ??= new SimulatorInputViewModel(this, Log);

        private ChartViewModel? _chartVM;
        public ChartViewModel? ChartVM => _chartVM ??= new ChartViewModel(this, Log);

        private PlayerViewModel? _playerVM;
        public PlayerViewModel? PlayerVM => _playerVM ??= new PlayerViewModel(this, Log);

        public enum Die
        {
            Left, Center, Right, Dot1, Dot2, Dot3 
        }
    }
}
