using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Spy.Projectiles
{
    class Sapper_Projectile : ModProjectile
    {
        public override string Texture => "TF2_Content/Items/Spy/Sapper";
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.penetrate = int.MaxValue;
        }

        bool rotate = true;
        bool attack = false;
        int rotation = 360;

        public override void AI()
        {
            projectile.rotation = rotation;
            if (rotate == true)
            {
                rotation -= 20;
                if (projectile.timeLeft <= 480)
                {
                    projectile.velocity.Y += 0.25f;
                }
                else
                {
                    projectile.velocity.Y += 0.1f;
                }
            }

            if(attack == true)
            {
                if (projectile.timeLeft % 60 == 0)
                {
                    for (int x = 0; x < 360; x += 8)
                    {
                        Vector2 pos = new Vector2(16 * 8, 0).RotatedBy(MathHelper.ToRadians(x));
                        Dust.NewDustPerfect(projectile.Center + pos, DustID.Electric, new Vector2(0, 0));
                    }
                    projectile.alpha = 255;
                    projectile.position = projectile.Center;
                    projectile.width = 250;
                    projectile.height = 250;
                    projectile.Center = projectile.position;
                }
                else
                {
                    projectile.alpha = 0;
                    projectile.position = projectile.Center;
                    projectile.width = 16;
                    projectile.height = 16;
                    projectile.Center = projectile.position;

                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
            rotate = false;
            attack = true;
            return false;
        }
    }
}
