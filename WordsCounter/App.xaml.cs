using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WordsCounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var wordsCounterView = new WordCounterView();
            var wordsCounterViewModel = new WordCounterViewModel();
            wordsCounterView.DataContext = wordsCounterViewModel;
            wordsCounterView.Show();
        }
    }
}
