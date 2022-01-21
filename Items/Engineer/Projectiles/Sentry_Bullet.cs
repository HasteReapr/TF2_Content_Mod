using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TF2_Content.Items.Engineer.Projectiles
{
    class Sentry_Bullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sentry Bullet");
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 60;

            drawOriginOffsetX = -4;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.timeLeft <= 10)
            {
                projectile.alpha += 25;
            }
        }
    }
}
