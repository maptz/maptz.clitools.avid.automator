using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsHook;
namespace Maptz.Avid.Automation.Tool
{

    public class MarkerSectionPullerFactory : IMarkerSectionPullerFactory
    {
        public MarkerSectionPullerFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public MarkerSectionPuller Create(PullMode mode, string filePath)
        {
            var markerSectionPuller = new MarkerSectionPuller(
              ServiceProvider.GetRequiredService<ISoundService>(),
              ServiceProvider.GetRequiredService<IOutputWriter>(),
              ServiceProvider.GetRequiredService<IMarkersReader>(),
              ServiceProvider.GetRequiredService<IMarkerMerger>(),
              Options.Create<MarkerSectionPullerSettings>(new MarkerSectionPullerSettings
              {
                  FilePath = filePath,
                  Mode = mode
              }));
            return markerSectionPuller;
        }
    }
}