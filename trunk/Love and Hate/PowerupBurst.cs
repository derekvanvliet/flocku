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
        public PowerupBurst(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "powerup_burst";
        }

        public override void EngagePowerup(Player p)
        {
            p.mPowerupBurst = true;
            base.EngagePowerup(p);
        }
    }
}
