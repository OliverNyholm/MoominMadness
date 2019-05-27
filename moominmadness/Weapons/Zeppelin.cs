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
    class Zeppelin
    {
        private Texture2D texture;
        private Vector2 position;
        private List<Bullet> bulletList;
        private SpriteEffects spriteEffect;
        private float direction;

        private double shootTimer, shootTimerReset;
        private double animationTimer, animationTimerReset;
        private int animationFrame;
        private Rectangle animationBox;
        private int width;

        private int bulletsShot;
        public bool canDelete;
        
        public Zeppelin(Vector2 position, ref List<Bullet> bulletList, SpriteEffects spriteEffect, float direction)
        {
            texture = TextureManager.zeppelinTexture;
            this.position = position;
            this.bulletList = bulletList;
            this.spriteEffect = spriteEffect;
            this.direction = direction;

            width = texture.Width / 2;
            animationBox = new Rectangle(0, 0, width, texture.Height);
            shootTimer = shootTimerReset = 1300;
            animationTimer = animationTimerReset = 50;
        }

        public void Update(GameTime gameTime)
        {
            shootTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            animationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if(shootTimer <= 0)
            {
                bulletList.Add(new Bullet(TextureManager.standardBullet, position + new Vector2(0, 20), direction));
                bulletsShot++;
                shootTimer = shootTimerReset;
            }

            if (animationTimer <= 0)
            {
                animationTimer = animationTimerReset;
                animationFrame++;
                animationBox.X = (animationFrame % 2) * width;
            }

            if (bulletsShot == 4)
                canDelete = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, animationBox, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, spriteEffect, FloatLayerCalculator());
        }

        protected float FloatLayerCalculator()
        {
            return 0 + position.Y * 0.0005f;
        }
    }
}
