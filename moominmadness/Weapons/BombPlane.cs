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
    class BombPlane
    {
        private Texture2D texture;
        private Vector2 position, origin;
        private List<Grenade> playerOneGrenadeList;
        private List<Grenade> playerTwoGrenadeList;
        private SpriteEffects spriteEffect;
        private float direction;

        private double shootTimer, shootTimerReset;

        private int bulletsShot;
        public bool canRemove;

        private float topMaxSpawn, botMaxSpawn;
        private Random random = new Random();
        bool isPlayerOne;

        public BombPlane(bool isPlayerOne, ref List<Grenade> playerOneGrenadeList, ref List<Grenade> playerTwoGrenadeList)
        {
            texture = TextureManager.planeShadow;
            this.playerOneGrenadeList = playerOneGrenadeList;
            this.playerTwoGrenadeList = playerTwoGrenadeList;
            this.isPlayerOne = isPlayerOne;
            topMaxSpawn = 260;
            botMaxSpawn = 920;

            if (isPlayerOne)
            {
                this.position = new Vector2(-500, (botMaxSpawn - topMaxSpawn) / 2);
                this.spriteEffect = SpriteEffects.None;
                this.direction = 1;
            }
            else
            {
                this.position = new Vector2(2400, (botMaxSpawn - topMaxSpawn) / 2);
                this.spriteEffect = SpriteEffects.FlipHorizontally;
                this.direction = -1;

            }

            origin = new Vector2(texture.Width / 2, 0);
            shootTimer = shootTimerReset = 200;

        }

        public void Update(GameTime gameTime)
        {

            position += new Vector2(direction, 0) * 6;

            if (isPlayerOne && position.X >= 0 || !isPlayerOne && position.X <= 1920)
                shootTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;


            if (shootTimer <= 0)
            {
                int randomYPos = random.Next((int)topMaxSpawn, (int)botMaxSpawn);
                if (position.X < (1920 / 2))
                    playerTwoGrenadeList.Add(new Bomb(TextureManager.standardBullet, new Vector2(position.X, randomYPos), 0));
                else
                    playerOneGrenadeList.Add(new Bomb(TextureManager.standardBullet, new Vector2(position.X, randomYPos), 0));

                bulletsShot++;
                shootTimer = shootTimerReset;

                //if (AudioManager.sound)
                //    AudioManager.bombDropSound.Play();
            }

            if (isPlayerOne && position.X >= 2200)
                canRemove = true;
            if (!isPlayerOne && position.X <= -200)
                canRemove = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, new Color(Color.White, 0.6f), 0, origin, 1, spriteEffect, 0.9f);
        }
    }
}
