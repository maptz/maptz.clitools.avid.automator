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

    public class PatternRepeater
    {
        public string[] Pattern { get; set; } = new string[] { "S", "V" };

       

        public async Task<bool> RunAsync(CancellationToken cancellationToken)
        {
            var keyWaitMs = 200;
            var MAX_REPEAT_COUNT = 50;
            for (int i = 0; i < MAX_REPEAT_COUNT; i++)
            {
                foreach (var item in Pattern)
                {
                    SendKeys.Send(item);
                    await Task.Delay(keyWaitMs);
                    if (cancellationToken.IsCancellationRequested) return false;
                }
            }
            
            return true;
        }

    }
}