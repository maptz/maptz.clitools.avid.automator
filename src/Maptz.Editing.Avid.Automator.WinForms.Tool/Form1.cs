﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace Maptz.Editing.Avid.Automator.WinForms.Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateMousePos();

            /*
             * 
             * T
             * Ctrl+A
             * Shift+2
             * B
             * Esc
             * Ctrl+Shift+A
             * 8
             * S
             * T
             */
        }

        public async Task UpdateMousePos()
        {
            while (this.Enabled)
            {
                var pos = Win32.GetMousePosition();
                this.label1.Text = pos.ToString();
                await Task.Delay(100);
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Delay(2000);

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
            //2

            //Setup state is no in out V1 selected



        


            //SendKeys.Send(send);

        }
    }
}
