﻿using Terraria.ModLoader;
using Terraria.ID;

namespace TF2_Content.Items.Sniper
{
    class Kukri : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kukri");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Excalibur);
        }
    }
}
