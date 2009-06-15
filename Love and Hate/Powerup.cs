using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlockU
{
    public class Powerup : Sprite
    {
        public Powerup(Game game, ContentManager theContentManager)
            : base(game)
        {

        }

        public virtual void EngagePowerup(Player p)
        {
        }
    }
}
