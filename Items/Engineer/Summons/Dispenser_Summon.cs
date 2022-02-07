using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TF2_Content.Items.Engineer.Summons
{
    class Dispenser_Summon : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.height = 52;
            projectile.width = 48;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
        }

        int HealRate = 50;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target == Main.player[projectile.owner])
            {
                target.statLife += HealRate / 60;
            }
        }

        int dispenserHitPoints = 100;
        static int maxHitPoints = 500;
        int healTimer = 30;
        int healAmount = 50;
        int healthBarTimer = 60;
        int invulnFrames = 5;
        private Color gradientA;
        private Color gradientB;


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D HealthTexture = mod.GetTexture("Items/Engineer/Summons/SentryHealthBar");
            gradientA = new Color(0, 127, 14); //Darker goes here
            gradientB = new Color(0, 124, 20); //Lighter goes here
            if (dispenserHitPoints < maxHitPoints || healthBarTimer >= 0)
            {
                float quotient = (float)dispenserHitPoints / maxHitPoints;
                spriteBatch.Draw(HealthTexture, projectile.Center + new Vector2(-22, 32) - Main.screenPosition, Color.White);
                quotient = Utils.Clamp(quotient, 0f, 1f);

                Rectangle hitbox = new Rectangle();
                hitbox.X = (int)projectile.Center.X - 20 - (int)Main.screenPosition.X;
                hitbox.Width = 46;
                hitbox.Y = (int)projectile.Center.Y + 34 - (int)Main.screenPosition.Y;
                hitbox.Height = 6;

                int left = hitbox.Left;
                int right = hitbox.Right;
                int steps = (int)((right - left) * quotient);
                for (int i = 0; i < steps; i += 1)
                {
                    float percent = (float)i / (right - left);
                    spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
                }
            }
            return true;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            dispenserHitPoints -= target.damage;
            healthBarTimer = 60;
            invulnFrames = 15;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (invulnFrames <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int timeer = 0;
        int playerHealTimer = 60;
        public override void AI()
        {
            timeer++;
            if (projectile.ai[0] == 1 && timeer >= 5)
                projectile.ai[0] = 0;

            if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.velocity = new Vector2(0, 4);
                projectile.velocity *= 1.1f;
            }

            var player = Main.player[projectile.owner];
            if (player.active)
                projectile.timeLeft = 2;

            if (projectile.ai[0] == 0 && Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Dispenser_Summon>()] > 1)
            {
                projectile.Kill();
            }

            //just a simple check to see if the projectile owner is alive.

            healDispenser();
            if(--playerHealTimer <= 0)
            {
                HealPlayers();
                playerHealTimer = 30;
            }
        }

        private void healDispenser()
        {
            //this just handles the life function of the dispenser
            if (dispenserHitPoints <= 0)
            {
                projectile.Kill();
            }
            if (dispenserHitPoints < maxHitPoints && invulnFrames <= 0)
            {
                healTimer--;
            }
            if (healTimer <= 0)
            {
                dispenserHitPoints += healAmount;
                healTimer = 30;
            }
            if (dispenserHitPoints > maxHitPoints || dispenserHitPoints == maxHitPoints)
            {
                dispenserHitPoints = maxHitPoints;
                healTimer = 30;
                healthBarTimer--;
            }
            if (invulnFrames >= 0)
                invulnFrames--;
        }

        private void HealPlayers()
        {
            Player player = Main.player[projectile.owner];
            for (int x = 0; x < Main.maxPlayers; x++)
            {
                if (((Main.player[x].active && Main.player[x].Hitbox.Intersects(projectile.Hitbox) && Main.player[x].team == player.team && player.team != 0) || Main.player[x] == player && Main.player[x].Hitbox.Intersects(projectile.Hitbox)) && Main.player[x].statLife < Main.player[x].statLifeMax2)
                {
                    Main.player[x].statLife += 3;
                    Main.player[x].HealEffect(3);
                }
            }
        }
    }
}