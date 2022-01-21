using Terraria.ModLoader;
using Terraria.ID;
namespace TF2_Content.Items.Soldier
{
    class Shovel : ModItem
    {
        public override string Texture => "TF2_Content/Items/Placeholder"; public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shovel");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}