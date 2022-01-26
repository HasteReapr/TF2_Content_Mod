using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TF2_Content.Items.Demo.Projectiles;
using Terraria;

namespace TF2_Content.Items.Demo
{
    class Grenade_Launcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grenade Launcher");
            Tooltip.SetDefault("Kablooie!");
        }

        public override void SetDefaults()
        {
            item.damage = 300;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<Grenade_Pill>();
            item.shootSpeed = 16;
            item.useTime = 20;
            item.useAnimation = 20;
            item.autoReuse = true;
            item.UseSound = SoundID.Item111;
        }

        /*public override bool UseItem(Player player)
        {
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "TF2_Content/Sounds/Custom/Grenade_launcher_shoot");
            return true;
        }*/

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
