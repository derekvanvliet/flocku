using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class P1Head : PHead
    {
        public P1Head(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "p1_head";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mPositionX = 0 + PixelWidth/2;
            mPositionY = 0 + PixelHeight/2;
        }
    }
}
