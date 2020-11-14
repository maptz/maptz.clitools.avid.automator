using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
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
    public class Form2Model
    {
        public string FilePath { get; set; }
        public IEnumerable<Section> Sections { get; set; }




    }

    public partial class Form2 : Form
    {
        public Form2Model Model { get; }
        public ISectionTyper SectionTyper { get; }
        public IMarkerMerger MarkerMerger { get; }
        public IMarkersReader MarkersReader { get; }

        public Form2(ISectionTyper sectionTyper, IMarkerMerger markerMerger, IMarkersReader markersReader)
        {
            InitializeComponent();
            this.Model = new Form2Model();
            SectionTyper = sectionTyper;
            MarkerMerger = markerMerger;
            MarkersReader = markersReader;
        }

      

            private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Model.FilePath))
            {
                this.Log("No file specified.");
                return;
            }

            this.Log($"Pressing keys for all sections in file {this.Model.FilePath}");


            System.Media.SystemSounds.Asterisk.Play();

            try
            {
                await this.PressKeys();
            }
            catch(Exception ex)
            {
                this.Log(ex.ToString());
            }
            

        }

        private async Task PressKeys()
        {
            var sections = this.Model.Sections;
            await Task.Delay(5000);

            var sectionTyper = new SectionTyper();
            await sectionTyper.DoTyping(sections);
        }

        private void Log(string v)
        {
            this.textBox1.Text += v + Environment.NewLine;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog odf = new OpenFileDialog();
            var dr = odf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.Model.FilePath = odf.FileName;
                await this.ReadSectionsFromFileAsync();
            }
            else
            {
                this.Log($"Open file cancelled.");
            }
        }

        public async Task ReadSectionsFromFileAsync()
        {

            this.Log($"Opening markers file '{this.Model.FilePath}'");

            var markers = await this.MarkersReader.ReadFromTextFileAsync(this.Model.FilePath);
            var sections = this.MarkerMerger.Merge(markers);
            this.Model.Sections = sections;

            this.Log($"Found '{this.Model.Sections.Count()}' sections.");

        }

    }
}
