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

    public class PatternRepeater
    {
        public VirtualKeyCode[] GetPattern1()
        {
            return new VirtualKeyCode[] { VirtualKeyCode.VK_S, VirtualKeyCode.VK_V };
        }

        public VirtualKeyCode[] GetPattern3()
        {
            return new VirtualKeyCode[] { VirtualKeyCode.VK_G, VirtualKeyCode.VK_T };
        }

        public async Task<bool> RunAsync(CancellationToken cancellationToken)
        {
            var keyWaitMs = 1000;
            var MAX_REPEAT_COUNT = 2;
            var pattern = GetPattern3();
            for (int i = 0; i < MAX_REPEAT_COUNT; i++)
            {
                foreach (var item in pattern)
                {
                    //var convertedPattern = ConvertItem(item);
                    Console.WriteLine("Sending keys " + item);
                    //SendKeys.Send(convertedPattern);
                    var inputSim = new InputSimulator();
                    inputSim.Keyboard.KeyPress(item);
                    await Task.Delay(keyWaitMs);
                    if (cancellationToken.IsCancellationRequested) return false;
                }
            }

            return true;
        }


    }
}