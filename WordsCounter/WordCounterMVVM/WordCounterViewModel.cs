using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordsCounter.ProgressBar;

namespace WordsCounter.WordsCounterMVVM
{
    public class WordCounterViewModel : BaseViewModel
    {

        private Dictionary<string, int> tableView { get; set; }

        public WordCounterViewModel()
        {
            SearchCommand = new RelayCommand(OnSearch);         
        }

        public RelayCommand SearchCommand { get; set; }

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

        public async void OnSearch()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;


                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Dictionary<string, int> temDictionarry = new Dictionary<string, int>();

                    string fileSelected = openFileDialog.FileName;

                    try
                    {
                        CancellationTokenSource cts = new CancellationTokenSource();
                        ProgressBarViewModel progressBarVM = new ProgressBarViewModel(cts);
                        ProgressBarView progressBarView = new ProgressBarView();
                        progressBarView.DataContext = progressBarVM;

                        progressBarView.Show();

                        var newWords = await Task.Run(() => FileParser.Parser.WhiteSpaceParser(fileSelected, cts.Token));

                        await Task.Run(() =>
                        {
                            foreach (var rawWord in newWords)
                            {

                                if (cts.Token.IsCancellationRequested)
                                {
                                    temDictionarry.Clear();
                                    break;
                                }

                                if (temDictionarry.ContainsKey(rawWord))    
                                    temDictionarry[rawWord] += 1;
                                else
                                    temDictionarry.Add(rawWord, 1);
                            }
                        });

                        await Task.Run(() =>
                            TableView = new Dictionary<string, int>(temDictionarry.OrderByDescending(tr => tr.Value)
                                                 .ToDictionary(pair => pair.Key, pair => pair.Value)));

                        progressBarView.Close();
                        
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
