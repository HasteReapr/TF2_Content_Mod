using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Medic.Projectiles
{
    class Syringe : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.timeLeft = 120;
            projectile.friendly = true;

            drawOffsetX = -17;
            drawOriginOffsetY = -4;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.velocity.Y += 0.5f;
        }
    }
}
