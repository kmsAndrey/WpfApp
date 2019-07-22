using AksenovNewTeleTeth.Models;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksenovNewTeleTeth.Models
{
    public class Progressbar : ViewModelBase
    {
        private bool _work = false;
        public bool Work
        {
            get { return _work; }
            set
            {
                if(_work!=value)
                {
                    _work = value;
                    RaisePropertiesChanged("Work");
                }
            }
        }
    }
}
