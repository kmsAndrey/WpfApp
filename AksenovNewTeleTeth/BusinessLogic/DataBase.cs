using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AksenovNewTeleTeth.Models;

namespace AksenovNewTeleTeth.BusinessLogic
{
    public class DataBase
    {
        public event EventHandler StopWork;
        public ObservableCollection<MainObject> MainObjects = new ObservableCollection<MainObject>();

        public void DownloadDBAction()
        {
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
                i.ForEach(x => MainObjects.Add(x));
                StopWork?.Invoke(this, null);
            }
        }

        public async void UploadDBAction(ObservableCollection<MainObject> mainObjects,CancellationTokenSource cts)
        {
            if (mainObjects.Count == 0) return;
            CancellationToken token = cts.Token;
            var result = await UploadDataAsync(mainObjects, token);
            StopWork?.Invoke(this, null);
        }

        private async Task<bool> UploadDataAsync(ObservableCollection<MainObject> list, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return false;
            var result = await Task.Run(() => UploadData(list, token));
            return result;
        }

        private bool UploadData(ObservableCollection<MainObject> list, CancellationToken token)
        {
            using (var con = new UserContext())
            {
                int i = 0;
                foreach (var element in list)
                {
                    if (token.IsCancellationRequested)
                    {
                        return false;
                    }
                    i++;
                    con.MainObjects.Add(element);
                }
                con.SaveChanges();
            }
            return true;
        }



    }
}
