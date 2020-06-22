using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WordsCounter.ProgressBar;

namespace WordsCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Dictionary<string, int> TableView { get; set; }

        private async void search_button_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Task");
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TableView = new Dictionary<string, int>();
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
                                    TableView.Clear();
                                    break;
                                }
                                if (TableView.ContainsKey(rawWord))
                                {
                                    TableView[rawWord] += 1;
                                    
                                }


                                else
                                    TableView.Add(rawWord, 1);

                            }
                        });

                        await Task.Run(() =>
                            TableView = TableView.OrderByDescending(tr => tr.Value)
                                                 .ToDictionary(pair => pair.Key, pair => pair.Value));

                        progressBarView.Close();

                        // It could be a good practice to use IPropertyChanged
                        // But in this case it is easier and faster to fill it like this
                        gridView.ItemsSource = TableView;
                         
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
