using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace TF2_Content.Items.Medic
{
	class Medigun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Medigun");
			Tooltip.SetDefault("Medic's finest achievment");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LastPrism);
			item.magic = true;
			item.mana = 0;
			item.damage = 25;
			item.shoot = ModContent.ProjectileType<Medigun_Holdout>();
			item.shootSpeed = 30f;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<Medigun_Holdout>()] <= 0;
	}
}
