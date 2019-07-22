using DevExpress.Mvvm;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.CodeView;
using AksenovNewTeleTeth.Models;

namespace AksenovNewTeleTeth.BusinessLogic
{
    public class FileReader
    {
        public event EventHandler StopWork;
        public ObservableCollection<MainObject> MainObjects = new ObservableCollection<MainObject>();

        public async void Init(CancellationTokenSource cts)
        {
            List<string[]> listString = new List<string[]>();
            CancellationToken token = cts.Token;
            string path = FindFile();
            if (!String.IsNullOrEmpty(path))
            {
                var result = await ReadFileAsync(path, token);
                if (result != null)
                {
                    listString.AddRange(result);
                    var result2 = await ConvertDataAsync(listString,token);
                    MainObjects.AddRange(result2);
                }
            }
            StopWork?.Invoke(this, null);
        }

        private async Task<ObservableCollection<MainObject>> ConvertDataAsync(List<string[]> listString, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return null;
            var result = await Task.Run(() => ConvertData.ConvertDataFile(listString, token));
            return result;
        }

        private string FindFile()
        {
            IOpenFileDialogService OpenFileDialogService = new OpenFileDialogService
            {
                Filter = "Text Files (.csv)|*.csv|All Files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Test Dialog",
            };


            var DialogResult = OpenFileDialogService.ShowDialog();
            if (!DialogResult)
            {
                return string.Empty;
            }
            else
            {
                IFileInfo file = OpenFileDialogService.Files.First();
                return file.GetFullName();

            }
        }

        private async Task<List<string[]>> ReadFileAsync(string filename, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return null;
            var result = await Task.Run(() => ReadFile(filename, token));
            return result;
        }

        private List<string[]> ReadFile(string filename, CancellationToken token)
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

    }
}
