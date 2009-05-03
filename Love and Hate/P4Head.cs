﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class P4Head : PHead
    {
        public P4Head(Game game, ContentManager theContentManager)
            : base(game, theContentManager)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "p4_head";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mPositionX = Config.Instance.GetAsInt("ScreenWidth") - PixelWidth / 2;
            mPositionY = Config.Instance.GetAsInt("ScreenHeight") - PixelHeight / 2;
        }

    }
}
