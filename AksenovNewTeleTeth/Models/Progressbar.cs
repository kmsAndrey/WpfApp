using AksenovNewTeleTeth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksenovNewTeleTeth.Models
{
    public class Progressbar : Base
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
                    OnPropertyChanged("Work");
                }
            }
        }
    }
}
