using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class PowerupBurst : Powerup
    {

        AnimatedSprite mAnim;

        public PowerupBurst(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {
            mAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\powerup_burst", Config.Instance.GetAsInt("PlayerFrameRate"));

        }

        protected override void SetAssetName()
        {
            mAssetName = "powerup_burst";
        }

        public override void EngagePowerup(Player p)
        {
            p.mPowerupBurst = true;
            p.mPowerupHoming = false;
            base.EngagePowerup(p);
        }

        public override void Update(GameTime gameTime)
        {
            mAnim.Update(gameTime);

            base.Update(gameTime);
        }

        public override void DrawSprite(GameTime gameTime)
        {
            mAnim.Draw(gameTime, this.mPosition - Vector2.One * Radius, SpriteEffects.None);
        }
    }
}
