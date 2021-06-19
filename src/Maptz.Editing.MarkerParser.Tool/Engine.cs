using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Maptz.Editing.MarkerParser.Tool
{
    class Engine
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public Engine(SmpteFrameRate frameRate, double sectionDurationSeconds)
        {
            ConfigureServices(frameRate, sectionDurationSeconds);
        }

        private void ConfigureServices(SmpteFrameRate frameRate, double sectionDurationSeconds)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IMarkerSectionParser, MarkerSectionParser>();
            serviceCollection.AddTransient<IMarkersReader, MarkersReader>();
            serviceCollection.AddTransient<IMarkerMerger, MarkerMerger>();
            serviceCollection.Configure<MarkerMergerSettings>(settings =>
            {
                settings.FrameRate = frameRate;
                settings.SectionDurationSeconds = sectionDurationSeconds;
            });
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public async Task ParseMarkersFileAsync(string filePath)
        {
            AnsiConsole.MarkupLine($"Parsing markers from file {filePath}");
            var msp = ServiceProvider.GetRequiredService<IMarkerSectionParser>();
            var sections = await msp.ParseSectionsAsync(filePath);
            var sectionsStr = string.Join(Environment.NewLine, sections.Select(p => $"{p.In.ToString()}\t{p.Out.ToString()}"));
            var outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".out.txt");
            AnsiConsole.MarkupLine($"Writing {sections.Count()} sections to file {outputFilePath}");
            File.WriteAllText(outputFilePath, sectionsStr);
        }
    }
}