using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DevExpress.Mvvm;

namespace AksenovNewTeleTeth.Models
{
    public class MainObject: ViewModelBase
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                    _Id = value;
                RaisePropertiesChanged("Id");
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                    _Date = value;
                RaisePropertiesChanged("Date");
            }
        }

        private PointObject _PointObjectA;

        public PointObject PointObjectA
        {
            get { return _PointObjectA; }
            set
            {
                if (_PointObjectA != value)
                    _PointObjectA = value;
                RaisePropertiesChanged("PointObjectA");
            }
        }

        private string _Direction;
        public string Direction
        {
            get { return _Direction; }
            set
            {
                if (_Direction != value)
                    _Direction = value;
                RaisePropertiesChanged("Direction");
            }
        }

        private string _Color;
        public string Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                    _Color = value;
                RaisePropertiesChanged("Color");
            }
        }

        private int _Intensity;
        public int Intensity
        {
            get { return _Intensity; }
            set
            {
                if (_Intensity != value)
                    _Intensity = value;
                RaisePropertiesChanged("Intensity");
            }
        }

        private PointObject _PointObjectB;

        public PointObject PointObjectB
        {
            get { return _PointObjectB; }
            set
            {
                if (_PointObjectB != value)
                    _PointObjectB = value;
                RaisePropertiesChanged("PointObjectB");
            }
        }
    }
}
