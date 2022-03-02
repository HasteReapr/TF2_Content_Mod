using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using TF2_Content.Buffs;
using TF2_Content.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using TF2_Content.Items.Engineer.Projectiles;

namespace TF2_Content.Items.Engineer.Summons
{
    class Sentry_SummonTierTwo : ModProjectile
    {
        string TextureString = "TF2_Content/Items/Engineer/Summons/SentryGun_lvl_2_head";
        public override string Texture => TextureString;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
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
            projectile.height = 32;
            projectile.width = 64;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
            projectile.damage *= 0;
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
        static int maxHitPoints = 1000;
        int healTimer = 30;
        int healAmount = 50;
        int healthBarTimer = 60;
        int invulnFrames = -30;
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


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            sentryHitPoints -= target.damage;
            healthBarTimer = 60;
            if (invulnFrames <= -30)
                invulnFrames = 15;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return invulnFrames <= 0;
        }

        int shotTimer = 8;
        int spawnAmmo = 100;
        bool tierUpgrade = false;

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

            //this kills the current minion if theres one of a higher tier.
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree>()] > 0)
            {
                tierUpgrade = true;
                projectile.Kill();
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
        }

        private void healSentry()
        {
            if (sentryHitPoints <= 0)
            {
                projectile.Kill();
            }
            if (sentryHitPoints < maxHitPoints && invulnFrames <= 0)
            {
                healTimer--;
            }
            if (healTimer <= 0)
            {
                sentryHitPoints += healAmount;
                healTimer = 30;
            }
            if (sentryHitPoints > maxHitPoints || sentryHitPoints == maxHitPoints)
            {
                sentryHitPoints = maxHitPoints;
                healTimer = 30;
                healthBarTimer--;
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
                    Projectile.NewProjectile(projectile.Center, direction * speed, DefaultProjType, 300, projectile.knockBack, projectile.owner);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sentry_shoot2"), projectile.Center); 
                    spawnAmmo--;
                    shotTimer = 8;
                }
                else if (shotTimer <= 0)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sentry_empty"), projectile.Center); 
                    shotTimer = 8;
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
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sentry_shoot2"), projectile.Center);
                if (ProjType.type != ItemID.EndlessMusketPouch)
                {
                    ProjType.stack--;
                }
                shotTimer = 8;
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
                if (!bulletSlot1.IsAir && !tierUpgrade)
                {
                    Item.NewItem(projectile.Center, bulletSlot1.type, bulletSlot1.stack);
                    bulletSlot1.stack = 0;
                }
                if (!bulletSlot2.IsAir && !tierUpgrade)
                {
                    Item.NewItem(projectile.Center, bulletSlot2.type, bulletSlot2.stack);
                    bulletSlot2.stack = 0;
                }
                if (!bulletSlot3.IsAir && !tierUpgrade)
                {
                    Item.NewItem(projectile.Center, bulletSlot3.type, bulletSlot3.stack);
                    bulletSlot3.stack = 0;
                }
                if (!bulletSlot4.IsAir && !tierUpgrade)
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
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Level_2_Muzzle_Gore"), projectile.scale);
            Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Legs_Gore"), projectile.scale);
            for (int p = Main.rand.Next(20); p >= 0; p--)
            {
                Gore.NewGore(projectile.Center, new Vector2(0, -8), mod.GetGoreSlot("Gores/Sentry_Bullet_Shell"), projectile.scale);
            }
        }
    }
}