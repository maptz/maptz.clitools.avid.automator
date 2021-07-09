using Maptz.Editing.Avid.MarkerSections;
using Maptz.Editing.Edl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace Maptz.Editing.SectionParser.Tool
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
                    var engine = new Engine();
                    engine.ConvertEdlToSections(input, e=> e.ClipName != null && (e.ClipName.Contains("_BOT_") || e.ClipName.Contains("_JEB_") || e.ClipName.Contains("_ERO_") || e.ClipName.Contains("_BTR_")));
                });
            }
            rootCommand.Invoke(args);
        }
    }
}
