using GalaSoft.MvvmLight.Command;
using System.Threading;

namespace WordsCounter.ProgressBar
{
    public class ProgressBarViewModel
    {
        private CancellationTokenSource cts;

        public ProgressBarViewModel(CancellationTokenSource cts)
        {
            this.cts = cts;
            CancelCommand = new RelayCommand(() => OnCancel());
        }

        public RelayCommand CancelCommand { get; set; }

        public void OnCancel()
        {
            cts.Cancel();
        }
    }
}
