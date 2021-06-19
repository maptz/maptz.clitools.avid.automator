using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
namespace Maptz.Avid.Automation.Tool
{

    public class FileSectionPullerSettings
    {
        
        public int KeyWaitMs { get; set; } = 50;
    }
}