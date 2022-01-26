using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content.Buffs;

namespace TF2_Content.Items.Medic.Projectiles
{
    public class Medigun_Projectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 2;
            projectile.friendly = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target.active && target.team == Main.player[projectile.owner].team && Main.player[projectile.owner].team != 0)
            {
                target.statLife += projectile.damage / 60;
                if (!Main.player[projectile.owner].HasBuff(ModContent.BuffType<Ubercharge>()))
                    Main.player[projectile.owner].GetModPlayer<MedicPlayer>().CurrentUber += 0.02f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //purely for debugging in singleplayer
            target.life += projectile.damage / 60;
            if (!Main.player[projectile.owner].HasBuff(ModContent.BuffType<Ubercharge>()))
                Main.player[projectile.owner].GetModPlayer<MedicPlayer>().CurrentUber += 1f;
        }

        /*public override bool? CanHitNPC(NPC target)
        {
            return false;
        }*/

        private const float MOVE_DISTANCE = 30;

        public float Distance
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
                projectile.velocity, 10, projectile.damage, -1.57f, 1f, 50, Color.White, (int)MOVE_DISTANCE);
            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;

            // Draws the laser 'body'
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                var origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 18, 16, 32), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }

            // Draws the laser 'tail'
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 16, 18), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            // Draws the laser 'head'
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 16, 20), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            Vector2 unit = projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
                player.Center + unit * Distance, 22, ref point);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Player player = Main.player[projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Medigun_Holdout>()] > 0)
            {
                projectile.timeLeft = 2;
            }

            SetLaserPosition(player);
            UpdatePlayer(player);
            //stopOnCursor();
        }

        private void healPlayer()
        {
            Rectangle beamHitBox = new Rectangle();
            beamHitBox.X = projectile.width;
            beamHitBox.Y = projectile.height;
            
        }

        private void stopOnCursor()
        {
            //somehow figure out when the lazer has hit the mouse and stop drawing
            if(projectile.owner == Main.myPlayer)
            {
                Distance = Main.MouseWorld.X;
            }
        }

        private void SetLaserPosition(Player player)
        {
            if (Main.myPlayer == projectile.owner)
            for (Distance = MOVE_DISTANCE; Distance <= Main.MouseWorld.X; Distance += 5f)
            {
                var start = player.Center + projectile.velocity * Distance;
                if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
            }
        }

        private void UpdatePlayer(Player player)
        {
            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
                Distance = Main.MouseWorld.X;
            }
            int dir = projectile.direction;
            player.ChangeDir(dir); // Set player direction to where we are shooting
            player.heldProj = projectile.whoAmI; // Update player's held projectile
            player.itemTime = 2; // Set item time to 2 frames while we are used
            player.itemAnimation = 2; // Set item animation time to 2 frames while we are used
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir); // Set the item rotation to where we are shooting
        }
    }
}