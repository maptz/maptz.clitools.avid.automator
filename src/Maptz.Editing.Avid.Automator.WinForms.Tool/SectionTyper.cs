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
using WindowsInput;
using WindowsInput.Native;

namespace Maptz.Editing.Avid.Automator.WinForms.Tool
{

    public class SectionTyper : ISectionTyper
    {
       

        public SectionTyper()
        {


        }

        public int KeyWaitMs = 150;
        public async Task DoTyping(IEnumerable<Section> sections)
        {
            foreach (var section in sections)
            {
                await this.DoTyping(section);
            }
        }

        public async Task DoTyping(Section section)
        {

            foreach (var key in section.In.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";
                    Console.WriteLine("Sending " + send);
                    
                    var isim = new InputSimulator();
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    //SendKeys.Send(send);
                    await Task.Delay(KeyWaitMs);
                }
            }
            SendKeys.Send("{ENTER}");
            await Task.Delay(KeyWaitMs);
            SendKeys.Send("i");
            await Task.Delay(KeyWaitMs);

            foreach (var key in section.Out.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";
                    Console.WriteLine("Sending " + send);
                    //SendKeys.Send(send);
                    var isim = new InputSimulator();
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    await Task.Delay(KeyWaitMs);
                }
            }
            SendKeys.Send("{ENTER}");
            await Task.Delay(KeyWaitMs);
            SendKeys.Send("o");
            await Task.Delay(KeyWaitMs);
            await Task.Delay(KeyWaitMs);
            SendKeys.Send("b");
        }
    }
}