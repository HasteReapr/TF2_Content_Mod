using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Pyro
{
    class Fireaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireaxe");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}
