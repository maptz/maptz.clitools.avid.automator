using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maptz.Editing.MarkerParser.Tool
{

    class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand();
            {
                var fileCommand = new Command("file");
                rootCommand.AddCommand(fileCommand);
                var io = new Option<string>("--input", description: "The file to trim.");
                io.AddAlias("-i");
                fileCommand.AddOption(io);
                fileCommand.Handler = CommandHandler.Create<string>(async (string input) =>
                {
                    //Parse a file of markers into an in-out list for use with the automator.
                    var engine = new Engine(SmpteFrameRate.Smpte25, 10.0);
                    await engine.ParseMarkersFileAsync(input);
                });
            }
            {
                var directoryCommand = new Command("directory");
                rootCommand.AddCommand(directoryCommand);
                var io = new Option<string>("--input", description: "The file to trim.");
                io.AddAlias("-i");
                directoryCommand.AddOption(io);
                directoryCommand.Handler = CommandHandler.Create<string>(async (string input) =>
                {
                    foreach (var fi in new DirectoryInfo(input).GetFiles())
                    {
                        //Parse a file of markers into an in-out list for use with the automator.
                        var engine = new Engine(SmpteFrameRate.Smpte25, 15.0);
                        await engine.ParseMarkersFileAsync(fi.FullName);
                    }

                });
            }
            rootCommand.Invoke(args);
        }


    }
}
