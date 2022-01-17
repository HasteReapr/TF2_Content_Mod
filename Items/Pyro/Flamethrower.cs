using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Pyro
{
    class Flamethrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyro's Flamethrower");
            Tooltip.SetDefault("\"Hudda hudda huh!\"");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Flamethrower);
            item.damage = 100;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
