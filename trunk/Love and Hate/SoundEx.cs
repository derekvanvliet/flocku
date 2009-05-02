using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class SoundEx
    {
        public enum eSoundType
        {
            LOOP,
            KILL
        }

        public eSoundType mSoundType = eSoundType.KILL;

        public eSoundType SoundType
        {
            get { return mSoundType; }
            set { mSoundType = value; }
        }

        public SoundEx(eSoundType eType)
        {

        }
    }
}
