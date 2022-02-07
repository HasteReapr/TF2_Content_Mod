using TF2_Content.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace TF2_Content.Items.Mounts
{
    class TruckKey : ModItem
    {
        public override string Texture => "TF2_Content/Items/Placeholder";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Truck Keys");
			Tooltip.SetDefault("Yeehaw!");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 30000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.mountType = ModContent.MountType<TexasTruckinMount>();
		}
	}
}
