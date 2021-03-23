using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Maptz.Avid.Automation.Tool
{

    class Program
    {
        static void Main(string[] args)
        {
            
            AllocConsole();
            //See https://github.com/topstarai/WindowsHook

            var sc = new ServiceCollection();
            sc.Configure<MarkerSectionPullerSettings>(settings =>
            {
                //Markers file path
                settings.FilePath = @"C:\Users\steph\OneDrive\Desktop\MRO _SPAN - SE NW.txt";
            });
            sc.AddSingleton<IBackgroundTaskRunner, MarkerSectionPuller>();
            sc.AddSingleton<ISoundService, SoundService>();
            sc.AddSingleton<IKeyboardListener, KeyboardListener>();
            sc.AddSingleton<IOutputWriter, OutputWriter>();
            sc.AddTransient<IMarkersReader, MarkersReader>();
            sc.AddTransient<IMarkerMerger, MarkerMerger>();
            
            
            var sp = sc.BuildServiceProvider();


            var ow = sp.GetRequiredService<IOutputWriter>();
            var kl = sp.GetRequiredService<IKeyboardListener>();
            kl.Subscribe();
            ow.WriteLine("Avid Automation Tool Running");
            Application.Run(new ApplicationContext());

            kl.Unsubscribe();

            sp.Dispose();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
