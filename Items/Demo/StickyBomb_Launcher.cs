using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content.Items.Demo.Projectiles;

namespace TF2_Content.Items.Demo
{
    class StickyBomb_Launcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stickybomb Launcher");
            Tooltip.SetDefault("Left click to shoot the stickybombs.\nRight click with any weapon to detonate stickybombs.");
        }
        public override void SetDefaults()
        {
            item.damage = 400;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<StickyBomb>();
            item.shootSpeed = 12;
            item.useTime = 30;
            item.useAnimation = 30;
            item.autoReuse = true;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/stickybomb_shoot");
        }
    }
}
