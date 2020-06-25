using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Windows;

namespace WordsCounter.ProgressBar
{
    public class ProgressBarViewModel : BaseViewModel
    {
        private CancellationTokenSource cts;
        private long totalLineLegth;
        private int currentProgress;
        private Visibility sortingVisibility;

        public ProgressBarViewModel(CancellationTokenSource cts)
        {
            this.cts = cts;
            totalLineLegth = 0;
            SortingVisibility = Visibility.Hidden;
            CancelCommand = new RelayCommand(() => cts.Cancel());
            FileParser.Parser.LineReadHandler += MoveTheBar;
            FileParser.Parser.ReadingFinished += (a,b) => SortingVisibility = Visibility.Visible;
        }
        
        public Visibility SortingVisibility
        {
            get { return sortingVisibility; }
            private set
            {
                if (sortingVisibility != value)
                {
                    sortingVisibility = value;
                    OnPropertyChanged(nameof(SortingVisibility));
                }
            }
        }

        public int CurrentProgress
        {
            get { return currentProgress; }
            private set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged(nameof(CurrentProgress));
                }
            }
        }

        public RelayCommand CancelCommand { get; set; }

        private void MoveTheBar(object sender, (long length, long lineLength) data)
        {
            totalLineLegth += data.lineLength + 1;
            CurrentProgress = (int)((totalLineLegth / (double)data.length)*100); 
        }
    }
}
