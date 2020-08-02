using Maptz.Editing.Avid.MarkerSections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Maptz.Editing.Avid.Automator.WinForms.Tool
{
    public interface ISectionTyper
    {
        Task DoTyping(IEnumerable<Section> sections);
        Task DoTyping(Section section);
    }
}