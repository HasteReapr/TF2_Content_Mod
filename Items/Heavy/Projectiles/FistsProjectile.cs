using Terraria;
using Terraria.ModLoader;

namespace TF2_Content.Items.Heavy.Projectiles
{
    class FistsProjectile : ModProjectile
    {
        public override string Texture => "TF2_Content/Items/empty";
        public override void SetDefaults()
        {
            projectile.timeLeft = 1;
            projectile.height = 48;
            projectile.width = 26;
            projectile.penetrate = 50;
            projectile.friendly = true;
            projectile.tileCollide = false;
        }
    }
}
