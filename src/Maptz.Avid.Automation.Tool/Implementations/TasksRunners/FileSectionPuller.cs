using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
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

    public class FileSectionPuller
    {
        public FileSectionPuller( IOutputWriter outputWriter, IOptions<FileSectionPullerSettings> settings)
        {
            OutputWriter = outputWriter;
            Settings = settings.Value;
        }

        public FileSectionPullerSettings Settings { get; }
        public IOutputWriter OutputWriter { get; }

        private void TypeSection(Section section, CancellationToken cancellationToken)
        {
            var isim = new InputSimulator();
            var keyWaitMs = Settings.KeyWaitMs;
            foreach (var key in section.In.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";

                    
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    //SendKeys.Send(send);
                    Task.Delay(keyWaitMs).GetAwaiter().GetResult();
                }
                if (cancellationToken.IsCancellationRequested) return;
            }
            isim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            //SendKeys.Send("{ENTER}");
            Task.Delay(keyWaitMs).GetAwaiter().GetResult();
            isim.Keyboard.KeyPress(VirtualKeyCode.VK_I);
            //SendKeys.Send("i");
            Task.Delay(keyWaitMs).GetAwaiter().GetResult();
            if (cancellationToken.IsCancellationRequested) return;

            foreach (var key in section.Out.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";

                    //SendKeys.Send(send);
                    
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    Task.Delay(keyWaitMs).GetAwaiter().GetResult();
                }
                if (cancellationToken.IsCancellationRequested) return;
            }
            isim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            //SendKeys.Send("{ENTER}");
            Task.Delay(keyWaitMs).GetAwaiter().GetResult();
            //SendKeys.Send("o");
            isim.Keyboard.KeyPress(VirtualKeyCode.VK_O);
            Task.Delay(keyWaitMs).GetAwaiter().GetResult();
            Task.Delay(keyWaitMs).GetAwaiter().GetResult();
            //SendKeys.Send("b");
            isim.Keyboard.KeyPress(VirtualKeyCode.VK_B);
            if (cancellationToken.IsCancellationRequested) return;
        }

        public void TypeSections(IEnumerable<Section> sections, CancellationToken cancellationToken)
        {
            var sectionNum = 0;
            foreach (var section in sections)
            {
                sectionNum++;
                OutputWriter.WriteLine($"Typing section {sectionNum} of {sections.Count()}.");
                //Console.Title = $"Maptz Avid Automation Tool ({sectionNum} of {sections.Count()})";
                this.TypeSection(section, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Canncellation requested");
                    break;
                }
            }
            //Console.Title = $"Maptz Avid Automation Tool";
        }

        public void PullFromFile(string filePath)
        {
            var sections = ReadSectionsFromFile(filePath);
            TypeSections(sections, new CancellationTokenSource().Token);
        }

        private IEnumerable<Section> ReadSectionsFromFile(string filePath)
        {
            var retval = new List<Section>();
            var contents = File.ReadAllText(filePath);
            var lines = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var isProcessing = true; //No need for a prefix
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("====")) { isProcessing = true; continue; }
                if (!isProcessing) continue;

                var frameRate = SmpteFrameRate.Smpte25;
                var parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.None);
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