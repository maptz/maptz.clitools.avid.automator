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
    public class AvidDSToSectionsConverter
    {
        public IEnumerable<Section> Convert(string avidDSFilePath, SmpteFrameRate smpteFrameRate)
        {
            var dsText = File.ReadAllText(avidDSFilePath);
            var avidDSDocumentReader = new AvidDSDocumentReader();
            var avidDSDocument = avidDSDocumentReader.Read(dsText);

            var retval = new List<Section>();
            foreach(var dsComponent in avidDSDocument.Components)
            {
                var outTC1 = new TimeCode(dsComponent.Out, smpteFrameRate);
                var outTC = TimeCode.FromFrames(outTC1.TotalFrames - 1, smpteFrameRate);
                retval.Add(new Section
                {
                    In = new TimeCode(dsComponent.In, smpteFrameRate),
                    Out = outTC
                });
            }
            return retval.ToArray();
        }
    }
}