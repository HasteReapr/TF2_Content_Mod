using Terraria.ModLoader;
using Terraria.ID;
using TF2_Content.Items.Spy.Projectiles;

namespace TF2_Content.Items.Spy
{
    class Sapper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sapper");
            Tooltip.SetDefault("Zap Zap!\nThrows a sapper which lasts for 10 seconds and damages anything in it's radius.");
        }

        public override void SetDefaults()
        {
            item.damage = 125;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.shoot = ModContent.ProjectileType<Sapper_Projectile>();
            item.shootSpeed = 8;
        }
    }
}
