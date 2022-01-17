using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TF2_Content.Items.Demo
{
    class Grenade_Pill : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.height = 4;
            projectile.width = 4;
            drawOffsetX = -6;
            drawOriginOffsetY = -3;

            projectile.timeLeft = 120;
        }

        public override void AI()
        {
            projectile.rotation = projectile.timeLeft/4;
            projectile.velocity.Y += 0.1f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(projectile.ai[0] == 0)
            {
                projectile.damage = (int)(projectile.damage * 0.75f);
                if (projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                {
                    projectile.velocity.X = oldVelocity.X * -0.9f;
                }
                if (projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                {
                    projectile.velocity.Y = oldVelocity.Y * -0.9f;
                }
                projectile.ai[0]++;
            }
            return false;
        }
    }
}
