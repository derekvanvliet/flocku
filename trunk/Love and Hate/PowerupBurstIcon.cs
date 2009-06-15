using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlockU
{
    public class PowerupBurstIcon : Sprite
    {
        public PowerupBurstIcon(Game game, ContentManager theContentManager)
            : base(game)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "powerup_burst/pickup_green_0000";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mScale.X = 0.5f;
            mScale.Y = mScale.X;
        }
    }
}
