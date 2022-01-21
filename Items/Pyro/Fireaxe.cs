using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Pyro
{
    class Fireace : ModItem
    {
        public override string Texture => "TF2_Content/Items/Placeholder";
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
