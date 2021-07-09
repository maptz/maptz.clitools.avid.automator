using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Maptz.Editing.Edl;
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



    public class ComplexPatternRepeater
    {
        public class Item
        {
            public string TimeCode { get; set; }
        }

        public IEnumerable<Item> GetItems()
        {
            var edl = new CMX3600Deserializer(Options.Create(new CMX3600DeserializerSettings { IgnoreFps = true }));
            var edlText = File.ReadAllText(@"C:\Users\steph\OneDrive\Desktop\20210702\ASSEMBLIES - ACT 3.02.00.07.FED SEP.01.edl");
            var entries = edl.Read(edlText);

            var retval = new List<Item>();
            foreach (var entry in entries.Where(p => p.ClipName.Contains("V00007341"))) //36-40
            {
                if (string.IsNullOrEmpty(entry.RecordIn)) throw new InvalidOperationException("Timecode should not be empty");
                retval.Add(new Item { TimeCode = entry.RecordIn });
            }

            return retval;
        }

        public async Task<bool> RunAsync(CancellationToken cancellationToken)
        {
            var demoMode = false;
            var keyWaitMs = 50;
            var items = GetItems();
            foreach (var item in items)
            {
                Console.WriteLine(item.TimeCode);
                if (demoMode) continue;
                VirtualKeyCode[] keys = GetPattern(item);
                //Console.WriteLine("Pressing: " + string.Join(",", keys.Select(p=>p.ToString())));
                foreach (var key in keys)
                {

                    InputSimulator sim = new InputSimulator();
                    sim.Keyboard.KeyPress(key);
                    await Task.Delay(keyWaitMs);
                    if (cancellationToken.IsCancellationRequested) return false;
                }
            }
            return true;
        }

        private VirtualKeyCode[] GetPattern(Item item)
        {
            bool reverseMatch = true;
            var pattern = new List<VirtualKeyCode>();
            pattern.Add(VirtualKeyCode.F6); //Clear all tracks. 

            foreach (var c in item.TimeCode)
            {
                if (!char.IsNumber(c)) continue;
                var numpadKey = (VirtualKeyCode)((int)VirtualKeyCode.NUMPAD0 + int.Parse(c.ToString()));
                pattern.Add(numpadKey);
            }
            pattern.Add(VirtualKeyCode.RETURN);
            VirtualKeyCode[] patt;
            if (reverseMatch)
            {
                patt = new VirtualKeyCode[] {
                VirtualKeyCode.F11, //Select video 1
                VirtualKeyCode.VK_G,
                VirtualKeyCode.VK_T, //Select current clip
                VirtualKeyCode.F5, //Select all tracks
                VirtualKeyCode.F8, //Reverse Match frame
                VirtualKeyCode.ESCAPE, //Go to source
                VirtualKeyCode.VK_G, //Clear marks on source
                VirtualKeyCode.VK_I, //Mark in on source
                VirtualKeyCode.F5, //Select all tracks (on source)
                VirtualKeyCode.VK_B,  //overwrite
                VirtualKeyCode.ESCAPE //Go b
            };
            }
            else
            {
                patt = new VirtualKeyCode[] {
                VirtualKeyCode.F11, //Select video 1
                VirtualKeyCode.VK_G,
                VirtualKeyCode.VK_T, //Select current clip
                VirtualKeyCode.F5, //Select all tracks
                VirtualKeyCode.F7, //Match frame
                VirtualKeyCode.F5, //Select all tracks (on source)
                VirtualKeyCode.VK_B,  //overwrite
                VirtualKeyCode.ESCAPE //Go b
            };
            }

            pattern.AddRange(patt);
            var retval = pattern.ToArray();
            return retval;
        }

        private string ConvertKey(string item)
        {
            var split = item.Split('+');
            var ret = new List<string>();
            foreach (var part in split)
            {
                if (part == "Ctrl") { ret.Add("^"); }
                else if (part == "Shift") { ret.Add("+"); }
                else if (part == "Alt") { ret.Add("%"); }
                else ret.Add(part);
            }
            var retval = string.Join("", ret);
            return retval;

        }


    }
}