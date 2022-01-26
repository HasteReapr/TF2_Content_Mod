using Terraria;
using Terraria.ModLoader;

namespace TF2_Content.Items
{
    class UberDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ubercharged Dye");
            Tooltip.SetDefault("Become shiny!");
        }
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.consumable = false;
        }
    }
}
