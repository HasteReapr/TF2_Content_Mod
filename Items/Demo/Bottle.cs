using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Demo
{
    class Bottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottle");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}
