using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MoominMadness
{
    class FlowerGrenade : Grenade
    {
        private float flowerRotation;
        public Color color;

        public FlowerGrenade(Texture2D texture, Vector2 position, float directionX, Color color) : base(texture, position, directionX, false)
        {
            this.color = color;
            flowerRotation = 0;
        }

        public override void Update(GameTime gameTime)
        {
            flowerRotation += 0.3f;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, shadowPosition, null, new Color(0, 0, 0, 120), 0f, origin, shadowScale, SpriteEffects.None, 0.1f);

            spriteBatch.Draw(texture, position, null, color, flowerRotation, origin, 1, spriteEffect, FloatLayerCalculator());
        }
    }
}
