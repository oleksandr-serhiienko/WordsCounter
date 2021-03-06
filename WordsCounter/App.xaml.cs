﻿using System.Windows;
using WordsCounter.WordsCounterMVVM;

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
