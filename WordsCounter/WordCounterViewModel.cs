using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using WordsCounter.ProgressBar;

namespace WordsCounter
{
     public class WordCounterViewModel : BaseViewModel
    {

        private Dictionary<string, int> tableView { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public Dictionary<string, int> TableView
        {
            get { return tableView; }
            set
            {
                if (tableView != value)
                {
                    tableView = value;
                    OnPropertyChanged(nameof(TableView));
                }
            }
        }

        public WordCounterViewModel()
        {
            CancelCommand = new RelayCommand(OnSearch);         
        }

        public async void OnSearch()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Task");
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var stopWatch = Stopwatch.StartNew();
                    TableView = new Dictionary<string, int>();
                    string fileSelected = openFileDialog.FileName;

                    try
                    {
                        CancellationTokenSource cts = new CancellationTokenSource();
                        ProgressBarViewModel progressBarVM = new ProgressBarViewModel(cts);
                        ProgressBarView progressBarView = new ProgressBarView();
                        progressBarView.DataContext = progressBarVM;

                        progressBarView.Show();

                        var ee = FileParser.Parser.WhiteSpaceParser(fileSelected, cts.Token);

                        var newWords = await Task.Run(() => FileParser.Parser.WhiteSpaceParser(fileSelected, cts.Token));
                                               
                        await Task.Run(() =>
                        {
                            try
                            {
                                foreach (var rawWord in newWords.ToList())
                                {

                                    if (cts.Token.IsCancellationRequested)
                                    {
                                        TableView.Clear();
                                        break;
                                    }

                                    if (TableView.ContainsKey(rawWord))
                                        TableView[rawWord] += 1;
                                    else
                                        TableView.Add(rawWord, 1);

                                };
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            
                        });

                        await Task.Run(() =>
                        TableView = new Dictionary<string, int>(TableView.OrderByDescending(tr => tr.Value)
                                             .ToDictionary(pair => pair.Key, pair => pair.Value)));

                        progressBarView.Close();
                        Trace.WriteLine(stopWatch.Elapsed.TotalSeconds);                    
                    }
                    
                    catch (Exception exep)
                    {
                        throw exep;
                    }
                }
            }
        }
    }
}
