using Ninject;
using Ninject.Parameters;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Ninject.Extensions.Logging;

namespace PnWatcher.Lib
{
    public interface IPnFileWatcher : IDisposable
    {
        IObservable<string> Action { get; }
        IObservable<Exception> ActionException { get; }
        IObservableFileSystemWatcher Watcher { get; }
        void Start();
        void Stop();
        void inspect();
    }
    public class PnFileWatcher : IPnFileWatcher, IInitializable
    {
        private readonly string path;
        private  IObservableFileSystemWatcher watcher;
        private readonly bool allowMoveFile;
        private readonly bool inspectExisting;
        private readonly string extension;

        public event EventHandler<PnFileWatcherEventArgs> onActionException;
        public event EventHandler<PnFileWatcherEventArgs> onAction;
        public IObservable<Exception> ActionException { get; private set; }
        public IObservable<string> Action { get; private set; }

        [Inject]
        public PnFileWatcher(string path,string extension,bool allowMoveFile,bool inspectExisting)
        {
            this.path = path;
            this.extension = extension;
            this.allowMoveFile = allowMoveFile;
            this.inspectExisting = inspectExisting;
        }
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        public IObservableFileSystemWatcher Watcher => watcher;
        public void Dispose()
        {
            watcher.Dispose();
        }

        public void Initialize()
        {
            Action<FileSystemWatcher> configure = c => { c.Path = path;c.Filter = this.extension; c.IncludeSubdirectories = false; };
            watcher = Kernel.Get<IObservableFileSystemWatcher>(new ConstructorArgument("configure", configure));
            watcher.Created.Subscribe(file => MoveFile(file.FullPath));
            watcher.StatusChanged.Subscribe(status =>
            {
                if (status && inspectExisting) inspect();
            });
            ActionException = Observable.FromEventPattern<PnFileWatcherEventArgs>(
                    evt => this.onActionException += evt,
                    evt => this.onActionException -= evt
                ).Select(x => x.EventArgs.Exception);

            Action = Observable.FromEventPattern<PnFileWatcherEventArgs>(
                    evt => this.onAction+= evt,
                    evt => this.onAction -= evt
                ).Select(x => x.EventArgs.Message);

           
        }
        public void Start() { watcher.Start(); }
        public void Stop() { watcher.Stop(); }
        private void MoveFile(string filename)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                try
                {
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var pathDest = getPathDestination(name);
                    if (this.allowMoveFile)
                    {

                        if (!Directory.Exists(pathDest))
                        {
                            Directory.CreateDirectory(pathDest);
                            sendAction(String.Format("Creation du répertoire {0}",  pathDest));
                        }
                        var fileDest = getFileDestination(name);
                        if (File.Exists(fileDest))
                        {
                            File.Delete(fileDest);
                            sendAction(String.Format("Suppression du fichier existant {0} avant copie", fileDest));
                        }
                        File.Move(filename, fileDest);
                    }
                    sendAction( String.Format("Le fichier {0} a été déplacé vers {1}", filename, pathDest));
                }
                catch (Exception ex)
                {
                    sendException(ex);
                    
                }

            });

        }

        private void sendAction(string message)
        {
            onAction(this, new PnFileWatcherEventArgs(this, message));
            Logger.Debug(message);
        }

        private void sendException(Exception ex)
        {
            onActionException(this, new PnFileWatcherEventArgs(this, ex));
            Logger.ErrorException("Erreur", ex);
        }

        private string getPathDestination(string name)
        {
            
            var sNumber = name.Substring(0, name.Length - 3);
            var iNumber = (int)(Int32.Parse(sNumber) / 1000)*1000;
            return  Path.Combine(path, iNumber.ToString());
        }

        private string  getFileDestination(string name)
        {
            return Path.Combine(getPathDestination(name), name + this.extension.Replace("*",""));
        }

        public void inspect()
        {
            sendAction(String.Format("Inspection des fichiers existant dans {0}", path));
            var files=Directory.EnumerateFiles(path, this.extension);
            files.ToList().ForEach(file => MoveFile(file));
        }
    }

}
