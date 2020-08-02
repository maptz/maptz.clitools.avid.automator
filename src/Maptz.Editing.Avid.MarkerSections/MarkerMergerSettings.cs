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

    public class MarkerMergerSettings
    {
        public double SectionDurationSeconds = 5.0;
        public SmpteFrameRate FrameRate = SmpteFrameRate.Smpte30;
    }
}