using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace TF2_Content.Items.Pyro
{
    class Flamethrower : Flamethrowers
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyro's Flamethrower");
            Tooltip.SetDefault("\"Hudda hudda huh!\"");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 300;
        }
    }
}
