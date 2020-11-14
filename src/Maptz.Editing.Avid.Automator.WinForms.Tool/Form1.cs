using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            await Task.Delay(1000);

            var KeyWaitMs = 200;


            //2
            var isim = new InputSimulator();

            //Setup state is no in out V1 selected



            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);


            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_8);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_T);

            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);

            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_2);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_B);

            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

            isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_8);
            isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);

            //isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);

            //isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);

            await Task.Delay(3000);


            //SendKeys.Send(send);

        }
    }
}
