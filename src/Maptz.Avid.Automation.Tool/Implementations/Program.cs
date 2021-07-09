using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Maptz.Editing.Edl;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maptz.Avid.Automation.Tool
{

    
    class Program
    {

        private static IServiceProvider GetServiceProvider()
        {

            //See https://github.com/topstarai/WindowsHook
            IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false)
          .Build();
            var sc = new ServiceCollection();
            sc.AddSingleton<ISoundService, SoundService>();
            sc.AddSingleton<IKeyboardListener, KeyboardListener>();
            sc.AddSingleton<IOutputWriter, OutputWriter>();
            sc.AddTransient<IWorkEngine, WorkEngine>();
            var sp = sc.BuildServiceProvider();
            return sp;
        }

        static void StartListen()
        {

            ///TODO Should we be using https://github.com/MediatedCommunications/WindowsInput
            AllocConsole();
            var sp = GetServiceProvider();
            var ow = sp.GetRequiredService<IOutputWriter>();
            var kl = sp.GetRequiredService<IKeyboardListener>();
            //Console.Title = "Maptz Avid Automation Tool";
            ow.WriteLine("Maptz Avid Automation Tool");
            Console.WriteLine("Hello 2");
            kl.Subscribe();
            Application.Run(new ApplicationContext());
            kl.Unsubscribe();
            (sp as IDisposable).Dispose();
        }


        static void Start(string[] args)
        {
            //var edl = new CMX3600Deserializer(Options.Create(new CMX3600DeserializerSettings { IgnoreFps = true }));
            //var edlText = File.ReadAllText(@"C:\Users\steph\OneDrive\Desktop\20210702\ASSEMBLIES - ACT 2.02.00.05  - ARCH VID.edl");
            //var entries = edl.Read(edlText);
            //var first = entries.Take(10).ToArray();
            //return;

            //To insert filler, run the 'listen' command. 
            //Then start typing with Alt+A, stop with Alt+C.


            var app = new CommandLineApplication();
            app.Name = "ConsoleArgs";
            app.Description = ".NET Core console app with argument parsing.";

            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                return 0;
            });

            //To begin typing, run in listen mode.
            app.Command("listen", command =>
            {
                command.OnExecute(() =>
                {
                    Console.Title = "Avid Automator";
                    StartListen();
                    return 0;
                });
            });


            app.Command("file", command =>
            {
                var inputFilePathOption = command.Option("-i|--input", "Some option value", CommandOptionType.SingleValue);
                command.Description = "This is the description for simple-command.";
                command.HelpOption("-?|-h|--help");
                command.OnExecute(() =>
                {
                    ///TODO Should we be using https://github.com/MediatedCommunications/WindowsInput
                    //See https://github.com/topstarai/WindowsHook
                    AllocConsole();
                    //Currently this just pulls from a simple text file. Could be made to be more sophisticated. 
                    PullFromFile(inputFilePathOption.Value());
                    return 0;
                });
            });

            app.Execute(args);
        }

      
        private static void PullFromFile(string filePath)
        {
            var sc = new ServiceCollection();
            sc.Configure<FileSectionPullerSettings>(settings =>
            {
            });

            sc.AddSingleton<ISoundService, SoundService>();
            sc.AddSingleton<IKeyboardListener, KeyboardListener>();
            sc.AddSingleton<IOutputWriter, OutputWriter>();
            sc.AddTransient<FileSectionPuller>();

            var sp = sc.BuildServiceProvider();

            var fsp = sp.GetRequiredService<FileSectionPuller>();
            var totalSeconds = 5;
            Console.WriteLine($"Waiting {totalSeconds}s");
            var ss = sp.GetRequiredService<ISoundService>();
            for (int i = 0; i < totalSeconds; i++)
            {
                ss.Play(SoundServiceSound.LeadIn);
                Task.Delay(1000).GetAwaiter().GetResult();
            }
            ss.Play(SoundServiceSound.Start);
            Task.Delay(1000).GetAwaiter().GetResult();
            fsp.PullFromFile(filePath);
            ss.Play(SoundServiceSound.End);
            Task.Delay(1000).GetAwaiter().GetResult();
        }

        [STAThread]
        static void Main(string[] args)
        {
            //NOTE - An issue with a Windows Forms console application.  See https://github.com/dotnet/sdk/issues/13331#issuecomment-693002436
            // <OutputType>Exe</OutputType> Value is Exe not WinExe
            //<DisableWinExeOutputInference > true </ DisableWinExeOutputInference >

            Console.WriteLine("Hello");
            //DoApp();
            //await Task.CompletedTask;
            Start(args);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
