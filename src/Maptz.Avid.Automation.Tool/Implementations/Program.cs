using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Maptz.Avid.Automation.Tool
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            AllocConsole();
            //See https://github.com/topstarai/WindowsHook

            IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false)
          .Build();

            var sc = new ServiceCollection();
            sc.Configure<MarkerSectionPullerSettings>(settings =>
            {
                //Markers file path
                settings.FilePath = string.Empty;
            });
            sc.AddSingleton<IBackgroundTaskRunner, MarkerSectionPuller>();
            sc.AddSingleton<ISoundService, SoundService>();
            sc.AddSingleton<IKeyboardListener, KeyboardListener>();
            sc.AddSingleton<IOutputWriter, OutputWriter>();
            sc.AddTransient<IMarkersReader, MarkersReader>();
            sc.AddTransient<IMarkerMerger, MarkerMerger>();
            sc.Configure<MarkerMergerSettings>(Configuration.GetSection("MarkerMerger"));
            sc.AddTransient<IWorkEngine, WorkEngine>();


            var sp = sc.BuildServiceProvider();

            var ow = sp.GetRequiredService<IOutputWriter>();
            var kl = sp.GetRequiredService<IKeyboardListener>();
            Console.Title = "Maptz Avid Automation Tool";
            ow.WriteLine("Maptz Avid Automation Tool");
            kl.Subscribe();

            Application.Run(new ApplicationContext());

            kl.Unsubscribe();

            sp.Dispose();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
