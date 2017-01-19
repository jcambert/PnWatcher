using Fclp;
using Ninject;
using Ninject.Parameters;
using PnWatcher.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PnWatcher
{
    public partial class MainForm : Form,IInitializable
    {
        private readonly string path;
        private IPnFileWatcher watcher;
        private readonly Subject<bool> StartStop;
        public MainForm(string path)
        {
            InitializeComponent();
            this.path = path;
            StartStop = new Subject<bool>();
        }

        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public FluentCommandLineParser<ApplicationArguments> CommandLine { get; set; }

        public void Initialize()
        {

            // var args = Environment.GetCommandLineArgs();

            watcher = Kernel.Get<IPnFileWatcher>(
                new ConstructorArgument("path", path),
                new ConstructorArgument("extension", CommandLine.Object.Extension),
                new ConstructorArgument("allowMoveFile",CommandLine.Object.AllowMoveFile),
                new ConstructorArgument("inspectExisting", CommandLine.Object.Inspect)
            );
            watcher.Watcher.StatusChanged.Subscribe(status =>
            {
                string message = (status ? "Démarrage" : "Arret") + " de la surveillance";
                addMessage(message);
                
            });
            watcher.Action.Subscribe(msg => addMessage(msg));
            watcher.ActionException.Subscribe(msg => {
                addMessage("ERREUR:" + msg);
            });

            StartStop.Subscribe(start_stop => {
                //startBtn.InvokeIfRequired(c=> c.Visible = !(start_stop));
                //pauseBtn.InvokeIfRequired(c => c.Visible = !(startBtn.Visible));
                //pauseBtn.Visible = !startBtn.Visible;

                
                if (start_stop)
                {
                    startBtn.Visible = false;
                    pauseBtn.Visible = true;
                    watcher.Start();
                }else
                {
                    startBtn.Visible = true;
                    pauseBtn.Visible = false;
                    watcher.Stop();
                }
            });
            Observable.FromEventPattern<EventArgs>(startBtn,"Click").Subscribe(btn => Start());
            Observable.FromEventPattern<EventArgs>(pauseBtn, "Click").Subscribe(btn => Stop() );

            if (CommandLine.Object.AutoRun)
                Start();
            else
                Stop();
        }

        public void Start() {
            StartStop.OnNext(true);
        }
        public void Stop() {
            StartStop.OnNext(false);
        }
        private void addMessage(string message)
        {
            statusTxt.InvokeIfRequired(c=> c.Text = message + "\r\n" + statusTxt.Text.RemoveLines(CommandLine.Object.MaxStatusLinesCount, Sens.BottomTop));
            statusTxt.InvokeIfRequired(c => ((TextBox) c).Select(0, 0));
        }



    }
}
