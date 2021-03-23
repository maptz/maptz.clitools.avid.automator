using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsHook;
using WindowsInput;
using WindowsInput.Native;
namespace Maptz.Avid.Automation.Tool
{
    public class MarkerSectionPullerSettings
    {
        public string FilePath { get; set; }
        public int KeyWaitMs { get; set; } = 150;
    }
}