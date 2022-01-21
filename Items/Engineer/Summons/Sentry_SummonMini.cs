using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using TF2_Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TF2_Content.Items.Engineer.Projectiles;

namespace TF2_Content.Items.Engineer.Summons
{

    class Sentry_SummonMini : ModProjectile
    {
        string TextureString = "TF2_Content/Items/Engineer/Summons/SentryGun_mini_head";
        public override string Texture => TextureString;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sentry Gun");
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        float distanceFromTarget;
        bool foundTarget;
        Vector2 targetCenter;


        public override void SetDefaults()
        {
            projectile.height = 20;
            projectile.width = 46;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
            projectile.damage *= 0;

            drawOriginOffsetY = -12;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        int sentryHitPoints = 100;
        static int maxHitPoints = 250;
        int healTimer = 60;
        int healAmount = 50;
        int healthBarTimer = 60;
        private Color gradientA;
        private Color gradientB;


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D LegTexture = mod.GetTexture("Items/Engineer/Summons/SentryGun_lvl_1_Legs");
            spriteBatch.Draw(LegTexture, projectile.Center + new Vector2(-24, 10) - Main.screenPosition, Color.White);
            Texture2D HealthTexture = mod.GetTexture("Items/Engineer/Summons/SentryHealthBar");
            gradientA = new Color(0, 127, 14); //Darker goes here
            gradientB = new Color(0, 124, 20); //Lighter goes here
            if (sentryHitPoints < maxHitPoints || healthBarTimer >= 0)
            {
                float quotient = (float)sentryHitPoints / maxHitPoints;
                spriteBatch.Draw(HealthTexture, projectile.Center + new Vector2(-22, 50) - Main.screenPosition, Color.White);
                quotient = Utils.Clamp(quotient, 0f, 1f);

                Rectangle hitbox = new Rectangle();
                hitbox.X = (int)projectile.Center.X - 20 - (int)Main.screenPosition.X;
                hitbox.Width = 46;
                hitbox.Y = (int)projectile.Center.Y + 53 - (int)Main.screenPosition.Y;
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
            sentryHitPoints -= target.damage;
            healthBarTimer = 60;
        }

        int shotTimer = 15;

        public override void AI()
        {
            if (Collision.SolidCollision(projectile.position + new Vector2(0, 32), projectile.width, projectile.height))
            {
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.velocity = new Vector2(0, 4);
                projectile.velocity *= 1.1f;
            }

            // This is supposed to keep the minion from dying instantly if you have the buff
            Player player = Main.player[projectile.owner];
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Sentry_Buff>());
            }
            if (player.HasBuff(ModContent.BuffType<Sentry_Buff>()))
            {
                projectile.timeLeft = 2;
            }

            //this allows the sentry to draw aggro of simple enemies, like zombies slimes etc. Like the stardust guardian.
            Main.player[projectile.owner].tankPet = projectile.whoAmI;
            Main.player[projectile.owner].tankPetReset = false;

            //this code handles the hitpoint system, if the projectile's health goes below zero it dies, it also has a healing timer.
            if (sentryHitPoints <= 0)
            {
                projectile.Kill();
            }
            if (sentryHitPoints < maxHitPoints)
            {
                healTimer--;
            }
            if (healTimer <= 0)
            {
                sentryHitPoints += healAmount;
                healTimer = 60;
            }
            if (sentryHitPoints > maxHitPoints || sentryHitPoints == maxHitPoints)
            {
                sentryHitPoints = maxHitPoints;
                healTimer = 60;
                healthBarTimer--;
            }

            // the rest of the code handles the shooting part of the sentry gun
            SearchForTargets();
            if (foundTarget)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            shotTimer--;
            Vector2 direction = targetCenter - projectile.Center;
            float speed = 32;
            direction.Normalize();
            //projectile.rotation = direction.ToRotation();
            if(targetCenter.X < projectile.Center.X)
            {
                projectile.spriteDirection = -1;
                drawOffsetX = 2;
            }
            else
            {
                projectile.spriteDirection = 1;
                drawOffsetX = 0;
            }
            projectile.rotation = direction.ToRotation() + (projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            if (shotTimer <= 0)
            {
                Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Sentry_Bullet>(), 150, projectile.knockBack, projectile.owner);
                Main.PlaySound(SoundID.Item11, projectile.Center);
                shotTimer = 15;
            }
        }

        private void SearchForTargets()
        {
            var owner = Main.player[projectile.owner];
            distanceFromTarget = 32;
            targetCenter = projectile.position;
            foundTarget = false;
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, projectile.Center);

                if (between < 32)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, projectile.Center);
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Body_Gore_Mini"), projectile.scale);
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Mini_Muzzle_Gore"), projectile.scale);
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Mini_Light_Gore"), projectile.scale);
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Legs_Gore"), projectile.scale);
            for (int p = Main.rand.Next(20); p >= 0; p--)
            {
                Gore.NewGore(projectile.Center, new Vector2(Main.rand.Next(8), -8), mod.GetGoreSlot("Gores/Sentry_Bullet_Shell"), projectile.scale);
            }
        }
    }
}