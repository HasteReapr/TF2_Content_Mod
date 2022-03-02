using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using TF2_Content.Buffs;
using TF2_Content.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using System;
using TF2_Content.Items.Engineer.Projectiles;

namespace TF2_Content.Items.Engineer.Summons
{
    class Sentry_SummonTierThree : ModProjectile
    {
        string TextureString = "TF2_Content/Items/Engineer/Summons/SentryGun_lvl_3_head";
        public override string Texture => TextureString;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            DisplayName.SetDefault("Sentry Gun");
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        float distanceFromTarget;
        bool foundTarget;
        Vector2 targetCenter;


        public override void SetDefaults()
        {
            projectile.height = 32;
            projectile.width = 64;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
            projectile.damage *= 0;

            drawOriginOffsetY = -30;
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
        static int maxHitPoints = 1500;
        int healTimer = 60;
        int healTime = 30;
        int healAmount = 50;
        int healthBarTimer = 60;
        private Color gradientA;
        private Color gradientB;


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D LegTexture = mod.GetTexture("Items/Engineer/Summons/SentryGun_lvl_1_Legs");
            spriteBatch.Draw(LegTexture, projectile.Center + new Vector2(-24, 4) - Main.screenPosition, Color.White);
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

        int shotTimer = 8;
        int spawnAmmo = 100;
        int rocketTimer = 120;
        int inBetweenRocket = 15;
        int rockets = 4;
        int buffAmmount = 0;
        int invulnFrames = -30;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            sentryHitPoints -= (int)(target.damage * (buffAmmount * 0.05f));
            healthBarTimer = 60;
            if (invulnFrames <= -30)
                invulnFrames = 15;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return invulnFrames <= 0;
        }


        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = Main.LocalPlayer.GetModPlayer<TF2_Player>();
                modPlayer.SentryHealth = sentryHitPoints;
                modPlayer.SentryHealthMax = maxHitPoints;
                modPlayer.SentrySpawnAmmo = 100;
                modPlayer.SentryCurrentAmmo = spawnAmmo;
            }
            invulnFrames--;
            if (Collision.SolidCollision(projectile.position + new Vector2(0, 22), projectile.width, projectile.height))
            {
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.velocity = new Vector2(0, 4);
                projectile.velocity *= 1.1f;
            }

            // This is supposed to keep the minion from dying instantly if you have the buff
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
            healSentry();

            //this bit of code is supposed to handle when the player right clicks on the sentry gun.
            ChestCheck();

            // the rest of the code handles the shooting part of the sentry gun
            SearchForTargets();
            if (foundTarget)
            {
                Shoot();
            }

            BuffSentry();
        }

        private void healSentry()
        {
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
                healTimer = healTime;
            }
            if (sentryHitPoints > maxHitPoints || sentryHitPoints == maxHitPoints)
            {
                sentryHitPoints = maxHitPoints;
                healTimer = healTime;
                healthBarTimer--;
            }
        }
        int projDamage = 300;
        int shotTime = 8;
        private void BuffSentry()
        {
            Player player = Main.player[projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree_Buff>()] > 0)
            {
                buffAmmount = player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree_Buff>()];
                maxHitPoints = (int)(1500 * (1 + (buffAmmount * 0.1f)));
                projDamage = (int)(300 * (1 + (buffAmmount * 0.25f)));
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree_Buff>()] >= 4)
            {
                shotTime = 4;
                healTime = 15;
            }
            else if(player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree_Buff>()] >= 6)
            {
                shotTime = 2;
            }
        }

        private void ChestCheck()
        {
            Player player = new Player();
            if (Main.mouseRight && Main.mouseRightRelease && projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && player.whoAmI == Main.myPlayer)
            {
                SentryUI s = new SentryUI();
                SentryUI.Visible = true;
                s.SentryGun = new UIImage(ModContent.GetTexture("TF2_Content/Items/Engineer/Summons/SentryGun_lvl_1_head"));
            }
        }

        int DefaultProjType = ModContent.ProjectileType<Sentry_Bullet>();
        Item ProjType;
        public Item bulletSlot1 => ModContent.GetInstance<TF2_Content>().SentryUI._vanillaItemSlot1.Item;
        public Item bulletSlot2 => ModContent.GetInstance<TF2_Content>().SentryUI._vanillaItemSlot2.Item;
        public Item bulletSlot3 => ModContent.GetInstance<TF2_Content>().SentryUI._vanillaItemSlot3.Item;
        public Item bulletSlot4 => ModContent.GetInstance<TF2_Content>().SentryUI._vanillaItemSlot4.Item;

        private void Shoot()
        {
            shotTimer--;
            rocketTimer--;
            Vector2 direction = targetCenter - projectile.Center;
            float speed = 32;
            direction.Normalize();
            projectile.rotation = direction.ToRotation() + (projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            if (targetCenter.X < projectile.Center.X)
            {
                projectile.spriteDirection = -1;
                drawOffsetX = 2;
            }
            else
            {
                projectile.spriteDirection = 1;
                drawOffsetX = 0;
            }
            if (!bulletSlot1.IsAir)
            {
                ProjType = bulletSlot1;
            }
            else if (!bulletSlot2.IsAir)
            {
                ProjType = bulletSlot2;
            }
            else if (!bulletSlot3.IsAir)
            {
                ProjType = bulletSlot3;
            }
            else if (!bulletSlot4.IsAir)
            {
                ProjType = bulletSlot4;
            }
            else
            {
                if (shotTimer <= 0 && spawnAmmo > 0)
                {
                    Projectile.NewProjectile(projectile.Center, direction * speed, DefaultProjType, projDamage, projectile.knockBack, projectile.owner);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sentry_shoot3"), projectile.Center);
                    spawnAmmo--;
                    shotTimer = shotTime;
                }
                else if (shotTimer <= 0)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sentry_empty"), projectile.Center);
                    shotTimer = shotTime;
                }
            }

            if (shotTimer <= 0)
            {
                if (ProjType.type == ItemID.MusketBall || ProjType.type == ItemID.EndlessMusketPouch)
                {
                    Projectile.NewProjectile(projectile.Center, direction * speed, DefaultProjType, 300, projectile.knockBack, projectile.owner);
                }
                else
                {
                    Projectile.NewProjectile(projectile.Center, direction * speed, ProjType.shoot, 300, projectile.knockBack, projectile.owner);
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sentry_shoot3"), projectile.Center);
                if (ProjType.type != ItemID.EndlessMusketPouch)
                {
                    ProjType.stack--;
                }
                shotTimer = shotTime;
            }

            if (rocketTimer <= 0)
            {
                inBetweenRocket--;
                if (inBetweenRocket <= 0)
                {
                    Projectile.NewProjectile(projectile.Center, direction * 16, ModContent.ProjectileType<Sentry_Missile>(), 300, projectile.knockBack, projectile.owner);
                    Main.PlaySound(SoundID.Item61, projectile.Center);
                    rockets--;
                    inBetweenRocket = 5;
                    if (rockets <= 0)
                    {
                        rockets = 4;
                        inBetweenRocket = 120;
                    }
                }
            }

            if (++projectile.frameCounter >= 8)
            {
                if (++projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }

        private void SearchForTargets()
        {
            var owner = Main.player[projectile.owner];
            targetCenter = projectile.position;
            foundTarget = false;

            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float npcDistance = Vector2.Distance(npc.Center, projectile.Center);
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > npcDistance;
                        float between = Vector2.Distance(npc.Center, projectile.Center);

                        if (closest || !foundTarget)
                        {
                            bool closeThroughWall = npcDistance < 100f;
                            bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

                            if ((lineOfSight || closeThroughWall) && between < 1280)
                            {
                                targetCenter = npc.Center;
                                foundTarget = true;
                            }
                            else if (between > 1600)
                            {
                                foundTarget = false;
                            }
                        }
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (player.whoAmI == Main.myPlayer)
            {
                SentryUI.Visible = false;
                if (!bulletSlot1.IsAir)
                {
                    Item.NewItem(projectile.Center, bulletSlot1.type, bulletSlot1.stack);
                    bulletSlot1.stack = 0;
                }
                if (!bulletSlot2.IsAir)
                {
                    Item.NewItem(projectile.Center, bulletSlot2.type, bulletSlot2.stack);
                    bulletSlot2.stack = 0;
                }
                if (!bulletSlot3.IsAir)
                {
                    Item.NewItem(projectile.Center, bulletSlot3.type, bulletSlot3.stack);
                    bulletSlot3.stack = 0;
                }
                if (!bulletSlot4.IsAir)
                {
                    Item.NewItem(projectile.Center, bulletSlot4.type, bulletSlot4.stack);
                    bulletSlot4.stack = 0;
                }
            }
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
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Body_Gore2"), projectile.scale);
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Level_3_Muzzle_Gore"), projectile.scale);
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Legs_Gore"), projectile.scale);
            for (int p = Main.rand.Next(20); p >= 0; p--)
            {
                Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Bullet_Shell"), projectile.scale);
            }
        }
    }

    class Sentry_SummonTierThree_Buff : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.damage = 0;
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = 5;
            projectile.minion = true;
            projectile.minionSlots = 1f;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            // this bit here is the alive conditions, if the player is dead, or has the buff, or the level 3 sentry doesnt exist
            #region lifeConditions
            Player player = Main.player[projectile.owner];
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Sentry_Buff>());
            }
            if (player.HasBuff(ModContent.BuffType<Sentry_Buff>()))
            {
                projectile.timeLeft = 2;
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree>()] < 1)
            {
                projectile.Kill();
            }
            #endregion

            //this code is supposed to make the projectile follow behind the player
            #region idleMovement
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f;
            float minionPositionOffsetX = (10 + projectile.minionPos * 40) * -player.direction;
            idlePosition.X += minionPositionOffsetX;
            float speed = 8f;
            float inertia = 20f;
            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                projectile.position = idlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            float overlapVelocity = 0.04f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                {
                    if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
                    else projectile.velocity.X += overlapVelocity;

                    if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
                    else projectile.velocity.Y += overlapVelocity;
                }
            }

            if (distanceToIdlePosition > 600f)
            {
                speed = 12f;
                inertia = 60f;
            }
            else
            {
                speed = 4f;
                inertia = 80f;
            }
            if (distanceToIdlePosition > 20f)
            {
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;
                projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (projectile.velocity == Vector2.Zero)
            {
                projectile.velocity.X = -0.15f;
                projectile.velocity.Y = -0.05f;
            }

            #endregion

            //animation
            if (++projectile.frameCounter >= 4)
            {
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            Gore.NewGore(projectile.Center, new Vector2(0, 4), mod.GetGoreSlot("Gores/Sentry_Drone_Gore"), projectile.scale);
        }
    }
}