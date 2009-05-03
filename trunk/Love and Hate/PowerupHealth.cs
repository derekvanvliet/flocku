using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class PowerupHealth : Powerup
    {
        public PowerupHealth(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {

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
    }
}
