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

        public KeyboardListener(IBackgroundTaskRunner backgrounder, IOutputWriter outputWriter)
        {
            Backgrounder = backgrounder;
            OutputWriter = outputWriter;
        }
        private IKeyboardMouseEvents m_GlobalHook;

        private IBackgroundTaskRunner Backgrounder { get; }
        public IOutputWriter OutputWriter { get; }

        private void StartBackgrounder()
        {
            OutputWriter.WriteLine("Starting backgrounder");
            if (Backgrounder.IsRunning) { return; }
            Backgrounder.StartAsync().GetAwaiter();
        }

        private void StopBackgrounder()
        {
            OutputWriter.WriteLine("Stopping backgrounder");
            if (!Backgrounder.IsRunning) { return; }
            Backgrounder.Cancel();
        }

        public void Subscribe()
        {

            OutputWriter.WriteLine("Open the Avid, and bring it to the foreground.");
            OutputWriter.WriteLine("Press Alt+A to begin the background typing event.");
            OutputWriter.WriteLine("To cancel the event, press Alt+B.");
            // Note: for the application hook, use the Hook.AppEvents() instead

            m_GlobalHook = Hook.GlobalEvents();

            var map = new Dictionary<Combination, Action>
            {
                { Combination.FromString("Alt+A"), () =>{ StartBackgrounder();  }},
                { Combination.FromString("Alt+B"), () =>{ StopBackgrounder();  }}
            };
            m_GlobalHook.OnCombination(map);

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, WindowsHook.KeyPressEventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            //e.Handled = true;
            OutputWriter.WriteLine($"KeyPress: \t{e.KeyChar}");
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
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