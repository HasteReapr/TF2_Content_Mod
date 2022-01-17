using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Sniper
{
    class Sniper_Rifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sniper's Sniper Rifle");
            Tooltip.SetDefault("\"Snipin's a good job mate!\"");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.SniperRifle);
        }
    }
}
