using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class P3Head : PHead
    {
        public P3Head(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "p3_head";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mPositionX = 0 + PixelWidth/2;
            mPositionY = Config.Instance.GetAsInt("ScreenHeight") - PixelHeight / 2;
        }

    }
}
