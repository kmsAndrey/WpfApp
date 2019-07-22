using System;
using System.Threading;
using AksenovNewTeleTeth.Models;
using AksenovNewTeleTeth.BusinessLogic;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.DataAnnotations;

namespace AksenovNewTeleTeth.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        public DelegateCommand DownloadActionCommand { get; set; }
        public DelegateCommand StopDownloadActionCommand { get; set; }
        public DelegateCommand DownloadDBActionCommand { get; set; }
        public DelegateCommand UploadDBActionCommand { get; set; }
        public DelegateCommand CleanDataGridActionCommand { get; set; }
        public DelegateCommand InitCommand { get; set; }

        protected virtual IOpenFileDialogService OpenFileDialogService { get { return null; } }

        CancellationTokenSource cts;

        private ObservableCollection<MainObject> _MainObjects = new ObservableCollection<MainObject>();
        public ObservableCollection<MainObject> MainObjects
        {
            get { return _MainObjects; }
            set
            {
                if(_MainObjects!=value)
                {
                    _MainObjects = value;
                    RaisePropertyChanged("MainObjects");
                }
            }
        }

        public Progressbar _progressBar = new Progressbar();
        public Progressbar progressBar {
            get { return _progressBar; }
            set
            {
                if (_progressBar != value)
                {
                    _progressBar = value;
                    RaisePropertyChanged("progressBar");
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
                    RaisePropertyChanged("NotBlockElement");
                }
            }
        }

        public MainViewModel()
        {
            DownloadActionCommand = new DelegateCommand(DownloadAction);
            StopDownloadActionCommand = new DelegateCommand(StopDownloadAction);
            DownloadDBActionCommand = new DelegateCommand(DownloadDBAction);
            UploadDBActionCommand = new DelegateCommand(UploadDBAction);
            CleanDataGridActionCommand = new DelegateCommand(CleanDataGridAction);
            InitCommand = new DelegateCommand(Init);
        }

        public void Init()
        {
            MainObjects = new ObservableCollection<MainObject>();
            cts = new CancellationTokenSource();
            StopWork();
        }

        public void DownloadAction()
        {
            StartWork();
            FileReader svc = new FileReader();
            svc.StopWork += Svc_StopWork;
            svc.Init(cts);
            MainObjects=svc.MainObjects;
        }

        private void Svc_StopWork(object sender, EventArgs e)
        {
            if(sender is FileReader)
            {
                StopWork();
            }
        }

        public void StopDownloadAction()
        {
            if(cts!=null)
            {
                cts.Cancel();
                cts = new CancellationTokenSource();
            }
            StopWork();
        }

        public void DownloadDBAction()
        {
            StartWork();
            DataBase db = new DataBase();
            db.StopWork += Db_StopWork;
            db.DownloadDBAction();
            MainObjects=(db.MainObjects);
        }

        private void Db_StopWork(object sender, EventArgs e)
        {
            if (sender is DataBase)
            {
                StopWork();
            }
        }

        private void StopWork()
        {
            NotBlockElement = true;
            progressBar.Work = false;
        }

        private void StartWork()
        {
            NotBlockElement = false;
            progressBar.Work = true;
        }

        public void UploadDBAction()
        {
            if (MainObjects.Count==0) return;
            StartWork();
            DataBase db = new DataBase();
            db.StopWork += Db_StopWork;
            db.UploadDBAction(MainObjects,cts);
        }


        public void CleanDataGridAction()
        {
            Init();
        }

    }
}