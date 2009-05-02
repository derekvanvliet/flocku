using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class Enemy : Sprite
    {
        public float mMaxSpeed = 4000f;
        public float mChaseStrength = 500;
        public float mAvoidStrength = 1000;
        public float mMinHelpless = 0.5f;
        public Vector2 mVelocity = new Vector2();
        public int mLevel = 0;
        public AnimatedSprite mAttackSide;
        public AnimatedSprite mRunSide;
        public Player mOwner;
        public enum eEnemyState
        {
            ATTACK = 0,
            RUN
        }

        private eEnemyState mState = eEnemyState.ATTACK;

        public eEnemyState EnemyState
        {
            get { return mState; }
            set { mState = value; }
        }



        public Enemy(Game game, ContentManager theContentManager) : base(game)
        {
            
        }

        protected override void SetAssetName()
        {
            mAssetName = "enemy/AttackSide/enemyattack_0000";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();


            // init scale
            Random random = new Random((int)DateTime.Now.Ticks);

            mScale.X = 1.0f;
            mScale.Y = mScale.X;

            //this.mBounds =


              //  new BoundingSphere
                //(
                  //  new Vector3(this.mPosition.X, this.mPosition.Y, 0),
//                    this.Radius
                //);
            this.SetBboxPos(this.mPosition);
            

            // init position
            switch (random.Next(1, 5))
            {
                case 1: // left
                    {
                        mPositionX = -Radius;
                        mPositionY = random.Next(0, Config.Instance.GetAsInt("ScreenHeight"));
                        break;
                    }
                case 2: // top
                    {
                        mPositionX = random.Next(0, Config.Instance.GetAsInt("ScreenWidth"));
                        mPositionY = -Radius;
                        break;
                    }
                case 3: // right
                    {
                        mPositionX = Config.Instance.GetAsInt("ScreenWidth") + Radius;
                        mPositionY = random.Next(0, Config.Instance.GetAsInt("ScreenHeight"));
                        break;
                    }
                case 4: // bottom
                    {
                        mPositionX = random.Next(0, Config.Instance.GetAsInt("ScreenWidth"));
                        mPositionY = Config.Instance.GetAsInt("ScreenHeight") + Radius;
                        break;
                    }
            }
            int iPlayerFrameRate = Config.Instance.GetAsInt("PlayerFrameRate");

            mAttackSide = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\enemy\\AttackSide\\enemyattack", 8, iPlayerFrameRate);
            mRunSide = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\enemy\\RunSide\\enemyrunside", 8, iPlayerFrameRate);


        }

        public override void DrawSprite(GameTime gameTime)
        {
            if (EnemyState == eEnemyState.RUN)
            {
                if (mVelocity.X > 10)
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else if (mVelocity.X < -10)
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                else if (mVelocity.Y > 10)
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else
                    this.mRunSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
            }
            else
            {
                if (mVelocity.X > 10)
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else if (mVelocity.X < -10)
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                else if (mVelocity.Y > 10)
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else
                    this.mAttackSide.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
            }
            base.DrawSprite(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            
            float mls = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            this.mAttackSide.Update(gameTime);
            this.mRunSide.Update(gameTime);

            // move towards nearest owner
            if (mOwner != null)
            {
                Vector2 dir = mOwner.mPosition - mPosition;
                dir.Normalize();

                mVelocity = mVelocity + mls * (dir * mChaseStrength);
                EnemyState = eEnemyState.ATTACK;
            }
            else
            {
                // no owner, so move towards current point of interest

                Vector2 dir = Program.Instance.GetCurrentPointOfInterest() - mPosition;

                if (dir.Length() > InterestRadius)
                {
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mChaseStrength);
                }
                EnemyState = eEnemyState.RUN;
            }

            // move away from nearest predator
            Player predator = GetNearestPredator();

            if (predator != null)
            {
                Vector2 dir = mPosition - predator.mPosition;
                if (dir.Length() - Radius - predator.Radius < InterestRadius)
                {
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mAvoidStrength);
                }
            }

            // move away from nearest enemy
            Enemy enemy = GetNearestEnemy();

            if (enemy != this)
            {               
                Vector2 dir = mPosition - enemy.mPosition;
                if (dir.Length() - Radius - enemy.Radius < InterestRadius)
                {
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mAvoidStrength);
                }
            }

            // add drag
            Vector2 drag = new Vector2(-mVelocity.X, -mVelocity.Y);
            if (drag.Length() != 0)
            {
                drag.Normalize();
                mVelocity = mVelocity + mls * (drag * (mVelocity.Length() * 2));
            }

            if (mVelocity.Length() > mMaxSpeed)
            {
                mVelocity.Normalize();
                mVelocity = mVelocity * mMaxSpeed;
            }

            // set position
            mPositionX = mPosition.X + mls * mVelocity.X;
            mPositionY = mPosition.Y + mls * mVelocity.Y;

            
            base.Update(gameTime);
        }

        public Player GetNearestPrey()
        {
            Player closest = null;
            float closestDistance = 0f;

            foreach (Player player in Program.Instance.GamePlayers)
            {
                if (player.PixelWidth < PixelWidth)
                {
                    if (closestDistance == 0)
                    {
                        closest = player;
                        Vector2 distance = player.mPosition - mPosition;
                        closestDistance = distance.Length();
                    }
                    else
                    {
                        Vector2 distance = player.mPosition - mPosition;
                        if (distance.Length() < closestDistance)
                        {
                            closest = player;
                            closestDistance = distance.Length();
                        }
                    }
                }
            }

            return closest;
        }

        public Player GetNearestPredator()
        {
            Player closest = null;
            float closestDistance = 0f;

            foreach (Player player in Program.Instance.GamePlayers)
            {
                if (player.PixelWidth > PixelWidth)
                {
                    if (closestDistance == 0)
                    {
                        closest = player;
                        Vector2 distance = player.mPosition - mPosition;
                        closestDistance = distance.Length();
                    }
                    else
                    {
                        Vector2 distance = player.mPosition - mPosition;
                        if (distance.Length() < closestDistance)
                        {
                            closest = player;
                            closestDistance = distance.Length();
                        }
                    }
                }
            }

            return closest;
        }

        public Enemy GetNearestEnemy()
        {
            Enemy closest = this;
            float closestDistance = 0f;

            foreach (Enemy enemy in Program.Instance.mEnemies)
            {
                if (closestDistance == 0)
                {
                    closest = enemy;
                    Vector2 distance = enemy.mPosition - mPosition;
                    closestDistance = distance.Length();
                }
                else
                {
                    Vector2 distance = enemy.mPosition - mPosition;
                    if (distance.Length() - Radius - enemy.Radius < closestDistance)
                    {
                        closest = enemy;
                        closestDistance = distance.Length();
                    }
                }
            }

            return closest;
        }

        private int GetHelplessEnemyCount()
        {
            Player smallest = Program.Instance.GetSmallestPlayer();
            int count = 0;
            foreach (Enemy e in Program.Instance.mEnemies)
            {
                if (e.PixelWidth < smallest.PixelWidth)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
