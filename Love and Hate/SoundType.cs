using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace flocku
{
    public class SoundType
    {
        public WaveBank  wb;
        public SoundBank sb;
        public Song      sg;

        public SoundType(WaveBank wb)
        {
            this.wb = wb;
        }

        public SoundType(SoundBank sb)
        {
            this.sb = sb;
        }

        public SoundType(Song sg)
        {
            this.sg = sg;
        }
    }
}
