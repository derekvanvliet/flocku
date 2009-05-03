using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class Player : Sprite
    {
        public int mDefaultSize = 64;
        public bool bInitialized = false;
        private bool mIsUsingKeyboard = false;
        public int mOwnedCount = 0;
        List<Enemy> mEnemiesOwned = new List<Enemy>();
        bool mFiring = false;
        float mTimer = 0;
        float mLastShot = 0;
        public int mHealth = Config.Instance.GetAsInt("PlayerHealth");
        public Vector2 mPOI;

        public enum ePlayerState
        {
            WALK = 0,
            IDLE,
            RUN
        }

        private ePlayerState mState = ePlayerState.IDLE;

        public ePlayerState PlayerState 
        {
            get { return mState;  }
            set { mState = value; }
        }

        public int mLevel = 1;
        private float mMaxSpeed = 20000f;
        Vector2 mVelocity = new Vector2();

        private class PlayerMergeList
        {
            public List<Player> players = new List<Player>();

            public void Add(Player p)
            {
                players.Add(p);
            }

            public bool Find(Player p)
            {
                if (players.Contains(p))
                    return true;

                return false;
            }

            public bool AreAllPlayersReadyToMerge()
            {
                foreach (Player p in players)
                {
                    if (!p.IsMergeButtonPressed())
                        return false;
                }

                return true;
            }

            public bool IsAnyoneTryingToBreakTheMerge()
            {
                foreach (Player p in players)
                {
                    if ( p.IsMergeBreakButtonPressed() )
                        return true;
                }

                return false;
            }
        }

        float moveX = 0.0f;
        float moveY = 0.0f;

        float lastMovedX = 0.0f;
        float lastMovedY = 0.0f;

        // Merging properties
        private bool m_bIsMerged = false;

        public bool IsMerged
        {
            get { return m_bIsMerged; }
            set { m_bIsMerged = value; }
        }

        // Index on the dictionary is the merge captain
        private static Dictionary<PlayerIndex, PlayerMergeList> m_PlayerMerges = 
            new Dictionary<PlayerIndex, PlayerMergeList>();

        private static bool IsThisPlayerCaptain(Player p)
        {
            if (m_PlayerMerges.ContainsKey(p.id))
                return true;

            return false;
        }

        public static Vector2 GetAvgDirectionForAllPlayers(Player pCaptain)
        {
            Vector2 result = new Vector2();

            foreach (Player p in Player.m_PlayerMerges[pCaptain.id].players)
            {
                float moveX = GamePad.GetState(p.id).ThumbSticks.Left.X;
                float moveY = -GamePad.GetState(p.id).ThumbSticks.Left.Y;

                Vector2 playerDir = new Vector2(moveX, moveY);

                if (playerDir.Length() > 0)                    
                {
                    playerDir.Normalize();
                    result += playerDir;
                }
            }

            // Get captain movement
            float fCaptainMoveX = GamePad.GetState(pCaptain.id).ThumbSticks.Left.X;
            float fCaptainMoveY = GamePad.GetState(pCaptain.id).ThumbSticks.Left.Y;

            result += new Vector2(fCaptainMoveX, -fCaptainMoveY);

            if (result.Length() > 0)
                result.Normalize();

            return result;
        }

        private static void RemoveMergeList(PlayerIndex captain)
        {
            if ( m_PlayerMerges.ContainsKey(captain) )
            {
                foreach ( Player p in m_PlayerMerges[captain].players )
                {
                    p.IsMerged = false;
                }

                m_PlayerMerges[captain].players.Clear();
                m_PlayerMerges.Remove(captain);
            }
        }

        AnimatedSprite m_idleFrontUpAnim;
        AnimatedSprite m_idleFrontDownAnim;
        
        AnimatedSprite m_runUpAnim;
        AnimatedSprite m_runDownAnim;
        
        static AnimatedSprite m_mergeMonsterAnim;

        public PlayerIndex m_id;
        
        public PlayerIndex id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public Player(Game game, PlayerIndex id, bool bUsingKeyboard) : base(game)
        {
            this.mIsUsingKeyboard = bUsingKeyboard;
            this.m_id = id;

            int iPlayerFrameRate = Config.Instance.GetAsInt("PlayerFrameRate");

            switch (id)
            {
                case PlayerIndex.One:
                    {
                        this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\idle_up", iPlayerFrameRate);
                        this.m_idleFrontDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\idle_up", iPlayerFrameRate);
                        this.m_runUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\run_up", iPlayerFrameRate);
                        this.m_runDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\run_down", iPlayerFrameRate);
                        break;
                    }
                case PlayerIndex.Two:
                    {
                        this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player02\\idle_up", iPlayerFrameRate);
                        this.m_idleFrontDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player02\\idle_down", iPlayerFrameRate);
                        this.m_runUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player02\\run_up", iPlayerFrameRate);
                        this.m_runDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player02\\run_down", iPlayerFrameRate);
                        break;
                    }
                case PlayerIndex.Three:
                    {
                        this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player03\\idle_up", iPlayerFrameRate);
                        this.m_idleFrontDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player03\\idle_down", iPlayerFrameRate);
                        this.m_runUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player03\\run_up", iPlayerFrameRate);
                        this.m_runDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player03\\run_down", iPlayerFrameRate);
                        break;
                    }
                case PlayerIndex.Four:
                    {
                        this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player04\\idle_up",iPlayerFrameRate);
                        this.m_idleFrontDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player04\\idle_down", iPlayerFrameRate);
                        this.m_runUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player04\\run_up", iPlayerFrameRate);
                        this.m_runDownAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player04\\run_down", iPlayerFrameRate);
                        break;
                    }
            }

            if (Player.m_mergeMonsterAnim == null)
                Player.m_mergeMonsterAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\mergemonster", iPlayerFrameRate);                
        }

        protected override void LoadContent()
        {
            base.LoadContent();

			Reset(mDefaultSize);

            //int iPlayerFrameRate = Config.Instance.GetAsInt("PlayerFrameRate");

            //switch (id)
            //{
            //    case PlayerIndex.One:
            //        {
            //            this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\idle\\p1_idle", 8, iPlayerFrameRate);
            //            this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player01\\run\\p1_run", 8, iPlayerFrameRate);
            //            break;
            //        }
            //    case PlayerIndex.Two:
            //        {
            //            this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\IdleFront\\p2_idle", 8, iPlayerFrameRate);
            //            this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\RunLeft\\p2_run", 8, iPlayerFrameRate);
            //            break;
            //        }
            //    case PlayerIndex.Three:
            //        {
            //            this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\IdleFront\\p3_idle", 8, iPlayerFrameRate);
            //            this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\RunLeft\\p3_run", 8, iPlayerFrameRate);
            //            break;
            //        }
            //    case PlayerIndex.Four:
            //        {
            //            this.m_idleFrontUpAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\IdleFront\\p4_idle", 8, iPlayerFrameRate);
            //            this.m_runAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\player\\RunLeft\\p4_run", 8, iPlayerFrameRate);
            //            break;
            //        }
            //}

        }

        //public override void Draw(GameTime gameTime)
        public override void DrawSprite(GameTime gameTime)
        {
            if (mHealth <= 0)
                return;

            if (IsMerged && !Player.IsThisPlayerCaptain(this) )
                return;

            switch (this.PlayerState)
            {
                case ePlayerState.IDLE:
                    if (lastMovedX > 0 && lastMovedY > 0)
                    {
                        this.m_idleFrontUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    }
                    else if (lastMovedX < 0 && lastMovedY >= 0)
                    {
                        this.m_idleFrontUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    }
                    else if (lastMovedX > 0 && lastMovedY < 0)
                    {
                        this.m_idleFrontDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                    }
                    else if (lastMovedX < 0 && lastMovedY < 0)
                    {
                        this.m_idleFrontDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                    }
                    else
                        this.m_idleFrontDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);

                    break;

                case ePlayerState.RUN:
                    {
                        //if (IsMerged)
                        //{
                        //    //Player.m_mergeMonsterAnim.Scale = 2.0f;

                        //    //if (mVelocity.X > 10)
                        //    //    Player.m_mergeMonsterAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        //    //else if (mVelocity.X < -10)
                        //    //    Player.m_mergeMonsterAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        //    //else if (mVelocity.Y > 10)
                        //    //    Player.m_mergeMonsterAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        //    //else if (mVelocity.Y < -10)
                        //    //    Player.m_mergeMonsterAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        //    //else
                        //    //    Player.m_mergeMonsterAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        //}
                        //else
                        //{


                        if (mVelocity.X > 0.5 && IsValueInRange(mVelocity.Y, 0.5f))
                        {
                            this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        }
                        else if (mVelocity.X < -0.5 && IsValueInRange(mVelocity.Y, 0.5f) )
                        {
                            this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        }

                        else if (mVelocity.Y > 0.5 && IsValueInRange(mVelocity.X, 0.5f))
                        {
                            this.m_runDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        }
                        else if (mVelocity.Y < -0.5 && IsValueInRange(mVelocity.X, 0.5f))
                        {
                            this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        }

                        else if (mVelocity.X > 0.5 && mVelocity.Y > 0.5)
                        {
                            this.m_runDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        }
                        else if (mVelocity.X > 0.5 && mVelocity.Y < -0.5)
                        {
                            this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        }
                        else if (mVelocity.X < -0.5 && (mVelocity.Y > 0.5 || IsValueInRange(mVelocity.Y, 1.0f)))
                        {
                            this.m_runDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        }
                        else if (mVelocity.X < -0.5 && mVelocity.Y < -0.5)
                        {
                            this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        }
                        
                        // Should never get to this state but just in case as opposed
                        else
                            this.m_idleFrontDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);

                        //if (mVelocity.X > 10 && mVelocity.Y == 0)
                        //{
                        //    this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        //}
                        //else if (mVelocity.X < -10 && mVelocity.Y == 0)
                        //{
                        //    this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        //}

                        //else if (mVelocity.Y > 10 && mVelocity.X == 0)
                        //{
                        //    this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        //    this.m_runDownAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.FlipHorizontally);
                        //}
                        //else if (mVelocity.Y < -10 && mVelocity.X == 0)
                        //{
                        //    this.m_runUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        //}
                        //else
                        //{
                        //    this.m_idleFrontUpAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
                        //}
                        //}

                        break;
                    }
            }

            AnimatedSpriteManager.Instance.Draw(gameTime);

            base.DrawSprite(gameTime);           
        }

        public void SetState()
        {
            if (IsStopped())
                this.PlayerState = ePlayerState.IDLE;
            else
                this.PlayerState = ePlayerState.RUN;
        }

        public override void Update(GameTime gameTime)
        {
            float mls = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            mTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (mHealth <= 0)
                return;

        	if (!bInitialized)
            {
                ResetPosition();
                bInitialized = true;
                return;
            }

            //Vector2 mergedMoveDirection = new Vector2();

            //if (this.IsMerged)
            //{
            //    if (!Player.IsThisPlayerCaptain(this))
            //    {
            //        base.Update(gameTime);
            //        return;
            //    }
            //    else
            //    {
            //        if (m_PlayerMerges.ContainsKey(this.id))
            //        {
            //            // Check if someone is trying to break the merge which
            //            // anyone in the merge group is allowed to do
            //            if (m_PlayerMerges[this.id].IsAnyoneTryingToBreakTheMerge() || IsMergeBreakButtonPressed())
            //            {
            //                // If yes then this captain and its merge need to be removed from our 
            //                // merge list
            //                Player.RemoveMergeList(this.id);

            //                this.IsMerged = false;

            //                this.Reset(mDefaultSize);
            //            }
            //            else
            //            {
            //                // Set state to always running.  Update the animation here!
            //                //
            //                this.PlayerState = ePlayerState.RUN;
            //                //this.m_runAnim.Update(gameTime);
            //                //Player.m_mergeMonsterAnim.Update(gameTime);

            //                mergedMoveDirection = new Vector2();
            //                mergedMoveDirection = GetAvgDirectionForAllPlayers(this);

            //                // There is always a speed movement applied when the players 
            //                // are merged
            //                //
            //                if (mergedMoveDirection.Length() == 0)
            //                    mergedMoveDirection = new Vector2(1, 1);

            //                this.mVelocity.X = mls * (mergedMoveDirection.X * 800.0f);
            //                this.mVelocity.Y = mls * (mergedMoveDirection.Y * 800.0f);

            //                //mergedMoveDirection *= new Vector2(5, 5);

            //                this.mPositionX += this.mVelocity.X;// mergedMoveDirection.X;
            //                this.mPositionY += this.mVelocity.Y;//mergedMoveDirection.Y;
                            
            //                base.Update(gameTime);

            //                return;
            //            }
            //        }
            //    }
            //}

            // Check for collisions with other players
            foreach (Player p in Program.Instance.GamePlayers)
            {
                // Don't bother checking if this player has collided with itself
                //
                if (p.id == this.id)
                    continue;

                if (CheckForCollision(p))
                {
                    // Case 1 - I am not merged with any other players
                    if ( !IsMerged || !p.IsMerged )
                    {
                        // TODO - Remove for final game!
                        if (IsMergeButtonPressed() && p.IsMergeButtonPressed())
                        {
                            m_bIsMerged = true;

                            // Has a captain been selected?
                            // If yes then add me to the list of merged players for this list
                            if (m_PlayerMerges.ContainsKey(this.id))
                            {
                                m_PlayerMerges[this.id].Add(p);
                                p.IsMerged = true;
                            }
                            // If no then make me the captain
                            else
                            {
                                m_PlayerMerges[this.id] = new PlayerMergeList();
                                
                                p.IsMerged = true;
                                m_PlayerMerges[this.id].Add(p);                                
                            }
                        }
                    }

                    // Case 2 - I am merged with one or more players
                    else
                    {
                        // Another player is trying to join the collective.  Since all parties involved in the
                        // collision will send a collision event only the captain should check if members
                        // of its list have merge keys pressed.  To avoid doing too much processing.
                        //
                       if (Player.IsThisPlayerCaptain(this))
                       {    
                           // Checked if this player is already merged in with this captain.  If so
                           // then don't bother checking if we should merge it.
                           //
                           if (!p.IsMerged)
                           {
                               //bool bAllPlayersReadyToMerge = false;
                               //foreach (Player pMerged in Player.m_PlayerMerges[this.id].players)
                               //{
                                   // All players in the collective will need to have their merge
                                   // buttons pressed as well as the new player wanting to join to add
                                   // the new player

                                   // If all players have their merge buttons pressed
                                   // If yes then add the new player to the merged player list   
                                   //if (!pMerged.IsMergeButtonPressed())
                                   //{
                                     //  bAllPlayersReadyToMerge = false;
                                       //break;
                                   //}
                               //}

                               // Check if all the players who are already merged and the new merged player have their keys
                               // pressed to merge.  If they do then merge them!
                               //
                               if (Player.m_PlayerMerges[this.id].AreAllPlayersReadyToMerge() && p.IsMergeButtonPressed())
                                   Player.m_PlayerMerges[this.id].Add(p);
                           }
                       }
                    }
                }
            }

            SetState();

            switch(this.PlayerState)
            {
                case ePlayerState.IDLE:
                    this.m_idleFrontUpAnim.Update(gameTime);
                    this.m_idleFrontDownAnim.Update(gameTime);
                    break;

                case ePlayerState.RUN:
                    this.m_runUpAnim.Update(gameTime);
                    this.m_runDownAnim.Update(gameTime);
                    break;
            }         
          
 	        List<Enemy> destroy = new List<Enemy>();

            foreach (Enemy e in Program.Instance.mEnemies)
            {
                if (e.mOwner != this)
                {
                    if (CheckForCollision(e))
                    {
                        if (e.PixelWidth < this.PixelWidth)
                        {
                            AudioManager.Instance.PlaySound("MonsterDie");

                            /*
                            AnimatedSpriteEx killAnim = new AnimatedSpriteEx(this.Game);

                            killAnim.Position =
                                new Vector2(e.mPosition.X - (e.PixelWidth * 2),
                                            e.mPosition.Y - (e.PixelHeight * 2));
                            
                            killAnim.Load(this.Game.Content, new Vector2(0, 0), "\\kill anim\\Kill", 8, 20);

                            AnimatedSpriteManager.Instance.Add(killAnim);

                            e.Destroy();
                            destroy.Add(e);
                            */
                            if (e.mState == Enemy.eEnemyState.FLOCK && e.mOwner == null)
                            {
                                e.SetOwned(this);
                                OwnEnemy(e);
                            }
                            else
                            {
                                DamageMe();
                                Program.Instance.DestroyEnemy(e);
                            }

                            Vector2 pos = mPosition;

                            /*
                            PixelWidth += 5;
                            PixelHeight += 5;
                            */

                            //mScale.X *= 1.1f;
                            //mScale.Y = mScale.X;
                            mPosition = pos;
                            //mBounds.Radius = Radius;
                            //mBounds.Center.X = mPositionX;
                            //mBounds.Center.Y = mPositionY;                        
                            this.SetBboxPos(mPosition);


                            this.m_idleFrontUpAnim.Scale = mScale.X;
                            this.m_idleFrontDownAnim.Scale = mScale.X;
                            
                            this.m_runUpAnim.Scale = mScale.X;
                            this.m_runDownAnim.Scale = mScale.X;

                            Program.Instance.mEnemiesKilled++;
                            if (Program.Instance.mEnemiesKilled % 3 == 0)
                            {
                                //if (Program.Instance.mMaxEnemies < 100)
                                //Program.Instance.mMaxEnemies++;
                            }
                        }
                        else
                        {
                            /*
                            e.Destroy();
                            destroy.Add(e);

                            Reset((int)(PixelWidth*0.5f));
                             * */
                        }
                    }
                }
            }

            /*
            foreach (Enemy e in destroy)
            {
                Program.Instance.mEnemies.Remove(e);
            }
             * */

           
            GamePadState state = GamePad.GetState(id);

            if (mIsUsingKeyboard)
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    if (id == PlayerIndex.One)
                    {
                        this.moveY = Keyboard.GetState().IsKeyDown(Keys.W) ? -1 : 0;
                        this.moveX = Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

                        if (moveX == 0)
                            moveX = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 0;

                        if (moveY == 0)
                            moveY = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;
                    }
                    else if (id == PlayerIndex.Two)
                    {
                        this.moveY = Keyboard.GetState().IsKeyDown(Keys.I) ? -1 : 0;
                        this.moveX = Keyboard.GetState().IsKeyDown(Keys.L) ? 1 : 0;

                        if (moveX == 0)
                            moveX = Keyboard.GetState().IsKeyDown(Keys.J) ? -1 : 0;

                        if (moveY == 0)
                            moveY = Keyboard.GetState().IsKeyDown(Keys.K) ? 1 : 0;
                    }

                    mVelocity.X += mls * (moveX * 1000);
                    mVelocity.Y += mls * (moveY * 1000);

                    Vector2 drag = new Vector2(-mVelocity.X, -mVelocity.Y);
                    if (drag.Length() != 0)
                    {
                        drag.Normalize();
                        mVelocity = mVelocity + mls * (drag * (mVelocity.Length()*2));
                    }

                    if (mVelocity.Length() > mMaxSpeed)
                    {
                        mVelocity.Normalize();
                        mVelocity = mVelocity * mMaxSpeed;
                    }

                    // set position
                    mPositionX = mPosition.X + mls * mVelocity.X;
                    mPositionY = mPosition.Y + mls * mVelocity.Y;

                    if (mPositionX + Radius > Config.Instance.GetAsInt("ScreenWidth"))
                        mPositionX = Config.Instance.GetAsInt("ScreenWidth") - Radius;
                    if (mPositionX - Radius < 0)
                        mPositionX = Radius;
                    if (mPositionY + Radius > Config.Instance.GetAsInt("ScreenHeight"))
                        mPositionY = Config.Instance.GetAsInt("ScreenHeight") - Radius;
                    if (mPositionY - Radius < 0)
                        mPositionY = Radius;

                    base.Update(gameTime);
                    return;
                }
            }

            this.moveX = state.ThumbSticks.Left.X;
            this.moveY = state.ThumbSticks.Left.Y;

            state.ThumbSticks.Left.Normalize();

            this.mVelocity.X += mls * (state.ThumbSticks.Left.X*1000);
            this.mVelocity.Y += mls * (-state.ThumbSticks.Left.Y*1000);

            state.ThumbSticks.Right.Normalize();
            Vector2 firedir = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            if (firedir.Length() != 0)
            {
                mFiring = true;
                if (GamePad.GetState(m_id).Triggers.Right > 0)
                {
                    Vector2 defenddir = firedir;
                    defenddir.Y = -defenddir.Y;
                    mPOI = mPosition + (defenddir * 100);
                }
                else
                {
                    mPOI = mPosition + new Vector2(0, 48);
                }
            }
            else
            {
                mPOI = mPosition + new Vector2(0,48);
                mLastShot = 0;
                mFiring = false;
            }

            if (mFiring)
            {
                if (mTimer - mLastShot > 200)
                {
                    mLastShot = mTimer;
                    if (mEnemiesOwned.Count > 0)
                    {
                        firedir.Y = -firedir.Y;
                        mEnemiesOwned[0].mPosition = mPosition;
                        mEnemiesOwned[0].Fire(firedir);
                        UnOwnEnemy(mEnemiesOwned[0]);
                    }
                }
            }


            if (IsButtonPressed(GamePad.GetState(m_id).Buttons.A))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                while (mEnemiesOwned.Count > 0)
                {

                    firedir.X = random.Next(0, 100) - 50;
                    firedir.Y = random.Next(0, 100) - 50;
                    
                    mEnemiesOwned[0].mPosition = mPosition;
                    mEnemiesOwned[0].Fire(firedir);
                    UnOwnEnemy(mEnemiesOwned[0]);
                }
            }

            Vector2 drag2 = new Vector2(-mVelocity.X, -mVelocity.Y);
            if (drag2.Length() != 0)
            {
                drag2.Normalize();
                mVelocity = mVelocity + mls * (drag2 * (mVelocity.Length() * 2));
            }

            if (mVelocity.Length() > mMaxSpeed)
            {
                mVelocity.Normalize();
                mVelocity = mVelocity * mMaxSpeed;
            }

            // set position
            mPositionX = mPosition.X + mls * mVelocity.X;
            mPositionY = mPosition.Y + mls * mVelocity.Y;

            if (mPositionX + Radius > Config.Instance.GetAsInt("ScreenWidth"))
                mPositionX = Config.Instance.GetAsInt("ScreenWidth") - Radius;
            if (mPositionX - Radius < 0)
                mPositionX = Radius;
            if (mPositionY + Radius > Config.Instance.GetAsInt("ScreenHeight"))
                mPositionY = Config.Instance.GetAsInt("ScreenHeight") - Radius;
            if (mPositionY - Radius < 0)
                mPositionY = Radius;

            if (moveX != 0)
                lastMovedX = moveX;

            if (moveY != 0)
                lastMovedY = moveY;

            base.Update(gameTime);
        }

        public bool IsStopped()
        {
            if (mVelocity.Length() < 25)
                return true;

            return false;
        }

        public bool CheckForCollision(Sprite obj)
        {
            return this.Bounds.Intersects(obj.Bounds);
        }

        bool IsButtonPressed(ButtonState btn)
        {
            if (btn == ButtonState.Pressed)
                return true;

            return false;
        }

        public bool IsMergeButtonPressed()
        {
            String sMergeBtn = Config.Instance.GetAsString("PlayerMergeBtn");

            if (!String.IsNullOrEmpty(sMergeBtn))
            {
                if (sMergeBtn.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.A);

                else if (sMergeBtn.Equals("b", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.B);

                else if (sMergeBtn.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.X);

                else if (sMergeBtn.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.Y);

                Trace.WriteLine("Player::IsMergeButtonPressed - Merge button exists but not a valid x-box controll button");
            }

            Trace.WriteLine("Player::IsMergeButtonPressed - No player merge button detailed");
            return false;
        }

        public bool IsMergeBreakButtonPressed()
        {
            String sMergeBtn = Config.Instance.GetAsString("PlayerBreakMergeBtn");

            if (!String.IsNullOrEmpty(sMergeBtn))
            {
                if (sMergeBtn.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.A);

                else if (sMergeBtn.Equals("b", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.B);

                else if (sMergeBtn.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.X);

                else if (sMergeBtn.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    return IsButtonPressed(GamePad.GetState(this.id).Buttons.Y);

                Trace.WriteLine("Player::IsMergeButtonPressed - Merge button exists but not a valid x-box controll button");
            }

            Trace.WriteLine("Player::IsMergeButtonPressed - No player merge button detailed");
            return false;            
        }

        protected override void SetAssetName()
        {
            this.mAssetName = "dot_black";

            base.SetAssetName();
        }

 		public override void Initialize()
        {
            base.Initialize();
        }
	
 		public void Damage()
        {
            mLevel = 1;
        }

        public void Reset(int newPixelWidth)
        {
            if (newPixelWidth < mDefaultSize)
            {
                newPixelWidth = mDefaultSize;
            }
            mScale.X = mPixelScale * newPixelWidth;
            mScale.Y = mScale.X;
            if (m_idleFrontUpAnim != null)
            {
                this.m_idleFrontUpAnim.Scale = mScale.X;
                this.m_runUpAnim.Scale = mScale.X;
                this.m_runDownAnim.Scale = mScale.X;
            }
            mMaxSpeed = 10000f / PixelWidth;

            //float fPlayerBoundingRadius = this.Radius;

            this.SetBboxPos(this.mPosition);

            //if (this.mBounds == null)
            //{
            //    //this.mBounds =
            //      //  new BoundingSphere
            //        //(
            //          //  new Vector3(this.mPosition.X, this.mPosition.Y, 0),
            //            //fPlayerBoundingRadius
            //        //);

            //    this.SetBboxPos(this.mPosition);
            //}
            //else
            //{
            //    mBounds.Center.X = mPositionX;
            //    mBounds.Center.Y = mPositionY;

            //    mBounds.Radius = Radius;
            //}
        }

        public void OwnEnemy(Enemy e)
        {
            mEnemiesOwned.Add(e);
        }

        public void UnOwnEnemy(Enemy e)
        {
            if(mEnemiesOwned.Contains(e))
                mEnemiesOwned.Remove(e);
        }

        public void DamageMe()
        {
            if (mHealth > 0)
            {
                mHealth--;

                if (mHealth == 0)
                {
                    while (mEnemiesOwned.Count > 0)
                    {
                        Enemy e = mEnemiesOwned[0];
                        mOwnedCount--;
                        UnOwnEnemy(e);
                        e.mOwner = null;
                    }

                    Program.Instance.mAlivePlayerCount--;
                }
            }
        }

        public void ResetPosition()
        {
            int width = Config.Instance.GetAsInt("ScreenWidth");
            int height = Config.Instance.GetAsInt("ScreenHeight");

            switch (id)
            {
                case PlayerIndex.One:
                    {
                        mPosition = new Vector2(width * 0.5f * 0.5f, height * 0.5f * 0.5f);
                        break;
                    }
                case PlayerIndex.Two:
                    {
                        mPosition = new Vector2(width * 0.5f + width * 0.5f * 0.5f, height * 0.5f * 0.5f);
                        break;
                    }
                case PlayerIndex.Three:
                    {
                        mPosition = new Vector2(width * 0.5f * 0.5f, height * 0.5f + height * 0.5f * 0.5f);
                        break;
                    }
                case PlayerIndex.Four:
                    {
                        mPosition = new Vector2(width * 0.5f + width * 0.5f * 0.5f, height * 0.5f + height * 0.5f * 0.5f);
                        break;
                    }
            }
        }
    }
}
