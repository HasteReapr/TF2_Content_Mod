using Terraria.ModLoader;
using Terraria;
using TF2_Content.Items.Engineer.Summons;

namespace TF2_Content.Buffs
{
    class Sentry_Buff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sentry Gun");
            Description.SetDefault("\"I love that little gun!\"");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierOne>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierTwo>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonMini>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}