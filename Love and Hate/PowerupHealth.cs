using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlockU
{
    public class PowerupHealth : Powerup
    {
        AnimatedSprite mAnim;

        public PowerupHealth(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {
            mAnim = new AnimatedSprite(Game, new Vector2(0, 0), 0, mScale.X, 0, "\\powerup_health", Config.Instance.GetAsInt("PlayerFrameRate"));

        }

        protected override void SetAssetName()
        {
            mAssetName = "powerup_health";
        }

        public override void EngagePowerup(Player p)
        {
            base.EngagePowerup(p);

            p.mHealth += 25;
            if (p.mHealth > Config.Instance.GetAsInt("PlayerHealth"))
            {
                p.mHealth = Config.Instance.GetAsInt("PlayerHealth");
            }
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
