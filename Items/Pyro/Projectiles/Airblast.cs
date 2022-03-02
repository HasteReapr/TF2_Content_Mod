using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TF2_Content.Items.Pyro.Projectiles
{
    class Airblast : ModProjectile
    {
        public override string Texture => "TF2_Content/Items/empty";

        public override void SetDefaults()
        {
            projectile.height = 144;
            projectile.width = 78;
            projectile.friendly = true;
            projectile.timeLeft = 5;
            projectile.tileCollide = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
    }
}
