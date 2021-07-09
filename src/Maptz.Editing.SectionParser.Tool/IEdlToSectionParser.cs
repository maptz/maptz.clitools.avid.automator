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
    public interface IEdlToSectionParser
    {
        void ParseSections(string edlFilePath, string outputFilePath, Predicate<IEdlEntry> predicate);
    }
}