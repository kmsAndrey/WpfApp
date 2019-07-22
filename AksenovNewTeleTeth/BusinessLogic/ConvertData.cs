using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using AksenovNewTeleTeth.Models;

namespace AksenovNewTeleTeth.BusinessLogic
{
    public static class ConvertData
    {
        public static ObservableCollection<MainObject> ConvertDataFile(List<string[]> masDataSVC, CancellationToken token)
        {
            int[] numberColumn = FindNameColumns(masDataSVC);
            ObservableCollection<MainObject> MainObjects = new ObservableCollection<MainObject>();
            IFormatProvider formatProvider = new NumberFormatInfo { NumberDecimalSeparator = "." };
            foreach (var i in masDataSVC)
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
                        Latitude = double.Parse(i[numberColumn[8]], formatProvider),
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
                if (token.IsCancellationRequested)
                {
                    MainObjects = new ObservableCollection<MainObject>();
                    return MainObjects;
                }
                MainObjects.Add(main);
            }
            return MainObjects;
        }

        private static int[] FindNameColumns(List<string[]> masDataSVC)
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
    }
}
