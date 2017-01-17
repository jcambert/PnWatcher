namespace PnWatcher.Lib
{
    public class ObservableFileSystemWatcherEventArgs
    {
        public ObservableFileSystemWatcherEventArgs(IObservableFileSystemWatcher sender,bool status)
        {
            this.Sender = sender;
            this.Status = status;
        }

        public bool Status { get; private set; }
        public IObservableFileSystemWatcher Sender { get; private set; }
    }
}