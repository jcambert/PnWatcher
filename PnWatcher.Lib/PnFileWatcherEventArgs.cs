using System;

namespace PnWatcher.Lib
{
    public class PnFileWatcherEventArgs
    {
        public PnFileWatcherEventArgs(IPnFileWatcher sender,Exception ex)
        {
            this.Sender = sender;
            this.Exception = ex;
        }

        public PnFileWatcherEventArgs(IPnFileWatcher sender,string message)
        {
            this.Sender = sender;
            this.Message = message;
        }
        public Exception Exception { get; private set; }
        public string Message { get; private set; }
        public IPnFileWatcher Sender { get; private set; }
    }
}