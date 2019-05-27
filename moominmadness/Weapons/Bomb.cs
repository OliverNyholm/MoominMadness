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
    class Bomb : Grenade
    {
        public Bomb(Texture2D texture, Vector2 position, float directionX) : base(texture, position, directionX, false)
        {
            this.groundPosition = position.Y;
            this.position = position + new Vector2(0, -1000);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(position.X, groundPosition + 40), null, new Color(0, 0, 0, 120), 0f, origin, shadowScale, SpriteEffects.None, 0.1f);

            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1, spriteEffect, 0.5f);
        }
    }
}
