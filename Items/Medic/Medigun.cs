using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace TF2_Content.Items.Medic
{
    class Medigun : Mediguns
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medigun");
            Tooltip.SetDefault("Medic's finest achievment");
        }

        public override void SafeSetDefaults()
        {
            item.CloneDefaults(ItemID.LastPrism);
            item.mana = 0;
            item.damage = 25;
            item.shoot = ModContent.ProjectileType<Medigun_Holdout>();
            item.shootSpeed = 30f;
            UberCost = 100;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.GetModPlayer<MedicPlayer>().CurrentUber == UberCost)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Ubercharge>(), 60*8);
                return true;
            }
            else if (player.ownedProjectileCounts[ModContent.ProjectileType<Medigun_Holdout>()] <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
