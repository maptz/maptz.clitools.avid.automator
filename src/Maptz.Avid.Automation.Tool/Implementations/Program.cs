using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maptz.Avid.Automation.Tool
{

    class Program
    {
        static void DoApp()
        {
            ///TODO Should we be using https://github.com/MediatedCommunications/WindowsInput
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

            sc.AddTransient<IMarkerSectionPullerFactory, MarkerSectionPullerFactory>();
            sc.AddSingleton<ISoundService, SoundService>();
            sc.AddSingleton<IKeyboardListener, KeyboardListener>();
            sc.AddSingleton<IOutputWriter, OutputWriter>();
            sc.AddTransient<IMarkersReader, MarkersReader>();
            sc.AddTransient<IMarkerMerger, MarkerMerger>();
            sc.Configure<MarkerMergerSettings>(Configuration.GetSection("MarkerMerger"));
            sc.Configure<MarkerMergerSettings>(settings =>
            {
                settings.FrameRate = SmpteFrameRate.Smpte2997NonDrop;
            });
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


        static async Task Start(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "ConsoleArgs";
            app.Description = ".NET Core console app with argument parsing.";

            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                DoApp();
                return 0;
            });


            app.Command("file", command =>
            {
                var inputFilePathOption = command.Option("-i|--input", "Some option value", CommandOptionType.SingleValue);
                command.Description = "This is the description for simple-command.";
                command.HelpOption("-?|-h|--help");
                command.OnExecute(async () =>
                {
                    await DoFile(inputFilePathOption.Value());
                    return 0;
                });
            });

            app.Execute(args);
        }

        private static async Task DoFile(string filePath)
        {
            await Task.Delay(3000);
        }

        [STAThread]
        static void Main(string[] args)
        {
            DoApp();
            //await Task.CompletedTask;
            //await Start(args);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
