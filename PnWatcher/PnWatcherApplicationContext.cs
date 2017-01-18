using Fclp;

using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive;
using System.Reactive.Linq;
namespace PnWatcher
{
    public class PnWatcherApplicationContext:ApplicationContext, IInitializable
    {
        Form frm;
  
        public PnWatcherApplicationContext()
        {
  
        }
        [Inject]
        public FluentCommandLineParser<ApplicationArguments> CommandLine { get; set; }

        private void setCloseEvent(Form frm)
        {
            if (frm == null) ExitThread();
            Closed = Observable.FromEvent<FormClosedEventHandler, FormClosedEventArgs>(
                    evt => frm.FormClosed += evt,
                    evt => frm.FormClosed -= evt
                    );
            Closed.Subscribe(e => { ExitThread(); });
        }

        private bool parseCommandLine(out string errors)
        {
            errors = string.Empty;
            CommandLine.Setup(arg => arg.Path).As('p', "path").Required();
            CommandLine.Setup(arg => arg.AutoRun).As('a', "autorun").SetDefault(false);
            CommandLine.Setup(arg => arg.Inspect).As('i', "inspect").SetDefault(false);
            CommandLine.Setup(arg => arg.AllowMoveFile).As('m', "movefile").SetDefault(true);
            CommandLine.Setup(arg => arg.MaxStatusLinesCount).As('c', "maxlinescount").SetDefault(20);
            CommandLine.Setup(arg => arg.Extension).As('e', "extension").SetDefault("*.lcc");

            var result = CommandLine.Parse(Environment.GetCommandLineArgs());
            if (result.HasErrors) errors = result.ErrorText;
            return !result.HasErrors;
        }

        public void Initialize()
        {
            string errors;
            var parsed = parseCommandLine(out errors);
            if (!parsed)
            {
                frm = Kernel.Get<ErrorForm>(new ConstructorArgument("errors", errors));
                frm.StartPosition = FormStartPosition.CenterScreen;


            }
            else
            {
                frm = Kernel.Get<MainForm>(new ConstructorArgument("path", CommandLine.Object.Path));
                frm.Text = string.Format("PN File Watcher %s", CommandLine.Object.Path);
            }
            setCloseEvent(frm);
            frm.Show();
        }

        [Inject]
        public IKernel Kernel { get; set; }

        public IObservable<FormClosedEventArgs> Closed { get; private set; }
    }
}
