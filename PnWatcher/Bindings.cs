using System;
using Ninject.Modules;
using PnWatcher.Lib;
using Fclp;

namespace PnWatcher
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IObservableFileSystemWatcher>().To<ObservableFileSystemWatcher>().InSingletonScope();
            this.Bind<IPnFileWatcher>().To<PnFileWatcher>().InSingletonScope();
            this.Bind<FluentCommandLineParser<ApplicationArguments>>().ToSelf().InSingletonScope();
        }
    }
}