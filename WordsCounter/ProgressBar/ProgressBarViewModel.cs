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
        private Visibility transferFinished;

        public ProgressBarViewModel(CancellationTokenSource cts)
        {
            this.cts = cts;
            totalLineLegth = 0;
            TransferFinished = Visibility.Hidden;
            CancelCommand = new RelayCommand(() => cts.Cancel());
            FileParser.Parser.LineReadHandler += MoveTheBar;
            FileParser.Parser.ReadingFinished += (a,b) => TransferFinished = Visibility.Visible;
        }
        
        public Visibility TransferFinished
        {
            get { return this.transferFinished; }
            private set
            {
                if (this.transferFinished != value)
                {
                    this.transferFinished = value;
                    this.OnPropertyChanged(nameof(TransferFinished));
                }
            }
        }

        public int CurrentProgress
        {
            get { return this.currentProgress; }
            private set
            {
                if (this.currentProgress != value)
                {
                    this.currentProgress = value;
                    this.OnPropertyChanged(nameof(CurrentProgress));
                }
            }
        }

        public RelayCommand CancelCommand { get; set; }

        private void MoveTheBar(object sender, (long length, long lineLength) data)
        {
            totalLineLegth += data.lineLength + 1;
            CurrentProgress = (int)(((double)totalLineLegth / (double)data.length)*100); 
        }
    }
}
