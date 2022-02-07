using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TF2_Content.Items.Engineer.Summons
{
    class Teleporter_Summon : ModProjectile
    {
        public override string Texture => "TF2_Content/Items/Engineer/Summons/Teleporter";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.height = 16;
            projectile.width = 36;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
        }

        int teleporterHitPoints = 100;
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
            if (teleporterHitPoints < maxHitPoints || healthBarTimer >= 0)
            {
                float quotient = (float)teleporterHitPoints / maxHitPoints;
                spriteBatch.Draw(HealthTexture, projectile.Center + new Vector2(-26, 16) - Main.screenPosition, Color.White);
                quotient = Utils.Clamp(quotient, 0f, 1f);

                Rectangle hitbox = new Rectangle();
                hitbox.X = (int)projectile.Center.X - 24 - (int)Main.screenPosition.X;
                hitbox.Width = 46;
                hitbox.Y = (int)projectile.Center.Y + 18 - (int)Main.screenPosition.Y;
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
            teleporterHitPoints -= target.damage;
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

            if (projectile.ai[0] == 0 && Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Teleporter_Summon>()] > 1)
            {
                projectile.Kill();
            }

            //just a simple check to see if the projectile owner is alive.

            healTeleporter();

            if (Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<TeleporterExit_Summon>()] == 1)
            {
                Animate();
                spawnDusts();
                TeleportPlayers();
                //This is done so the teleporter only spins when the exit is alive. If the exit isn't alive it won't spin, or spawn dusts.
                //it also won't attempt to look for the teleporter exit in TeleportPlayers(), this should prevent null pointers.
            }
            else
            {
                projectile.frame = 0;
            }
        }

        private void healTeleporter()
        {
            //this just handles the life function of the dispenser
            if (teleporterHitPoints <= 0)
            {
                projectile.Kill();
            }
            if (teleporterHitPoints < maxHitPoints && invulnFrames <= 0)
            {
                healTimer--;
            }
            if (healTimer <= 0)
            {
                teleporterHitPoints += healAmount;
                healTimer = 30;
            }
            if (teleporterHitPoints > maxHitPoints || teleporterHitPoints == maxHitPoints)
            {
                teleporterHitPoints = maxHitPoints;
                healTimer = 30;
                healthBarTimer--;
            }
            if (invulnFrames > 0)
                invulnFrames--;
        }

        private void TeleportPlayers()
        {
            var player = Main.player[projectile.owner];
            Vector2 pos = new Vector2(0, 0);

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                if (Main.projectile[p].type == ModContent.ProjectileType<TeleporterExit_Summon>() && Main.projectile[p].owner == projectile.owner)
                {
                    pos.X = Main.projectile[p].position.X;
                    pos.Y = Main.projectile[p].position.Y + 24;

                    for (int x = 0; x < Main.maxPlayers; x++)
                    {
                        if ((Main.player[x].active && Main.player[x].Hitbox.Intersects(projectile.Hitbox) && Main.player[x].team == player.team && player.team != 0) || Main.player[x] == player && Main.player[x].Hitbox.Intersects(projectile.Hitbox))
                        {
                            Main.player[x].position.X = pos.X;
                            Main.player[x].position.Y = pos.Y - Main.player[x].height;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/teleporter_ready"), projectile.Center);
                        }
                    }
                }
            }
        }

        private void Animate()
        {
            if(++projectile.frameCounter >= 3)
            {
                Dust.NewDustPerfect(projectile.Center - new Vector2(14, 6), ModContent.DustType<Dusts.Teleporter_spin_dust>(), new Vector2(0, -1));
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
        }

        private void spawnDusts()
        {

        }
    }

    class TeleporterExit_Summon : ModProjectile
    {
        public override string Texture => "TF2_Content/Items/Engineer/Summons/Teleporter";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.height = 16;
            projectile.width = 36;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
        }

        int teleporterHitPoints = 100;
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
            if (teleporterHitPoints < maxHitPoints || healthBarTimer >= 0)
            {
                float quotient = (float)teleporterHitPoints / maxHitPoints;
                spriteBatch.Draw(HealthTexture, projectile.Center + new Vector2(-26, 16) - Main.screenPosition, Color.White);
                quotient = Utils.Clamp(quotient, 0f, 1f);

                Rectangle hitbox = new Rectangle();
                hitbox.X = (int)projectile.Center.X - 24 - (int)Main.screenPosition.X;
                hitbox.Width = 46;
                hitbox.Y = (int)projectile.Center.Y + 18 - (int)Main.screenPosition.Y;
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
            teleporterHitPoints -= target.damage;
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

            if (projectile.ai[0] == 0 && Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<TeleporterExit_Summon>()] > 1)
            {
                projectile.Kill();
            }

            //just a simple check to see if the projectile owner is alive.

            healTeleporter();

            if (Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<TeleporterExit_Summon>()] == 1)
            {
                Animate();
                spawnDusts();
                //This is done so the teleporter only spins when the exit is alive. If the exit isn't alive it won't spin, or spawn dusts.
            }
            else
            {
                projectile.frame = 0;
            }
        }

        private void healTeleporter()
        {
            //this just handles the life function of the dispenser
            if (teleporterHitPoints <= 0)
            {
                projectile.Kill();
            }
            if (teleporterHitPoints < maxHitPoints && invulnFrames <= 0)
            {
                healTimer--;
            }
            if (healTimer <= 0)
            {
                teleporterHitPoints += healAmount;
                healTimer = 30;
            }
            if (teleporterHitPoints > maxHitPoints || teleporterHitPoints == maxHitPoints)
            {
                teleporterHitPoints = maxHitPoints;
                healTimer = 30;
                healthBarTimer--;
            }
            if (invulnFrames > 0)
                invulnFrames--;
        }

        private void Animate()
        {
            if (++projectile.frameCounter >= 3)
            {
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
        }

        private void spawnDusts()
        {

        }
    }
}