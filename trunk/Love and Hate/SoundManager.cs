using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using flocku;
using System.Diagnostics;

namespace WindowsGame1
{
    class SoundManager
    {
        static SoundManager me = new SoundManager();
        static Dictionary<String, SoundType> mSounds = new Dictionary<string, SoundType>();

        private AudioEngine mAudioEngine;

        public AudioEngine Engine
        {
            get { return mAudioEngine; }
        }

        public static SoundManager Instance
        {
            get { return me; }
        }

        public void Initialize(ContentManager contentMgr, String sAssetName)
        {
            mAudioEngine = new AudioEngine(contentMgr.RootDirectory + sAssetName);
        }

        // Load content in the game project!
        public bool Play(String sAsset)
        {
            try
            {
                if (!mSounds.ContainsKey(sAsset))
                {
                    throw new Exception("Asset " + sAsset + "does not exist!");
                }

                if (mSounds[sAsset].sg != null)
                    MediaPlayer.Play(mSounds[sAsset].sg);

                else if (mSounds[sAsset].sb != null)
                    mSounds[sAsset].sb.PlayCue(sAsset);
            }
            catch(Exception ex)
            {
                throw;
            }

            return true;
        }

        public void Update()
        {
            mAudioEngine.Update();
        }

        public void Register(String sAssetName, Song aSound)
        {
            mSounds[sAssetName] = new SoundType(aSound);
        }

        public void Register(String sAsset, WaveBank aBank)
        {
            mSounds[sAsset] = new SoundType(aBank);
        }

        public void Register(String sAsset, SoundBank aBank)
        {
            mSounds[sAsset] = new SoundType(aBank);
        }            
    }
}
