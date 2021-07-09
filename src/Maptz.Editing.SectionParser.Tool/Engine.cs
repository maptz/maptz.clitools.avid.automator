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

    class Engine
    {
        public IServiceProvider ServiceProvider { get; }

        public IServiceProvider GetServiceProvider()
        {
            var sc = new ServiceCollection();
            sc.AddTransient<IEdlDeserializer, CMX3600Deserializer>();
            sc.AddTransient<IEdlToSectionParser, EdlToSectionParser>();
            return sc.BuildServiceProvider();
        }

        public Engine()
        {
            ServiceProvider = GetServiceProvider();
        }

        public void ConvertEdlToSections(string edlFilePath, Predicate<IEdlEntry> predicate = null)
        {

            var converter = ServiceProvider.GetRequiredService<IEdlToSectionParser>();
            var outputFilePath = Path.Combine(Path.GetDirectoryName(edlFilePath), Path.GetFileNameWithoutExtension(edlFilePath) + $".sections.txt");
            converter.ParseSections(edlFilePath, outputFilePath, predicate);

        }
    }
}