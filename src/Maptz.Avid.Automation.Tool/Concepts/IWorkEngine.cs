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
    public interface IWorkEngine
    {
        bool IsRunning { get; }
        public void Start(PullMode mode);
        public void Stop();
        public void SelectFile();
        void RepeatInsert();
    }
}