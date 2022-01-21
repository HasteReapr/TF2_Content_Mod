using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TF2_Content.Items.Spy
{
    class Revolver : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Revolver");
            Tooltip.SetDefault("[tooltip.description]");
        }

        public override void SetDefaults()
        {
            item.damage = 200;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 16f;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
        }
    }
}
