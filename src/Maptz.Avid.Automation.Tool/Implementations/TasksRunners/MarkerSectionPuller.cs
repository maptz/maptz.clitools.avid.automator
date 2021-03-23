using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace Maptz.Avid.Automation.Tool
{

    public class MarkerSectionPuller : BackgroundTaskRunner
    {
        /* #region Private Methods */
        private async Task TypeSection(Section section)
        {
            var keyWaitMs = Settings.KeyWaitMs;
            foreach (var key in section.In.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";
                    Console.WriteLine("Sending " + send);

                    var isim = new InputSimulator();
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    //SendKeys.Send(send);
                    await Task.Delay(keyWaitMs);
                }
                if (CancellationTokenSource.IsCancellationRequested) return;
            }
            SendKeys.Send("{ENTER}");
            await Task.Delay(keyWaitMs);
            SendKeys.Send("i");
            await Task.Delay(keyWaitMs);
            if (CancellationTokenSource.IsCancellationRequested) return;

            foreach (var key in section.Out.ToString())
            {
                if (char.IsNumber(key))
                {
                    var send = "{NUM" + key + "}";
                    Console.WriteLine("Sending " + send);
                    //SendKeys.Send(send);
                    var isim = new InputSimulator();
                    var i = 96 + int.Parse(key.ToString());
                    var vk = (VirtualKeyCode)i;
                    isim.Keyboard.KeyPress(vk);
                    await Task.Delay(keyWaitMs);
                }
                if (CancellationTokenSource.IsCancellationRequested) return;
            }
            SendKeys.Send("{ENTER}");
            await Task.Delay(keyWaitMs);
            SendKeys.Send("o");
            await Task.Delay(keyWaitMs);
            await Task.Delay(keyWaitMs);
            SendKeys.Send("b");
            if (CancellationTokenSource.IsCancellationRequested) return;
        }
        /* #endregion Private Methods */
        /* #region Protected Methods */
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
        public override async Task<bool> RunAsync()
        {
            if (string.IsNullOrEmpty(Settings.FilePath))
            {
                throw new ArgumentException("FilePath not set.");
            }
            if (!File.Exists(Settings.FilePath))
            {
                throw new ArgumentException($"File does not exist at path '{Settings.FilePath}'.");
            }
            var markers = await this.MarkersReader.ReadFromTextFileAsync(Settings.FilePath);
            var sections = this.MarkerMerger.Merge(markers);
            await TypeSections(sections);
            return false;
        }
        public async Task TypeSections(IEnumerable<Section> sections)
        {
            foreach (var section in sections)
            {
                await this.TypeSection(section);
                if (CancellationTokenSource.IsCancellationRequested) break;
            }
        }
        /* #endregion Public Methods */
    }
}