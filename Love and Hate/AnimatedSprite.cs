using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Threading;

// Take from http://msdn.microsoft.com/en-us/library/bb203866.aspx
//

namespace FlockU
{
    public class AnimatedSprite 
    {
        private int framecount;
        private List<Texture2D> FrameTextures = new List<Texture2D>();
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;
        private string Asset;
        //private int FrameCount;
        private int FramesPerSec;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;
        public Game GameObj;

        SpriteBatch sb;

        public enum eAnimationType
        {
            KILL,
            INFINITE
        }

        public enum eAnimationEffect
        {
            NONE,
            FLASH
        }

        eAnimationEffect mAnimEffect = eAnimationEffect.NONE;

        public eAnimationType AnimationType { get; set; }

        public AnimatedSprite(Game game, Vector2 origin, float rotation, float scale, float depth, string asset, int framesPerSec)
        {
            this.GameObj = game;
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;

            this.Asset        = asset;
            this.framecount   = 0;
            this.FramesPerSec = framesPerSec;
            this.AnimationType = eAnimationType.INFINITE;

            Load(game.Content, Asset, FramesPerSec);
        }

        public void Load(ContentManager content, string asset, int framesPerSec)
        {
            sb = Program.Instance.mSpriteBatch;

            String sFullAssetName = content.RootDirectory + asset;
                
            // Increase frame count based on number of files in directory
            foreach (String s in Directory.GetFiles(sFullAssetName, "*.*", SearchOption.TopDirectoryOnly))
            {
                FrameTextures.Add(content.Load<Texture2D>(Path.Combine(sFullAssetName, Path.GetFileNameWithoutExtension(s))));
                this.framecount++;
            }
            
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
               return;

            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {
                switch (AnimationType)
                {
                    case eAnimationType.KILL:
                    {
                        if (Frame < this.framecount - 1)
                            Frame++;

                        
                        break;
                    }

                    case eAnimationType.INFINITE:
                    {
                        if (Frame < this.framecount - 1)
                            Frame++;
                        else
                            Frame = 0;

                        break;
                    }
                }

                //Frame++;
                
                // Keep the Frame between 0 and the total frames, minus one.
                //Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        public void Draw(GameTime gameTime, Vector2 pos, SpriteEffects effects)
        {
            if (sb != null)
            {
                // TODO: Add your drawing code here
                sb.Begin();

                DrawFrame(sb, pos, effects);

                sb.End();
            }

            //base.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (maxElapseLoops > 0)
            {
                timeElapsed += this.GameObj.TargetElapsedTime.Milliseconds;

                if (timeElapsed > 250)
                {
                    alpha--;

                    if (alpha == 0)
                        alpha = 255;

                    //Color[] texData = new Color[FrameTextures[Frame].Width * FrameTextures[frame].Height];
                    //FrameTextures[frame].GetData<Color>(texData);

                    //for (int i = 0; i < texData.Length; i++)
                    //{
                    //    if (texData[i].A != 0)
                    //        texData[i].A -= 5;
                    //}

                    //Texture2D tex = new Texture2D(this.GameObj.GraphicsDevice, FrameTextures[frame].Width, FrameTextures[frame].Height);

                    //tex.SetData<Color>(texData);

                    //batch.Draw(tex, screenPos, null, Color.White, 0, new Vector2(), this.Scale, effects, 0);//, Rotation, Origin, Scale, SpriteEffects.None, Depth);

                    timeElapsed = 0;
                    maxElapseLoops--;
                }
            }

            if (this.IsPaused)
                Play();

            UpdateFrame(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, SpriteEffects effects)
        {
            DrawFrame(batch, Frame, screenPos, effects);
        }

        Byte alpha          = 255;
        long timeElapsed    = 0;
        public int maxElapseLoops = 0;


        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos, SpriteEffects effects)
        {
            int FrameWidth = FrameTextures[frame].Width;// / framecount;
            
            //Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0, FrameWidth, FrameTextures[frame].Height);
            Rectangle sourcerect = new Rectangle(FrameWidth, 0, FrameWidth, FrameTextures[frame].Height);

            // If state is flashing

            if (maxElapseLoops > 0)
            {
                //timeElapsed += this.GameObj.TargetElapsedTime.Milliseconds;

                if (timeElapsed > 100)
                {
                    //alpha--;

                    //if (alpha == 0)
                      //  alpha = 255;

                    Color[] texData = new Color[FrameTextures[frame].Width * FrameTextures[frame].Height];
                    FrameTextures[frame].GetData<Color>(texData);

                    for (int i = 0; i < texData.Length; i++)
                    {
                        if (texData[i].A != 0)
                            texData[i].A -= 5;
                    }

                    Texture2D tex = new Texture2D(this.GameObj.GraphicsDevice, FrameTextures[frame].Width, FrameTextures[frame].Height);

                    tex.SetData<Color>(texData);

                    batch.Draw(tex, screenPos, null, Color.White, 0, new Vector2(), this.Scale, effects, 0);//, Rotation, Origin, Scale, SpriteEffects.None, Depth);

                    //timeElapsed = 0;
                    //maxElapseLoops--;
                }
            }
            else
                batch.Draw(FrameTextures[frame], screenPos, null, Color.White, 0, new Vector2(), this.Scale, effects, 0);//, Rotation, Origin, Scale, SpriteEffects.None, Depth);

            //batch.Draw(FrameTextures[frame], screenPos, null, Color.White, 0, new Vector2(), this.Scale, effects, 0);//, Rotation, Origin, Scale, SpriteEffects.None, Depth);
            //batch.Draw(FrameTextures[frame], screenPos, sourcerect, Color.White, Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
