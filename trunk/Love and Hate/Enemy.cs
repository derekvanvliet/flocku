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
        public float mAvoidPlayerStrength = 1000;
        public float mAvoidEnemyStrength = 1000;
        public float mMinHelpless = 0.5f;
        public Vector2 mVelocity = new Vector2();
        public int mLevel = 0;
        public AnimatedSprite mAnimPlayer0;
        public AnimatedSprite mAnimPlayer1;
        public AnimatedSprite mAnimPlayer2;
        public AnimatedSprite mAnimPlayer3;
        public AnimatedSprite mAnimPlayer4;
        public Player mOwner;
        public float mTimer;
        public float mOwnedTime;
        public float mOwnedDuration;
        public Vector2 mFireDir;

        public enum eEnemyState
        {
            FLOCK = 0,
            FIRE
        }

        public eEnemyState mState = eEnemyState.FLOCK;

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
            mAssetName = "enemy/player0/enemyrunside_0000";
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

            mAnimPlayer0 = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\enemy\\player4", iPlayerFrameRate);
            mAnimPlayer1 = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\player01\\flock\\run_down", iPlayerFrameRate);
            mAnimPlayer2 = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\player01\\flock\\run_down", iPlayerFrameRate);
            mAnimPlayer3 = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\player01\\flock\\run_down", iPlayerFrameRate);
            mAnimPlayer4 = new AnimatedSprite(Game, new Vector2(), 0, mScale.X, 0, "\\enemy\\player4", iPlayerFrameRate);
        }

        public override void DrawSprite(GameTime gameTime)
        {
            if (mOwner != null)
            {
                if (mOwner.m_id == PlayerIndex.One)
                {
                    if (mVelocity.X > 10)
                        this.mAnimPlayer1.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else if (mVelocity.X < -10)
                        this.mAnimPlayer1.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    else if (mVelocity.Y > 10)
                        this.mAnimPlayer1.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else
                        this.mAnimPlayer1.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                }
                else if (mOwner.m_id == PlayerIndex.Two)
                {
                    if (mVelocity.X > 10)
                        this.mAnimPlayer2.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else if (mVelocity.X < -10)
                        this.mAnimPlayer2.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    else if (mVelocity.Y > 10)
                        this.mAnimPlayer2.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else
                        this.mAnimPlayer2.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                }
                else if (mOwner.m_id == PlayerIndex.Three)
                {
                    if (mVelocity.X > 10)
                        this.mAnimPlayer3.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else if (mVelocity.X < -10)
                        this.mAnimPlayer3.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    else if (mVelocity.Y > 10)
                        this.mAnimPlayer3.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else
                        this.mAnimPlayer3.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);

                }
                else if (mOwner.m_id == PlayerIndex.Four)
                {
                    if (mVelocity.X > 10)
                        this.mAnimPlayer4.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else if (mVelocity.X < -10)
                        this.mAnimPlayer4.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    else if (mVelocity.Y > 10)
                        this.mAnimPlayer4.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    else
                        this.mAnimPlayer4.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);

                }
            }
            else
            {
                if (mVelocity.X > 10)
                    this.mAnimPlayer0.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else if (mVelocity.X < -10)
                    this.mAnimPlayer0.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                else if (mVelocity.Y > 10)
                    this.mAnimPlayer0.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                else
                    this.mAnimPlayer0.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);

            }

            //base.DrawSprite(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            float mls = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            mTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (mState == eEnemyState.FLOCK)
            {

                this.mAnimPlayer0.Update(gameTime);
                this.mAnimPlayer1.Update(gameTime);
                this.mAnimPlayer2.Update(gameTime);
                this.mAnimPlayer3.Update(gameTime);
                this.mAnimPlayer4.Update(gameTime);

                // move towards nearest owner
                if (mOwner != null)
                {
                    Vector2 dir = mOwner.mPOI - mPosition;
                    dir.Normalize();

                    mVelocity = mVelocity + mls * (dir * mChaseStrength);

                    if (mTimer - mOwnedTime > mOwnedDuration)
                    {
                        mOwner.mOwnedCount--;
                        mOwner.UnOwnEnemy(this);
                        mOwner = null;
                    }
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
                }

                // move away from nearest predator
                Player predator = GetNearestPredator();

                if (predator != null)
                {
                    Vector2 dir = mPosition - predator.mPosition;
                    if (dir.Length() - Radius - predator.Radius < InterestRadius)
                    {
                        dir.Normalize();

                        mVelocity = mVelocity + mls * (dir * mAvoidPlayerStrength);
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

                        mVelocity = mVelocity + mls * (dir * mAvoidEnemyStrength);
                    }
                }

                Enemy ownedEnemy = GetNearestOwnedEnemy();
                if (ownedEnemy != null)
                {
                    Vector2 dir = mPosition - enemy.mPosition;
                    if (dir.Length() - Radius - enemy.Radius < InterestRadius)
                    {
                        if (dir.Length() - Radius - enemy.Radius < Radius)
                        {
                            if (mOwner != null && enemy.mOwner != null && mOwner != enemy.mOwner)
                            {
                                // destroy both
                                Program.Instance.DestroyEnemy(this);
                                Program.Instance.DestroyEnemy(enemy);
                            }
                            else if (mOwner != null && enemy.mOwner == null && mState != eEnemyState.FIRE && enemy.mState != eEnemyState.FIRE)
                            {
                                // convert enemy
                                mOwner.OwnEnemy(enemy);
                                enemy.SetOwned(mOwner);
                            }
                            else if (mOwner == null && enemy.mOwner != null && mState != eEnemyState.FIRE && enemy.mState != eEnemyState.FIRE)
                            {
                                // convert this
                                enemy.mOwner.OwnEnemy(this);
                                SetOwned(enemy.mOwner);
                            }
                        }
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
            }
            else if (mState == eEnemyState.FIRE)
            {
                if (mVelocity.Length() > mMaxSpeed)
                {
                    mVelocity.Normalize();
                    mVelocity = mVelocity * mMaxSpeed;
                }

                // set position
                mPositionX = mPosition.X + mls * mVelocity.X;
                mPositionY = mPosition.Y + mls * mVelocity.Y;

                if (mPosition.X > Config.Instance.GetAsInt("ScreenWidth") ||
                    mPosition.Y > Config.Instance.GetAsInt("ScreenHeight") ||
                    mPosition.X < 0 ||
                    mPosition.Y < 0)
                {
                    Program.Instance.DestroyEnemy(this);
                }

            }

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
                if (player == mOwner || mOwner == null)
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

        public Enemy GetNearestOwnedEnemy()
        {
            Enemy closest = this;
            float closestDistance = 0f;

            foreach (Enemy enemy in Program.Instance.mEnemies)
            {
                if (enemy.mOwner != null && enemy.mOwner != this.mOwner)
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

        public void SetOwned(Player owner)
        {
            mOwner = owner;
            mOwnedTime = mTimer;
            owner.mOwnedCount++;
            mOwnedDuration = (owner.mOwnedCount * 1000) + 5000;
        }

        public void Fire(Vector2 direction)
        {
            mFireDir = direction;
            mFireDir.Normalize();
            mVelocity = mFireDir * 500;

            mState = eEnemyState.FIRE;
        }
    }
}
