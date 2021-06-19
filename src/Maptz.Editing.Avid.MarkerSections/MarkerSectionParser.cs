using Maptz;
using Maptz.Editing.Avid.Markers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace Maptz.Editing.Avid.MarkerSections
{

    public class MarkerSectionParser : IMarkerSectionParser
    {
        public MarkerSectionParser(IMarkersReader markersReader, IMarkerMerger markerMerger)
        {
            MarkersReader = markersReader;
            MarkerMerger = markerMerger;
        }

        public IMarkersReader MarkersReader { get; }
        public IMarkerMerger MarkerMerger { get; }

        public async Task<IEnumerable<Section>> ParseSectionsAsync(string filePath)
        {
            IEnumerable<Marker> markers;
            try
            {
                markers = await this.MarkersReader.ReadFromTextFileAsync(filePath);
                AnsiConsole.MarkupLine($"Read [green]{markers.Count()}[/] markers from file.");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Failed to read markers from file '{filePath}'.[/] " + ex.ToString());
                return new Section[0]; ;
            }

            var sections = this.MarkerMerger.Merge(markers);
            AnsiConsole.MarkupLine($"Created [green]{sections.Count()}[/] sections from markers.");
            return sections;
        }
    }
}