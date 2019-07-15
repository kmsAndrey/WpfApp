using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.VisualBasic.FileIO;
using AksenovNewTeleTeth.Models;
using System.Globalization;

namespace AksenovNewTeleTeth.ViewModels
{
    public class MainViewModel: Base
    {
        public UserCommand DownloadActionCommand { get; set; }
        public UserCommand StopDownloadActionCommand { get; set; }
        public UserCommand DownloadDBActionCommand { get; set; }
        public UserCommand UploadDBActionCommand { get; set; }
        public UserCommand CleanDataGridActionCommand { get; set; }

        private List<MainObject> _MainObjects = new List<MainObject>();
        public List<MainObject> MainObjects
        {
            get { return _MainObjects; }
            set
            {
                if(_MainObjects!=value)
                {
                    _MainObjects = value;
                    OnPropertyChanged("MainObjects");
                }
            }
        }

        CancellationTokenSource cts;

        public Progressbar _progressBar = new Progressbar();
        public Progressbar progressBar {
            get { return _progressBar; }
            set
            {
                if (_progressBar != value)
                {
                    _progressBar = value;
                    OnPropertyChanged("progressBar");
                }
            }
        }

        private bool _NotBlockElement;

        public bool NotBlockElement
        {
            get { return _NotBlockElement; }
            set
            {
                if (_NotBlockElement != value)
                {
                    _NotBlockElement = value;
                    OnPropertyChanged("NotBlockElement");
                }
            }
        }

        public MainViewModel()
        {
            DownloadActionCommand = new UserCommand(DownloadAction);
            StopDownloadActionCommand = new UserCommand(StopDownloadAction);
            DownloadDBActionCommand = new UserCommand(DownloadDBAction);
            UploadDBActionCommand = new UserCommand(UploadDBAction);
            CleanDataGridActionCommand = new UserCommand(CleanDataGridAction);
            NotBlockElement = true;
        }

        #region download data svc file
        public async void DownloadAction()
        {
            List<string[]> listString = new List<string[]>();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            string path = FindFile();
            if (!String.IsNullOrEmpty(path))
            {
                NotBlockElement = false;
                progressBar.Work = true;
                var result = await ReadFileAsync(path, token);
                progressBar.Work = false;
                NotBlockElement = true;
                if (result != null)
                {
                    listString.AddRange(result);
                    ConvertDataFile(listString);
                }
            }
        }

        public void ConvertDataFile(List<string[]> masDataSVC)
        {
            int[] numberColumn = FindNameColumns(masDataSVC);

            IFormatProvider formatProvider = new NumberFormatInfo { NumberDecimalSeparator = "." };
            foreach(var i in masDataSVC)
            {
                var ii = i[numberColumn[8]];
                MainObject main = new MainObject()
                {
                    Id = 0,
                    Date = DateTime.Parse(i[numberColumn[0]]),
                    PointObjectA = new PointObject
                    {
                        Id = 0,
                        Name = i[numberColumn[1]],
                        Latitude = double.Parse(i[numberColumn[8]],formatProvider),
                        Longitude = double.Parse(i[numberColumn[9]], formatProvider),
                        Type = i[numberColumn[2]]

                    },
                    Color = i[numberColumn[6]],
                    Direction = i[numberColumn[5]],
                    Intensity = Int32.Parse(i[numberColumn[7]]),
                    PointObjectB = new PointObject
                    {
                        Id = 0,
                        Name = i[numberColumn[3]],
                        Latitude = double.Parse(i[numberColumn[10]], formatProvider),
                        Longitude = double.Parse(i[numberColumn[11]], formatProvider),
                        Type = i[numberColumn[4]]
                    }
                };
                MainObjects.Add(main);
            }
        }

        public int[] FindNameColumns(List<string[]> masDataSVC)
        {
            string[] masNameColumn = { "Date", "Object A", "Type A", "Object B", "Type B", "Direction", "Color", "Intensity", "LatitudeA", "LongitudeA", "LatitudeB", "LongitudeB" };
            int[] number = new int[12];

            foreach (var i in masDataSVC)
            {
                for (int j = 0; j < i.Length; j++)
                {
                    for (int k = 0; k < masNameColumn.Length; k++)
                    {
                        if (i[j] == masNameColumn[k])
                        {
                            number[k] = j;
                            break;
                        }
                    }
                }
                masDataSVC.Remove(i);
                break;
            }
            return number;
        }

        public string FindFile()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Text documents (*.csv)|*.csv|All files (*.*)|*.*";
            dialog.FilterIndex = 1;

            Nullable<bool> result = dialog.ShowDialog();
            string filename = "";
            if (result == true)
            {
                filename = dialog.FileName;
            }
            return filename;
        }

        public async Task<List<string[]>> ReadFileAsync(string filename, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return null;
            var result = await Task.Run(() => ReadFile(filename, token));
            return result;
        }

        public List<string[]> ReadFile(string filename, CancellationToken token)
        {
            using (TextFieldParser textFieldParser = new TextFieldParser(filename))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(";");
                List<string[]> listStringMasSVC = new List<string[]>();
                while (!textFieldParser.EndOfData)
                {

                    if (token.IsCancellationRequested)
                    {
                        return null;
                    }
                    string[] Fields = textFieldParser.ReadFields().Where(n => !string.IsNullOrEmpty(n)).ToArray();
                    listStringMasSVC.Add(Fields);
                }
                return listStringMasSVC;
            }
        }

        #endregion

        public void StopDownloadAction()
        {
            if(cts!=null)
            {
                cts.Cancel();
                progressBar.Work = false;
                NotBlockElement = true;
            }
        }

        #region download data
        public void DownloadDBAction()
        {
            NotBlockElement = false;
            progressBar.Work = true;

            using (var con = new UserContext())
            {
                var i = con.MainObjects.Select(x => new
                {
                    Id = x.Id,
                    Date = x.Date,
                    PointObjectAId = x.PointObjectA.Id,
                    LatitudeA = x.PointObjectA.Latitude,
                    LongitudeA = x.PointObjectA.Longitude,
                    NameA = x.PointObjectA.Name,
                    TypeA = x.PointObjectA.Type,
                    PointObjectBId = x.PointObjectB.Id,
                    LatitudeB = x.PointObjectB.Latitude,
                    LongitudeB = x.PointObjectB.Longitude,
                    NameB = x.PointObjectB.Name,
                    TypeB = x.PointObjectB.Type,
                    Color = x.Color,
                    Direction = x.Direction,
                    Intensity = x.Intensity
                }).AsEnumerable().Select(y => new MainObject
                {
                    Id = y.Id,
                    Date = y.Date,
                    PointObjectA = new PointObject
                    {
                        Id = y.PointObjectAId,
                        Latitude = y.LatitudeA,
                        Longitude = y.LongitudeA,
                        Name = y.NameA,
                        Type = y.TypeA

                    },
                    PointObjectB = new PointObject
                    {
                        Id = y.PointObjectBId,
                        Latitude = y.LatitudeB,
                        Longitude = y.LongitudeB,
                        Name = y.NameB,
                        Type = y.TypeB
                    },
                    Color = y.Color,
                    Direction = y.Direction,
                    Intensity = y.Intensity
                }).ToList();
                MainObjects = i;
            }
            progressBar.Work = false;
            NotBlockElement = true;
        }
        #endregion

        #region upload data database

        public async void UploadDBAction()
        {
            if (MainObjects == null) return;
            progressBar.Work = true;
            NotBlockElement = false;
            CancellationToken token = cts.Token;
            var result = await UploadDataAsync(MainObjects, token);
            progressBar.Work = false;
            NotBlockElement = true;
        }

        public async Task<bool> UploadDataAsync(List<MainObject> list, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return false;
            var result = await Task.Run(() => UploadData(list, token));
            return result;
        }

        public bool UploadData(List<MainObject> list, CancellationToken token)
        {
            using (var con = new UserContext())
            {
                foreach(var element in list)
                {
                    if (token.IsCancellationRequested)
                    {
                        return false;
                    }
                    con.MainObjects.Add(element);
                }
                con.SaveChanges();
            }
            return true;
        }
        #endregion

        public void CleanDataGridAction()
        {
            MainObjects= new List<MainObject>();
        }

    }
}