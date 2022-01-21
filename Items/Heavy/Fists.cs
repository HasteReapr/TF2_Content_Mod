using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Heavy
{
    class Fists : ModItem
    {
        public override string Texture => "TF2_Content/Items/Placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fists");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}
