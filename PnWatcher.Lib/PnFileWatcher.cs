using Ninject;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace PnWatcher.Lib
{
    public interface IPnFileWatcher : IDisposable
    {
        IObservable<string> Action { get; }
        IObservable<Exception> ActionException { get; }
        IObservableFileSystemWatcher Watcher { get; }
        void Start();
        void Stop();

    }
    public class PnFileWatcher : IPnFileWatcher, IInitializable
    {
        private readonly string path;
        private readonly IObservableFileSystemWatcher watcher;

        public event EventHandler<PnFileWatcherEventArgs> onActionException;
        public event EventHandler<PnFileWatcherEventArgs> onAction;
        public IObservable<Exception> ActionException { get; private set; }
        public IObservable<string> Action { get; private set; }

        [Inject]
        public PnFileWatcher(string path)
        {
            this.path = path;
            watcher = new ObservableFileSystemWatcher(c => { c.Path = path; c.IncludeSubdirectories = false; });
        }


        public IObservableFileSystemWatcher Watcher => watcher;
        public void Dispose()
        {
            watcher.Dispose();
        }

        public void Initialize()
        {
            watcher.Created.Subscribe(file => MoveFile(file));

            ActionException = Observable.FromEventPattern<PnFileWatcherEventArgs>(
                    evt => this.onActionException += evt,
                    evt => this.onActionException -= evt
                ).Select(x => x.EventArgs.Exception);
            Action = Observable.FromEventPattern<PnFileWatcherEventArgs>(
                    evt => this.onActionException += evt,
                    evt => this.onActionException -= evt
                ).Select(x => x.EventArgs.Message);
            

        }
        public void Start() { watcher.Start(); }
        public void Stop() { watcher.Stop(); }
        private void MoveFile(FileSystemEventArgs file)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string filename = file.Name;
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var sNumber = name.Substring(0, name.Length - 3);
                    var iNumber = (int)(Int32.Parse(sNumber) / 1000);

                    var pathDest = Path.Combine(path, iNumber.ToString());
                    if (!Directory.Exists(pathDest))
                        Directory.CreateDirectory(pathDest);
                    File.Move(file.FullPath, Path.Combine(pathDest, filename));
                    onAction(this, new PnFileWatcherEventArgs(this, String.Format("Le fichier %s a été déplacé vers %s", filename, pathDest)));
                }
                catch (Exception ex)
                {
                    onActionException(this, new PnFileWatcherEventArgs(this, ex));
                }

            });

        }


    }

}
