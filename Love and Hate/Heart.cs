using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Love_and_Hate
{
    public class Heart : Sprite
    {
        public Heart(Game game, ContentManager theContentManager)
            : base(game)
        {

        }

        protected override void SetAssetName()
        {
            mAssetName = "heart";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mScale.X = 0.5f;
            mScale.Y = mScale.X;
        }
    }
}
