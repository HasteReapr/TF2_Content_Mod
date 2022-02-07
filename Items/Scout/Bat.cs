using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Scout
{
    class Bat : ModItem
    { 
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bat");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}
