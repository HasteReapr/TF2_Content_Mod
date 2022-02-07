using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Medic
{
    class Bonesaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bonesaw");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}
