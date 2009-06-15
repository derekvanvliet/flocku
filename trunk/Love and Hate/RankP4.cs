using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlockU
{
    public class RankP4 : Sprite
    {
        public RankP4(Game game, ContentManager theContentManager)
            : base(game)
        {
        }

        protected override void SetAssetName()
        {
            mAssetName = "p4_idle_0000";
        }
    }
}
