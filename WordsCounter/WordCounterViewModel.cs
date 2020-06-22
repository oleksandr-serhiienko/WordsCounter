using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsCounter
{
     public class WordCounterViewModel
    {
        public RelayCommand Search { get; set; }
        
        public Dictionary<string, int> TableView { get; set; }
    }
}
