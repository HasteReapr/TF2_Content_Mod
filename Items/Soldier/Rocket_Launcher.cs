using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content.Items.Engineer.Summons;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Soldier
{
    class Rocket_Launcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rocket Launcher");
            Tooltip.SetDefault("Pairs well with a shovel.");
        }

        public override void SetDefaults()
        {
            item.damage = 200;
            item.shootSpeed = 16;
            item.useTime = 15;
            item.useAnimation = 15;
            item.shoot = ModContent.ProjectileType<Sentry_Missile>();
            item.useAmmo = AmmoID.Rocket;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 5f;
            item.autoReuse = false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }
    }
}
