using System;
using AtlusLibSharp.Utilities;
using System.Media;
using System.IO;
using System.Windows.Forms;

namespace Amicitia.AudioViewer
{
    public partial class AudioPlayer : Form
    {
        public AudioPlayer(SoundPlayer sound)
        {
            InitializeComponent();
            Stream = sound.Stream;
            SoundPlayer = sound;
            AudioHelper.ContinuePlaying(sound);
        }

        private void roundedPlayButton_Click(object sender, EventArgs e)
        {
            using (EndiannessReader reader = new EndiannessReader((MemoryStream)Stream))
            { }
        }

        private Stream _Stream;
        public Stream Stream
        {
            get { return _Stream; }
            private set { _Stream = value; }
        }

        private SoundPlayer _SoundPlayer;
        public SoundPlayer SoundPlayer
        {
            get { return _SoundPlayer; }
            private set { _SoundPlayer = value; }
        }
    }
}
