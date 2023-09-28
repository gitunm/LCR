using LeftCenterRight.DataModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.ViewModel
{
    public abstract class BaseViewModel : NotifyPropertyChanged
    {
        public BaseViewModel(BaseViewModel? parent)
        {
            Parent = parent;
        }

        public BaseViewModel Parent { get; set; }

        public ILog Log { get; set; }
    }
}
