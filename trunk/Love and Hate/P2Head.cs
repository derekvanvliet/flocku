using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class P2Head : PHead
    {
        public P2Head(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "p2_head";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mScale.X = 0.5f;
            mScale.Y = mScale.X;

            mPositionX = Config.Instance.GetAsInt("ScreenWidth") - PixelWidth/2;
            mPositionY = 0 + PixelHeight/2;
        }

    }
}
