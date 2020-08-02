using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maptz.Editing.Avid.Automator.WinForms.Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateMousePos();
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

    }
}
