using LeftCenterRight.DataModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {
        public PlayerViewModel(BaseViewModel parent, ILog log)
            : base(parent)
        {
            Log = log;
        }

        private ObservableCollection<Player> _players;
        public ObservableCollection<Player> Players
        {
            get => _players;
            set
            {
                _players = value;
                OnPropertyChanged();
            }
        }
    }
}
