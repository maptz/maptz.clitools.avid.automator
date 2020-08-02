using Maptz;
using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maptz.Editing.Avid.Automator.WinForms.Tool
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //FieldInfo info = typeof(SendKeys).GetField("keywords", BindingFlags.Static | BindingFlags.NonPublic);
            //Array oldKeys = (Array)info.GetValue(null);
            //Type elementType = oldKeys.GetType().GetElementType();
            //Array newKeys = Array.CreateInstance(elementType, oldKeys.Length + 10);
            //Array.Copy(oldKeys, newKeys, oldKeys.Length);
            //for (int i = 0; i < 10; i++)
            //{
            //    var newItem = Activator.CreateInstance(elementType, "NUM" + i, (int)Keys.NumPad0 + i);
            //    newKeys.SetValue(newItem, oldKeys.Length + i);
            //}
            //info.SetValue(null, newKeys);

            //https://github.com/michaelnoonan/inputsimulator

            var sc = new ServiceCollection();
            sc.AddTransient<IMarkersReader, MarkersReader>();
            sc.AddTransient<IMarkerMerger, MarkerMerger>();
            sc.AddTransient<ISectionTyper, SectionTyper>();
            sc.Configure<MarkerMergerSettings>(settings =>
            {
                settings.FrameRate = SmpteFrameRate.Smpte25;
                //settings.FrameRate = SmpteFrameRate.Smpte30;
            });
            sc.AddSingleton<Form2>();
            var sp = sc.BuildServiceProvider();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //var form2 = sp.GetRequiredService<Form2>();
            //Application.Run(form2);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


    }
}
