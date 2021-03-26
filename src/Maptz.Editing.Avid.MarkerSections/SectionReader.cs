using Maptz.Editing.Avid.DS;
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

namespace Maptz.Editing.Avid.MarkerSections
{

    public class SectionReader
    {
        public IEnumerable<Section> ReadFromFile(string filePath, SmpteFrameRate frameRate = SmpteFrameRate.Smpte25)
        {
            var str = File.ReadAllText(filePath);
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var retval = new List<Section>();
            foreach (var line in lines)
            {
                var parts = line.Split(new char[] { '\t' }, StringSplitOptions.None);
                var section = new Section
                {
                    In = new TimeCode(parts[0], frameRate),
                    Out = new TimeCode(parts[1], frameRate)
                };
                retval.Add(section);
            }
            return retval;
        }
    }
}