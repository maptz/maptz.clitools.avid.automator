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

    public class MarkerSectionPuller
    {
        /* #region Private Methods */
        private async Task TypeSection(Section section, CancellationToken cancellationToken)
        {

            var keyWaitMs = Settings.KeyWaitMs;
            foreach (var key in section.In.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";

                    var isim = new InputSimulator();
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    //SendKeys.Send(send);
                    await Task.Delay(keyWaitMs);
                }
                if (cancellationToken.IsCancellationRequested) return;
            }
            SendKeys.Send("{ENTER}");
            await Task.Delay(keyWaitMs);
            SendKeys.Send("i");
            await Task.Delay(keyWaitMs);
            if (cancellationToken.IsCancellationRequested) return;

            foreach (var key in section.Out.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";

                    //SendKeys.Send(send);
                    var isim = new InputSimulator();
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    await Task.Delay(keyWaitMs);
                }
                if (cancellationToken.IsCancellationRequested) return;
            }
            SendKeys.Send("{ENTER}");
            await Task.Delay(keyWaitMs);
            SendKeys.Send("o");
            await Task.Delay(keyWaitMs);
            await Task.Delay(keyWaitMs);
            SendKeys.Send("b");
            if (cancellationToken.IsCancellationRequested) return;
        }
        /* #endregion Private Methods */
        /* #region Protected Methods */

        /* #endregion Protected Methods */
        /* #region Public Properties */
        public IMarkerMerger MarkerMerger { get; }
        public IMarkersReader MarkersReader { get; }
        public IOutputWriter OutputWriter { get; }
        public MarkerSectionPullerSettings Settings { get; }
        public ISoundService SoundService { get; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public MarkerSectionPuller(ISoundService soundService, IOutputWriter outputWriter, IMarkersReader markersReader, IMarkerMerger markerMerger, IOptions<MarkerSectionPullerSettings> settings)
        {
            SoundService = soundService;
            OutputWriter = outputWriter;
            MarkersReader = markersReader;
            MarkerMerger = markerMerger;
            Settings = settings.Value;
        }
        /* #endregion Public Constructors */
        /* #region Public Methods */
        public async Task<bool> RunAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Settings.FilePath))
            {
                throw new ArgumentException("FilePath not set.");
            }
            if (!File.Exists(Settings.FilePath))
            {
                throw new ArgumentException($"File does not exist at path '{Settings.FilePath}'.");
            }

            IEnumerable<Section> sections;
            if (Settings.Mode == PullMode.Markers)
            {
                OutputWriter.WriteLine($"Reading markers from file");
                IEnumerable<Marker> markers;
                try
                {
                    markers = await this.MarkersReader.ReadFromTextFileAsync(Settings.FilePath);
                }
                catch(Exception ex)
                {
                    OutputWriter.WriteLine($"Error: " + ex.ToString());
                    return false;
                }
                
                OutputWriter.WriteLine($"Merging markers");
                sections = this.MarkerMerger.Merge(markers);
                OutputWriter.WriteLine($"Merging markers: {markers.Count()} markers -> {sections.Count()} sections.");
            }
            else if (Settings.Mode == PullMode.AvidDS)
            {
                var converter = new AvidDSToSectionsConverter();
                sections = converter.Convert(Settings.FilePath, SmpteFrameRate.Smpte25);
                OutputWriter.WriteLine($"Reading DS sections: {sections.Count()} sections.");
            }
            else throw new NotSupportedException();

            OutputWriter.WriteLine("Starting task.");
            SoundService.Play(SoundServiceSound.Start);

            await TypeSections(sections, cancellationToken);

            var hasCancelled = cancellationToken.IsCancellationRequested;
            if (!hasCancelled)
            {
                OutputWriter.WriteLine($"Task completed.");
            }
            else
            {
                OutputWriter.WriteLine($"Task cancelled.");
            }

            SoundService.Play(SoundServiceSound.End);

            return true;
        }
        public async Task TypeSections(IEnumerable<Section> sections, CancellationToken cancellationToken)
        {
            var sectionNum = 0;
            foreach (var section in sections)
            {
                sectionNum++;
                OutputWriter.WriteLine($"Typing section {sectionNum} of {sections.Count()}.");
                Console.Title = $"Maptz Avid Automation Tool ({sectionNum} of {sections.Count()})";
                await this.TypeSection(section, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Canncellation requested");
                    break;
                }
            }
            Console.Title = $"Maptz Avid Automation Tool";
        }
        /* #endregion Public Methods */
    }
}