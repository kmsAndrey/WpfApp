﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksenovNewTeleTeth.Models
{
    public class PointObject: Base
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                    _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                    _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                    _Type = value;
                OnPropertyChanged("Type");
            }
        }

        private double _Latitude;
        public double Latitude
        {
            get { return _Latitude; }
            set
            {
                if (_Latitude != value)
                    _Latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        private double _Longitude;
        public double Longitude
        {
            get { return _Longitude; }
            set
            {
                if (_Longitude != value)
                    _Longitude = value;
                OnPropertyChanged("Longitude");
            }
        }
    }
}
