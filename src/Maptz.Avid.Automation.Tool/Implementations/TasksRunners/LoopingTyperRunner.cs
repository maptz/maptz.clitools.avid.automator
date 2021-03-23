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
using WindowsInput;

namespace Maptz.Avid.Automation.Tool
{

    public class LoopingTyperRunner : BackgroundTaskRunner
    {
        public LoopingTyperRunner(SoundService soundService, OutputWriter  outputWriter)
        {
            SoundService = soundService;
            OutputWriter = outputWriter;
        }

        protected override async Task OnCompleted(bool hasCancelled)
        {
            OutputWriter.WriteLine($"Task completed. Was cancelled {hasCancelled}");
            SoundService.Play(SoundServiceSound.End);
            await base.OnCompleted(hasCancelled);
        }

        protected override async Task<bool> OnStarting()
        {
            OutputWriter.WriteLine("Starting task.");
            SoundService.Play(SoundServiceSound.Start);
            return await base.OnStarting();   
        }

        public SoundService SoundService { get; }
        public OutputWriter OutputWriter { get; }

        public override async Task<bool> RunAsync()
        {
            OutputWriter.WriteLine("Do something");

            var KeyWaitMs = 200;

            var length = 10;
            var isim = new InputSimulator();
            for (int i = 0; i < length; i++)
            {
                SystemSounds.Exclamation.Play();
                SystemSounds.Asterisk.Play();

                //Deselect All tracks
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

                //Select V1
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_8);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

                //Select current set
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_T);

                //Select All tracks
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);

                //Matchframe
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_2);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

                //Activate record again
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

                //Deselect V1
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_8);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

                //Back to source
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

                //Overwrite
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_B);

                //Back to record
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

                //Deselect all to reset to previous.
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

                //Select V1*b\
                isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_8);
                isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

                //isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);

                //isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);

                await Task.Delay(3000);
            }

            return false;
        }

    }
}