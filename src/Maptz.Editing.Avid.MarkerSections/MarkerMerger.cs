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
namespace Maptz.Editing.Avid.MarkerSections
{

    public class MarkerMerger : IMarkerMerger
    {
        public MarkerMerger(IOptions<MarkerMergerSettings> settings)
        {
            this.Settings = settings.Value;
        }

        public IEnumerable<Section> Merge(IEnumerable<Marker> markers)
        {
            List<Section> sections = new List<Section>();
            Section lastSection = null;
            var orderedMarkers = markers.OrderBy(p => p.Timecode);
                
            foreach (var marker in orderedMarkers)
            {
                var tc = new TimeCode(marker.Timecode, this.Settings.FrameRate);
                var framesPerSecond = new TimeCode(1.0, this.Settings.FrameRate).TotalFrames;
                if (lastSection == null || (tc.TotalFrames - lastSection.Out.TotalFrames) > framesPerSecond * this.Settings.SectionDurationSeconds)
                {
                    var section = new Section { In = tc.Add(TimeCode.FromSeconds(0.0 - this.Settings.SectionDurationSeconds, this.Settings.FrameRate)), Out = tc.Add(TimeCode.FromSeconds(this.Settings.SectionDurationSeconds, this.Settings.FrameRate)), };
                    sections.Add(section);
                    lastSection = section;
                }
                else
                {
                    lastSection.Out = tc.Add(TimeCode.FromSeconds(this.Settings.SectionDurationSeconds, this.Settings.FrameRate));
                }
            }
            return sections;
        }

        public MarkerMergerSettings Settings
        {
            get;
        }
    }
}