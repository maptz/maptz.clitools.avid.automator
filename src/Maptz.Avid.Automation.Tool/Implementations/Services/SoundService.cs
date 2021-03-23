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
namespace Maptz.Avid.Automation.Tool
{

    public class SoundService : ISoundService
    {
        private Dictionary<SoundServiceSound, SoundPlayer> SoundPlayers { get; }

        public SoundService()
        {
            SoundPlayers = new Dictionary<SoundServiceSound, SoundPlayer>();
            Initialize();

        }

        private void Initialize()
        {
            SoundPlayers.Add(SoundServiceSound.Start, new SoundPlayer(@"C:\Windows\Media\Speech Misrecognition.wav"));
            SoundPlayers.Add(SoundServiceSound.End, new SoundPlayer(@"C:\Windows\Media\Speech Disambiguation.wav"));
        }

        public void Play(SoundServiceSound sound)
        {
            var found = SoundPlayers.TryGetValue(sound, out SoundPlayer value);
            if (!found) return;
            value.Play();
        }

        public void Dispose()
        {
            foreach (var sp in SoundPlayers.Values)
            {
                sp.Dispose();
            }
            SoundPlayers.Clear();
        }
    }
}