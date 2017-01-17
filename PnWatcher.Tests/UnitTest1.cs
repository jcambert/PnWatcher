using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PnWatcher.Lib;
using System.Reactive.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace PnWatcher.Tests
{
    [TestClass]
    public class UnitTest1 : FileIntegrationTestsBase
    {


        [TestMethod]
        public async Task TestCreateFile()
        {

            using (var watcher = new ObservableFileSystemWatcher(c => { c.Path = TempPath; c.IncludeSubdirectories = false; }))
            {

                watcher.Created.Subscribe(v => { Trace.WriteLine("Nouveau Fichier:" + v.Name); });
                watcher.StatusChanged.Subscribe(v => { Trace.WriteLine("Status Changed to:" + v.ToString()); });
                var first_created = watcher.Created.FirstAsync().ToTask();
                watcher.Start();


                File.WriteAllText(Path.Combine(TempPath, "Created.txt"), "foo");
                var created = await first_created;

                Assert.AreEqual(created.ChangeType, WatcherChangeTypes.Created);
                Assert.IsTrue(created.Name.Equals("Created.txt"));
            }
        }

        [TestMethod]
        public async Task TestCreateDesktopFile()
        {

            using (var watcher = new ObservableFileSystemWatcher(c => { c.Path = DesktopPath; c.IncludeSubdirectories = false; }))
            {
                string file = Path.Combine(DesktopPath, "Created.txt");
                if (File.Exists(file)) File.Delete(file);
                watcher.Created.Subscribe(v => { Trace.WriteLine("Nouveau Fichier:" + v.Name); });
                watcher.Changed.Subscribe(v => { Trace.WriteLine("Fichier change:" + v.Name); });
                watcher.StatusChanged.Subscribe(v => { Trace.WriteLine("Status Changed to:" + v.ToString()); });
                var first_created = watcher.Created.FirstAsync().ToTask();
                watcher.Start();
                File.WriteAllText(file, new string('c', 1000 * 1000 * 100));
                var created = await first_created;

                Trace.WriteLine("Wait while "+file+" is locked");
                Trace.WriteLine("");
                //Wait while file is locked
                while (file.FileInUse()) {
                    Trace.Write(".");
                    Thread.Sleep(50);
                }
                

                Assert.AreEqual(created.ChangeType, WatcherChangeTypes.Created);
                Assert.IsTrue(created.Name.Equals("Created.txt"));
                

                Trace.WriteLine(file + " is unlocked ");
                if (File.Exists(file)) File.Delete(file);
            }
        }
    }
}
