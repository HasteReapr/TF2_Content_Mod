using Terraria.ModLoader;
using Terraria;
using TF2_Content.Items.Medic;

namespace TF2_Content.Buffs
{
    class Ubercharge : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ubercharge");
            Description.SetDefault("\"I am bullet proof!\"");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var medic = player.GetModPlayer<MedicPlayer>();
            medic.CurrentUber -= 0.208333333333333f;
        }
    }
}
