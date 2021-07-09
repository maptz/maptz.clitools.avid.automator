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
    class EdlToSectionParser : IEdlToSectionParser
    {
        public EdlToSectionParser(IEdlDeserializer edlDeserializer)
        {
            EdlDeserializer = edlDeserializer;
        }

        public IEdlDeserializer EdlDeserializer { get; }

        public void ParseSections(string edlFilePath, string outputFilePath, Predicate<IEdlEntry> predicate)
        {
            var edlFileContents = File.ReadAllText(edlFilePath);
            var edl = new CMX3600Deserializer().Read(edlFileContents);

            List<Section> sections = new();
            foreach (var entry in edl)
            {
                if (predicate == null || predicate(entry))
                {
                    var outTC = new TimeCode(entry.RecordOut, SmpteFrameRate.Smpte25);
                    var outTC2 = TimeCode.FromFrames(outTC.TotalFrames - 1, outTC.FrameRate);
                    var section = new Section()
                    {
                        In = new TimeCode(entry.RecordIn, SmpteFrameRate.Smpte25),
                        Out = outTC2

                    };

                    sections.Add(section);
                }
            }

            var sw = new SectionWriter();
            sw.WriteToFile(outputFilePath, sections);
        }
    }
}