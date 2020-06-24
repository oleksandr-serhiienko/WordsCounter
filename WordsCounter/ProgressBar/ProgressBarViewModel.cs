using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Threading;

namespace WordsCounter.ProgressBar
{
    public class ProgressBarViewModel : BaseViewModel
    {
        private CancellationTokenSource cts;
        private long totalLineLegth;
        private int currentProgress;

        public ProgressBarViewModel(CancellationTokenSource cts)
        {
            this.cts = cts;
            totalLineLegth = 0;            
            CancelCommand = new RelayCommand(() => OnCancel());
            FileParser.Parser.LineReadHandler += MoveTheBar;
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

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.CurrentProgress = e.ProgressPercentage;
        }

        public void OnCancel()
        {
            cts.Cancel();
        }

        private void MoveTheBar(object sender, (long length, long lineLength) data)
        {
            totalLineLegth += data.lineLength + 1;
            CurrentProgress = (int)(((double)totalLineLegth / (double)data.length)*100); 
        }
    }
}
