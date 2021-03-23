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

    public class KeyboardListener : IKeyboardListener
    {

        public KeyboardListener(IOutputWriter outputWriter, IWorkEngine workEngine)
        {
            OutputWriter = outputWriter;
            WorkEngine = workEngine;
        }
        private IKeyboardMouseEvents m_GlobalHook;

        public IOutputWriter OutputWriter { get; }
        public IWorkEngine WorkEngine { get; }

        public void Subscribe()
        {

            OutputWriter.WriteLine("=============================================");
            OutputWriter.WriteLine("Open the Avid, and bring it to the foreground.");
            OutputWriter.WriteLine("Alt+D - Select a marker file.");
            OutputWriter.WriteLine("Alt+A - Begin background typing.");
            OutputWriter.WriteLine("Alt+B - Cancel background typing.");
            OutputWriter.WriteLine("=============================================");
            // Note: for the application hook, use the Hook.AppEvents() instead

            m_GlobalHook = Hook.GlobalEvents();

            var map = new Dictionary<Combination, Action>
            {
                { Combination.FromString("Alt+A"), () =>{ WorkEngine.Start();  }},
                { Combination.FromString("Alt+B"), () =>{ WorkEngine.Stop();  }},
                { Combination.FromString("Alt+D"), () =>{ WorkEngine.SelectFile();  }},
            };
            m_GlobalHook.OnCombination(map);

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, WindowsHook.KeyPressEventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            //e.Handled = true;
            if (Debug)
                OutputWriter.WriteLine($"KeyPress: \t{e.KeyChar}");
        }

        public bool Debug { get; set; } = false;

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (Debug)
                OutputWriter.WriteLine($"MouseDown: \t{e.Button}; \t System Timestamp: \t{e.Timestamp}");

            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }

        }

        public void Unsubscribe()
        {
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
    }
}